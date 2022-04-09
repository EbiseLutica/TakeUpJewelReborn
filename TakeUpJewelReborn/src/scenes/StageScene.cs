using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public class StageScene : Scene
	{
		public override void OnStart(Dictionary<string, object> args)
		{
			if (!(Core.I.CurrentAreaInfo is AreaInfo area && Core.I.CurrentMap is MapData map))
			{
				// todo: エラーシーンを作成してそこで表示するようにする
				throw new InvalidOperationException();
			}

			InitializeMap(area);
			InitializeHUD();

			messageBox = new Container();
			var backdrop = new Sprite(ResourceManager.MesBox);
			messageBox.Add(backdrop);
			messageBox.Width = (int)backdrop.Width;
			messageBox.Height = (int)backdrop.Height;
			message = new DEText("", Color.Black)
			{
				Location = Vector.One * 16,
			};
			messageBox.Add(message);

			Core.I.Entities.EntityAdded += EntityAdded;
			Core.I.Entities.EntityRemoved += EntityRemoved;
			EventRuntime.PostTeleport += HandleTeleport;

			Core.I.BgmPlay(Core.I.CurrentAreaInfo.Music);

		}

		public override void OnUpdate()
		{
			if (!DF.Window.IsFocused) return;

			Core.I._SetTick(Core.I.Tick + 1);

			if (DFKeyboard.G.IsKeyDown)
				(Core.I.Entities.MainEntity as EntityPlayer)?.SetGod();

			ControlCamera();
			RenderMap();

			var entities = Core.I.Entities;
			entities.Draw();
			entities.Update();

			hud.Text = shadow.Text = GenerateHUD();

			// プレイヤーの死亡処理
			if (!handlingDying && !Core.I.IsFreezing && ((entities.MainEntity is EntityLiving liv && liv.IsDying) || entities.MainEntity.IsDead))
			{
				CoroutineRunner.Start(HandleDying());
			}

			// ゴールハンドリング
			if (!handlingGoal && !handlingDying && !Core.I.IsFreezing && Core.I.IsGoal)
			{
				CoroutineRunner.Start(HandleGoal());
			}

			if ((eventRuntimeIterator == null) || !eventRuntimeIterator.MoveNext())
				eventRuntimeIterator = EventRuntime.Execute();

			if (EventRuntime.BalloonIsShowing && !Root.Contains(messageBox))
			{
				Root.Add(messageBox);
			}
			if (!EventRuntime.BalloonIsShowing && Root.Contains(messageBox))
			{
				Root.Remove(messageBox);
			}
			message.Text = EventRuntime.MesBuffer;

			messageBox.Location = new Vector(
				Const.Width / 2 - messageBox.Width / 2,
				EventRuntime.MesPos == EventRuntime.UpDown.Up
					? 2
					: Const.Height - messageBox.Height - 2
			);

			stage.Location = Core.I.Camera;
		}

		public override void OnDestroy()
		{
			Core.I.Entities.EntityAdded -= EntityAdded;
			Core.I.Entities.EntityRemoved -= EntityRemoved;
			EventRuntime.PostTeleport -= HandleTeleport;
		}

		private void HandleTeleport(EventArgs e)
		{
			DF.Router.ChangeScene<StageScene>();
		}

		private IEnumerator HandleDying()
		{
			handlingDying = true;
			Core.I.BgmPlay("jingle_miss.mid");
			yield return new WaitForSeconds(5);
			Core.I.LoadLevel(Core.I.CurrentLevel, Core.I.CurrentArea);
			Root.Clear();
			DF.Router.ChangeScene<StageScene>();
		}

		private IEnumerator HandleGoal()
		{
			handlingGoal = true;
			Core.I.BgmPlay("jingle_goal.mid");

			var time = 0f;
			var main = Core.I.Entities.MainEntity;
			while (true)
			{
				time += Time.DeltaTime;
				if (time > 7) break;

				main.Velocity = new Vector(1.4f, main.Velocity.Y);
				if (main is EntityLiving l && l.CollisionRight() == ColliderType.Land)
				{
					l.Velocity = new Vector(0, l.Velocity.Y);
				}
				yield return null;
			}
			if (Core.I.NextLevel == -1)
			{
				//todo エピローグを実装する
				DF.Router.ChangeScene<TitleScene>();
				Core.I.IsGoal = false;
				yield break;
			}
			Core.I.LoadLevel(Core.I.NextLevel);
			Core.I.IsGoal = false;
			DF.Router.ChangeScene<PreStageScene>();
		}

		private void EntityAdded(object? sender, Entity e)
		{
			if (sender is EntityList list && e is EntityVisible visible && list.GetDrawableByEntity(visible) is ElementBase el)
				entitiesLayer.Add(el);
		}

		private void EntityRemoved(object? sender, Entity e)
		{
			if (sender is EntityList list && e is EntityVisible visible && list.GetDrawableByEntity(visible) is ElementBase el)
				entitiesLayer.Remove(el);
		}

		private void RenderMap()
		{
			if (!(Core.I.CurrentMap is MapData map)) return;

			var chips = map.Chips;
			backTile.Clear();
			foreTile.Clear();
			for (var y = 0; y < map.Size.Y; y++)
				for (var x = 0; x < map.Size.X; x++)
				{
					var abs = Core.I.Camera + new Vector(x, y) * 16;

					if (abs.X <= -16 || abs.Y <= -16 || abs.X >= Const.Width || abs.Y >= Const.Height)
						continue;
					backTile[x, y] = Core.I.Mpts[chips[x, y, 1]];
					foreTile[x, y] = Core.I.Mpts[chips[x, y, 0]];
				}
		}

		private void InitializeHUD()
		{
			hud = new DEText("", Color.White);
			shadow = new DEText("", Color.Black)
			{
				Location = Vector.One
			};

			Root.Add(shadow);
			Root.Add(hud);
		}

		private string GenerateHUD()
		{
			return $@"①{Core.I.Coin} {(Core.I.CurrentGender == PlayerGender.Male ? "Alen" : "Lucy")}×∞{new string('♥', (Core.I.Entities.MainEntity as EntityPlayer)?.Life ?? 0)}
Level {Core.I.CurrentLevel}-{Core.I.CurrentArea} ⌚{Core.I.Time} {Time.Fps}FPS";
		}

		private void InitializeMap(AreaInfo area)
		{
			// Background
			var bg = new Sprite(ResourceManager.LoadTexture(area.BG));
			Root.Add(bg);

			// タイル
			backTile = new Tilemap((16, 16));
			foreTile = new Tilemap((16, 16));

			RenderMap();

			// エンティティ

			stage.Add(backTile);

			foreach (var d in Core.I.Entities.Drawables)
				entitiesLayer.Add(d);

			stage.Add(entitiesLayer);

			stage.Add(foreTile);

			// Foreground
			if (area.FG is string fg)
			{
				var f = new Sprite(ResourceManager.LoadTexture(fg));
				Root.Add(f);
			}

			var main = Core.I.Entities.MainEntity;

			if (Core.I.Middle != Vector.Zero)
			{
				main.Location = Core.I.Middle;
			}

			if (main is EntityPlayer player)
			{
				player.Form = Core.I.CurrentForm;
			}

			Core.I.Camera = -main.Location;

			Root.Add(stage);
		}

		private void ControlCamera()
		{
			var main = Core.I.Entities.MainEntity;
			if (main is EntityLiving living && living.IsDying) return;

			if (Core.I.CurrentMap == null) return;

			if ((main.Location.X + Core.I.Camera.X > Const.Width / 2) && (Core.I.Camera.X > -Core.I.CurrentMap.Size.X * 16 + Const.Width))
				Core.I.Camera = new Vector(-(int)main.Location.X + Const.Width / 2, Core.I.Camera.Y);

			if ((Core.I.CurrentMap.Size.X * 16 - main.Location.X > Const.Width / 2) && (Core.I.Camera.X < 0))
				Core.I.Camera = new Vector(-(int)main.Location.X + Const.Width / 2, Core.I.Camera.Y);

			if ((main.Location.Y + Core.I.Camera.Y > Const.Height / 2) && (Core.I.Camera.Y > -Core.I.CurrentMap.Size.Y * 16 + Const.Height))
				Core.I.Camera = new Vector(Core.I.Camera.X, -(int)main.Location.Y + Const.Height / 2);

			if ((Core.I.CurrentMap.Size.Y * 16 - main.Location.Y > Const.Height / 2) && (Core.I.Camera.Y < 0))
				Core.I.Camera = new Vector(Core.I.Camera.X, -(int)main.Location.Y + Const.Height / 2);

			if (Core.I.Camera.X > 0)
				Core.I.Camera = new Vector(0, Core.I.Camera.Y);

			if (Core.I.Camera.Y > 0)
				Core.I.Camera = new Vector(Core.I.Camera.X, 0);

			if (Core.I.Camera.X < -Core.I.CurrentMap.Size.X * 16 + Const.Width)
				Core.I.Camera = new Vector(-Core.I.CurrentMap.Size.X * 16 + Const.Width, Core.I.Camera.Y);

			if (Core.I.Camera.Y < -Core.I.CurrentMap.Size.Y * 16 + Const.Height)
				Core.I.Camera = new Vector(Core.I.Camera.X, -Core.I.CurrentMap.Size.Y * 16 + Const.Height);
		}

		private Container stage = new Container();
		private Container entitiesLayer = new Container();
		private Tilemap backTile;
		private Tilemap foreTile;
		private bool handlingDying;
		private DEText hud, shadow;
		private bool handlingGoal;
		private Sprite prompt;
		private Container messageBox;
		private DEText message;
		private IEnumerator eventRuntimeIterator;
	}
}
