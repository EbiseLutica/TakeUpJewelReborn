using System;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("KingBoss", 93)]
	public class EntityKingBoss : EntityLiving
	{
		public const int MaxLife = 40;

		private readonly float _left;
		private readonly float _top;
		private readonly float _waza1;
		private readonly float _waza2;
		private readonly float _waza3;
		private string _endScript;

		private Vector _firstLoc;
		private bool _isBattling;

		private KingBossBehaviorOption _nowBehavior;

		private int _startKyori = 256;

		private string _startScript;
		protected float InternalGravity;

		public override int DyingMax => 0;

		/// <summary>
		/// 無敵時間。0のときは無敵ではないが、0以上の時は無敵である。
		/// </summary>
		public int MutekiTime;

		protected int Tick;

		public EntityKingBoss(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			CollisionAIs.Add(new AiKillDefender(this));
			InternalGravity = 0.1f;
			IsOnLand = true;
			_firstLoc = pnt;
			Tick = 50;
			_left = _firstLoc.X - 192;
			_top = _firstLoc.Y - 128;
			Size = new Size(12, 30);

			var a = (_firstLoc.X - _left) / 5;
			_waza1 = _left + a;
			_waza2 = _waza1 + a;
			_waza3 = _waza2 + a;
		}

		public int Life { get; set; } = MaxLife;

		public override Texture2D[] ImageHandle => ResourceManager.King;


		public override RectangleF Collision => new RectangleF(new Vector(2, 2).ToPoint(), Size);

		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override float Gravity => InternalGravity;

		public override void OnUpdate()
		{
			base.OnUpdate();
			//FontUtility.DrawMiniString((int)Location.X - 9 + GameEngine.camera.X, (int)Location.Y + GameEngine.camera.Y, "" + AnimeSpeed, 0xffffff);


			if (!_isBattling)
			{
				// ReSharper disable once AssignmentInConditionalExpression
				if (_isBattling = Location.GetLengthTo(Parent.MainEntity.Location) < _startKyori)
				{
					try
					{
						EventRuntime.AddScript(new EventScript(_startScript));
					}
					catch (EventScript.EventScriptException ex)
					{
						EventRuntime.AddScript(new EventScript($@"[enstop]
[mesbox:down]
[mes:""エラー！\n{ex.Message.Replace(@"\", @"\\").Replace(@"""", @"\""")}""]
[mesend]
[enstart]"));
					}

					Velocity.X = -1.6f;
					InternalGravity = -0.04f;
					SetAnime(0, 3, 5);
				}

				return;
			}

			switch (_nowBehavior)
			{
				case KingBossBehaviorOption.JumpToLeft:
					if ((Location.Y <= _top) || (Location.Y <= 0) || (CollisionTop() == ObjectHitFlag.Hit))
					{
						Velocity.Y = InternalGravity = 0;
						Location.Y = _top;
					}
					if (Tick % 30 == 0)
					{
						DESound.Play(Sounds.ShootArrow);
						var r = Math.Atan2(Location.Y - Parent.MainEntity.Location.Y, Location.X - Parent.MainEntity.Location.X);
						float x = -(float)Math.Cos(r) * 3.4f,
							y = -(float)Math.Sin(r) * 3.4f;

						Parent.Add(new EntityPlayingCard(Location, Mpts, Map, Parent) { Velocity = new Vector(x, y) });
					}
					if ((Location.X <= _left) || (Location.X <= 0) || (CollisionLeft() == ObjectHitFlag.Hit))
					{
						InternalGravity = 0.1f;
						_nowBehavior = KingBossBehaviorOption.Waiting1;
						Tick = -1;
					}
					break;
				case KingBossBehaviorOption.Waiting1:
					if ((Tick < 0) && (CollisionBottom() == ObjectHitFlag.Hit))
						Tick = 120;

					if (Tick == 100)
						SetGraphic(0);
					if (Tick == 80)
						SetGraphic(6);
					if (Tick == 0)
					{
						_nowBehavior = KingBossBehaviorOption.ThrowChocolateToRight;
						Tick = 120;
						Parent.Add(Core.I.EntityRegister.CreateEntity("Bunyo", new Vector(Location.X, Location.Y - 16), Mpts, Map,
							Parent));

						DESound.Play(Sounds.ItemSpawn);
					}

					break;
				case KingBossBehaviorOption.ThrowChocolateToRight:
					if (Tick <= 0)
					{
						_nowBehavior = KingBossBehaviorOption.JumpToRight;

						Velocity.X = 1.6f;
						InternalGravity = -0.04f;
						SetAnime(6, 9, 5);
					}
					break;
				case KingBossBehaviorOption.JumpToRight:

					if ((Location.Y <= _top) || (Location.Y <= 0) || (CollisionTop() == ObjectHitFlag.Hit))
						Velocity.Y = InternalGravity = 0;
					if (Tick % 30 == 0)
					{
						DESound.Play(Sounds.ShootArrow);
						var r = Math.Atan2(Location.Y - Parent.MainEntity.Location.Y, Location.X - Parent.MainEntity.Location.X);
						float x = -(float)Math.Cos(r) * 3.4f,
							y = -(float)Math.Sin(r) * 3.4f;

						Parent.Add(new EntityPlayingCard(Location, Mpts, Map, Parent) { Velocity = new Vector(x, y) });
					}
					if (((Location.X >= _firstLoc.X) && (Location.X >= Core.I.CurrentMap.Size.X - 1)) ||
						(CollisionRight() == ObjectHitFlag.Hit))
					{
						InternalGravity = 0.1f;
						Velocity.X = 0;
						_nowBehavior = KingBossBehaviorOption.Waiting2;
						Tick = -1;
					}
					break;
				case KingBossBehaviorOption.Waiting2:
					if ((Tick < 0) && (CollisionBottom() == ObjectHitFlag.Hit))
						Tick = 120;

					if (Tick == 100)
						SetGraphic(6);
					if (Tick == 80)
						SetGraphic(0);
					if (Tick == 0)
					{
						_nowBehavior = KingBossBehaviorOption.ThrowChocolateToLeft;
						Parent.Add(Core.I.EntityRegister.CreateEntity("SoulChocolate", new Vector(Location.X, Location.Y - 16), Mpts,
							Map, Parent));
						Tick = 120;

						DESound.Play(Sounds.ItemSpawn);
					}

					break;
				case KingBossBehaviorOption.ThrowChocolateToLeft:
					if (Tick <= 0)
					{
						_nowBehavior = KingBossBehaviorOption.JumpToLeft;


						Velocity.X = -1.6f;
						InternalGravity = -0.04f;
						SetAnime(0, 3, 5);
					}
					break;
			}

			Tick--;

			if (MutekiTime <= 0) return;
			MutekiTime--;
		}

		public override void OnUpdate(Vector p, IDrawable d)
		{
			if (!(d is Sprite sprite)) return;

			var cond = (MutekiTime > 0) && MutekiTime % 8 < 4;

			sprite.Color = Color.FromArgb(cond ? 0 : 255, 255, 255, 255);
		}

		/// <summary>
		/// この EntityPlayer を殺害します。
		/// </summary>
		public override void Kill()
		{
			if (IsDying)
				return;

			if (IsFall)
			{
				base.Kill();
				Velocity = Vector.Zero;
				Life = 0;
				return;
			}


			if (MutekiTime > 0)
				return;

			if (Life < 1)
			{
				try
				{
					EventRuntime.AddScript(new EventScript(_endScript));
				}
				catch (EventScript.EventScriptException ex)
				{
					EventRuntime.AddScript(new EventScript($@"[enstop]
[mesbox:down]
[mes:""エラー！\n{ex.Message.Replace(@"\", @"\\").Replace(@"""", @"\""")}""]
[mesend]
[enstart]"));
				}
				base.Kill();


				return;
			}
			MutekiTime = 240;

			Life -= IsCrushed ? MaxLife / 4 : 1;
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			_startKyori = (int)jsonobj.StartKyori;
			_startScript = jsonobj.StartScript;
			_endScript = jsonobj.EndScript;
			return this;
		}


		public override void SetKilledAnime()
		{
			SetGraphic(5);
		}

		public override void SetCrushedAnime()
		{
			SetGraphic(5);

			IsCrushed = false;
		}
	}
}