using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public class StageScene : Scene
	{
		public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
		{
			if (!(Core.I.CurrentAreaInfo is AreaInfo area && Core.I.CurrentMap is MapData map))
			{
				// todo: エラーシーンを作成してそこで表示するようにする
				throw new InvalidOperationException();
			}

			InitializeMap(area);
			InitializeHUD();

			Core.I.Entities.EntityAdded += EntityAdded;
			Core.I.Entities.EntityRemoved += EntityRemoved;

			Core.I.BgmPlay(Core.I.CurrentAreaInfo.Music);
		}

		public override void OnUpdate(Router router, GameBase game, DFEventArgs e)
		{
			if (!game.IsFocused) return;

			Core.I._SetTick(Core.I.Tick + 1);

			ControlCamera();
			RenderMap();

			var entities = Core.I.Entities;
			entities.Draw();
			entities.Update();

			hud.Text = shadow.Text = GenerateHUD();

			// プレイヤーの死亡処理
			if (!handlingDying && !Core.I.IsFreezing && ((entities.MainEntity is EntityLiving liv && liv.IsDying) || entities.MainEntity.IsDead))
			{
				game.StartCoroutine(HandleDying(router, game));
			}

			// ゴールハンドリング
			if (!handlingGoal && !handlingDying && !Core.I.IsFreezing && Core.I.IsGoal)
			{
				game.StartCoroutine(HandleGoal(router, game));
			}

			stage.Location = Core.I.Camera;
		}

		public override void OnDestroy(Router router)
		{
			Core.I.Entities.EntityAdded -= EntityAdded;
			Core.I.Entities.EntityRemoved -= EntityRemoved;
		}

		private IEnumerator HandleDying(Router router, GameBase game)
		{
			handlingDying = true;
			Core.I.BgmPlay("zannnenn.mid");
			yield return new WaitForSeconds(5);
			Core.I.LoadLevel(Core.I.CurrentLevel, Core.I.CurrentArea);
			Root.Clear();
			router.ChangeScene<StageScene>();
		}

		private IEnumerator HandleGoal(Router router, GameBase game)
		{
			handlingGoal = true;
			Core.I.BgmPlay("jingle_gameclear.mid");

			var time = 0f;
			var main = Core.I.Entities.MainEntity;
			while (true)
			{
				time += Time.DeltaTime;
				if (time > 8) break;

				main.Velocity = Vector.Right * 1.4f;

				yield return null;
			}
		}

		private void EntityAdded(object? sender, Entity e)
		{
			if (sender is EntityList list && e is EntityVisible visible && list.GetDrawableByEntity(visible) is IDrawable d)
				entitiesLayer.Add(d);
		}

		private void EntityRemoved(object? sender, Entity e)
		{
			if (sender is EntityList list && e is EntityVisible visible && list.GetDrawableByEntity(visible) is IDrawable d)
				entitiesLayer.Remove(d);
		}

		private void RenderMap()
		{
			if (!(Core.I.CurrentMap is MapData map)) return;

			var chips = map.Chips;
			for (var y = 0; y < map.Size.Y; y++)
				for (var x = 0; x < map.Size.X; x++)
				{
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
Level {Core.I.CurrentLevel}-{Core.I.CurrentArea} ⌚{Core.I.Time}";
		}

		private void InitializeMap(AreaInfo area)
		{
			// Background
			var bg = new Sprite(ResourceManager.LoadTexture(area.BG));
			bg.ZOrder = -2;
			Root.Add(bg);

			// タイル
			backTile = new Tilemap(Vector.One * 16);
			foreTile = new Tilemap(Vector.One * 16);

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
				f.ZOrder = 2;
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

			if ((main.Location.X + Core.I.Camera.X > Const.Width / 2) &&
							(Core.I.Camera.X > -Core.I.CurrentMap.Size.X * 16 + Const.Width))
				Core.I.Camera = new Vector(-(int)main.Location.X + Const.Width / 2, Core.I.Camera.Y);

			if ((Core.I.CurrentMap.Size.X * 16 - main.Location.X > Const.Width / 2) &&
				/*main.Velocity.X < 0 &&*/ (Core.I.Camera.X < 0))
				Core.I.Camera = new Vector(-(int)main.Location.X + Const.Width / 2, Core.I.Camera.Y);

			if ((main.Location.Y + Core.I.Camera.Y > Const.Height / 2) &&
				/*main.Velocity.Y > 0 &&*/
				(Core.I.Camera.Y > -Core.I.CurrentMap.Size.Y * 16 + Const.Height))
				Core.I.Camera = new Vector(Core.I.Camera.X, -(int)main.Location.Y + Const.Height / 2);

			if ((Core.I.CurrentMap.Size.Y * 16 - main.Location.Y > Const.Height / 2) &&
				/*main.Velocity.Y < 0 &&*/ (Core.I.Camera.Y < 0))
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
	}
}
