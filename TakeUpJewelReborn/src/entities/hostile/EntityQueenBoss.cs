using System.Drawing;
using System.Globalization;
using DotFeather;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.DevelopmentUtility;
using Object = TakeUpJewel.Data.Object;

namespace TakeUpJewel.Entities
{
	public enum QueenBossBehaviorOption
	{
		Waiting1,
		ThrowWeaponToLeft,
		MoveToLeft,
		Waiting2,
		ThrowWeaponToRight,
		MoveToRight
	}

	public enum KingBossBehaviorOption
	{
		JumpToLeft,
		Waiting1,
		ThrowChocolateToRight,
		JumpToRight,
		Waiting2,
		ThrowChocolateToLeft
	}

	[EntityRegistry("QueenBoss", 92)]
	public class EntityQueenBoss : EntityLiving
	{
		public const int MaxLife = 20;

		private readonly float _left;
		private string _endScript;

		private Vector _firstLoc;
		private bool _isBattling;

		private QueenBossBehaviorOption _nowBehavior;

		private int _startKyori = 256;

		private string _startScript;
		private float _top;
		protected float InternalGravity;


		public override int DyingMax => 0;

		/// <summary>
		/// 無敵時間。0のときは無敵ではないが、0以上の時は無敵である。
		/// </summary>
		public int MutekiTime;

		protected int Tick;

		public EntityQueenBoss(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
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
			_left = _firstLoc.X - 200;
			Size = new Size(12, 30);
		}

		public int Life { get; set; } = MaxLife;

		public override Texture2D[] ImageHandle => ResourceUtility.Queen;


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

				return;
			}
			switch (_nowBehavior)
			{
				case QueenBossBehaviorOption.Waiting1:
					if (Tick == 0)
					{
						Tick = 60;
						_nowBehavior = QueenBossBehaviorOption.ThrowWeaponToLeft;
						SetGraphic(16);
					}
					break;
				case QueenBossBehaviorOption.ThrowWeaponToLeft:
					if (Tick == 30)
					{
						DESound.Play(Sounds.ShootArrow);
						SetGraphic(17);
						Parent.Add(
							new EntityCircusBall(Location, Mpts, Map, Parent).SetEntityData(
								JObject.FromObject(new { SpeedX = -2.0f })));
					}
					if (Tick == 0)
					{
						_nowBehavior = QueenBossBehaviorOption.MoveToLeft;
						SetAnime(0, 3, 8);
						Velocity.X = -1.6f;
					}
					break;
				case QueenBossBehaviorOption.MoveToLeft:
					if ((Location.X <= _left - 16) || (Location.X <= 0) || (CollisionLeft() == ObjectHitFlag.Hit))
					{
						Tick = 60;
						_nowBehavior = QueenBossBehaviorOption.Waiting2;
						SetGraphic(6);
						Velocity.X = 0;
					}
					break;
				case QueenBossBehaviorOption.Waiting2:
					if (Tick == 0)
					{
						Tick = 60;
						_nowBehavior = QueenBossBehaviorOption.ThrowWeaponToRight;
						SetGraphic(18);
					}
					break;
				case QueenBossBehaviorOption.ThrowWeaponToRight:
					if (Tick == 30)
					{
						DESound.Play(Sounds.ShootArrow);
						Parent.Add(
							new EntityCircusBall(Location, Mpts, Map, Parent).SetEntityData(
								JObject.FromObject(new { SpeedX = 2.0f })));
						SetGraphic(19);
					}
					if (Tick == 0)
					{
						_nowBehavior = QueenBossBehaviorOption.MoveToRight;
						SetAnime(6, 9, 8);
						Velocity.X = 1.6f;
					}
					break;
				case QueenBossBehaviorOption.MoveToRight:
					if (((Location.X >= _firstLoc.X) && (Location.X >= Core.I.CurrentMap.Size.X - 1)) ||
						(CollisionRight() == ObjectHitFlag.Hit))
					{
						Tick = 60;
						_nowBehavior = QueenBossBehaviorOption.Waiting1;
						SetGraphic(6);
						Velocity.X = 0;
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

			DESound.Play(Sounds.PowerDown);

			MutekiTime = 180;

			Life -= IsCrushed ? MaxLife / 3 : 1;
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