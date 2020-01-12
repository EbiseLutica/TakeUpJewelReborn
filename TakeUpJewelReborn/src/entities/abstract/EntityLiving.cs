using System;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public abstract class EntityLiving : EntitySprite
	{
		private const int Addition = 7;

		public bool BIsInWater;

		public List<AiBase> CollisionAIs = new List<AiBase>();

		/// <summary>
		/// 死んでいる間のタイマー。
		/// </summary>
		public int DyingTick;

		/// <summary>
		/// 地面についているかどうか。
		/// </summary>
		public bool IsOnLand;

		public int FlightingTime;

		/// <summary>
		/// 死んでいる途中かどうか。
		/// </summary>
		public bool IsDying;

		public bool IsUnderWater;

		/// <summary>
		/// ジャンプしているかどうか。
		/// </summary>
		public bool IsJumping;

		public AiBase? MainAi = null;

		/// <summary>
		/// この Entity の重力加速度を取得。基本的に変更せず、葉っぱなど空気抵抗があるものに対応させるときに、派生クラスでオーバーライドするべきです。
		/// </summary>
		public virtual float Gravity => 0.1f;

		/// <summary>
		/// この Entity の当たり判定。
		/// </summary>
		public virtual RectangleF Collision => new RectangleF(new PointF(0, 0), Size);

		public virtual Sounds KilledSound => Sounds.Killed;

		public virtual int DyingMax => 42;


		public abstract void SetKilledAnime();
		public abstract void SetCrushedAnime();

		public virtual void UpdateGravity()
		{
			Velocity.Y += Gravity;
			if (IsUnderWater)
				Velocity.Y -= 0.03f;
		}

		public override void OnUpdate()
		{
			UpdateGravity();

			var top = CollisionTop();
			var bottom = CollisionBottom();
			var left = CollisionLeft();
			var right = CollisionRight();
			if (Core.I.CurrentMap != null && Location.Y > Core.I.CurrentMap.Size.Y * 16)
				Kill(true, false);
			UpdatePhysics(top, bottom, left, right);

			IsUnderWater = GetIsInWater();
			if (IsDying)
				Dying();
			if (IsUnderWater && !BIsInWater)
				OnIntoWater();
			if (!IsUnderWater && BIsInWater)
				OnOutOfWater();

			base.OnUpdate();
		}

		public override void Backup()
		{
			BIsInWater = IsUnderWater;
			base.Backup();
		}

		/// <summary>
		/// 水中にいるかどうかを取得します。
		/// </summary>
		public virtual bool GetIsInWater()
		{
			var x = (int)Location.X + Size.Width / 2;
			var y = (int)Location.Y + Size.Height / 4;


			if (new Point(x, y).IsOutOfRange())
				return false;

			return Mpts[Map[x / 16, y / 16, 0]].CheckHit(x % 16, y % 16) == ColliderType.UnderWater;
		}

		/// <summary>
		/// 水中に入った時に呼ばれます。
		/// </summary>
		public virtual void OnIntoWater()
		{
			DESound.Play(Sounds.WaterSplash);
		}

		/// <summary>
		/// 水中から出た時に呼ばれます。
		/// </summary>
		public virtual void OnOutOfWater()
		{
			DESound.Play(Sounds.WaterSplash);
		}


		public virtual void Dying()
		{
			DyingTick--;
			if (DyingTick <= 0)
			{
				IsDead = true;
				DyingTick = 0;
			}
		}

		/// <summary>
		/// 物理演算を行います。
		/// </summary>
		public virtual void UpdatePhysics(ColliderType top, ColliderType bottom, ColliderType left, ColliderType right)
		{
			if (Location.X < 0)
				Location.X = 0;

			if (Core.I.CurrentMap != null && Location.X > Core.I.CurrentMap.Size.X * 16)
				Location.X = Core.I.CurrentMap.Size.X * 16;

			if (top.IsLandLike() && Velocity.Y < 0)
			{
				// 天井で反発する
				Velocity.Y = 0;
		}

			IsOnLand = bottom.IsLandLike();
			if (IsOnLand && Velocity.Y > 0)
			{
				Velocity.Y = 0;
			}

			// 滞空時間を更新
			FlightingTime = IsOnLand ? 0 : FlightingTime + 1;

			if (left.IsLandLike() && Velocity.X < 0)
			{
				if (CollisionTopLeft().IsLandLike() || FlightingTime > 10)
					Velocity.X = 0;
				else while (Misc.CheckHit((int)(Location.X), (int)(Location.Y + Collision.Bottom)).IsLandLike())
						Location.Y--;
			}

			if (right.IsLandLike() && Velocity.X > 0)
			{
				if (CollisionTopRight().IsLandLike() || FlightingTime > 10)
					Velocity.X = 0;
				else while (Misc.CheckHit((int)(Location.X + Collision.Right), (int)(Location.Y + Collision.Bottom)).IsLandLike())
						Location.Y--;
			}
		}

		/// <summary>
		/// 上の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionTop()
		{
			for (var x = (int)(Location.X + Collision.Left);
				x < (int)(Location.X + Collision.Right);
				x += (int)Collision.Width / 2)
			{
				var y = (int)(Location.Y + Collision.Y);
				var hit = Misc.CheckHit(x, y);
				if (hit != ColliderType.Air)
					return hit;
			}

			return ColliderType.Air;
		}

		/// <summary>
		/// 下の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionBottom()
		{
			for (var x = (int)(Location.X + Collision.Left);
				x < (int)(Location.X + Collision.Right);
				x += (int)Collision.Width / 2)
			{
				var y = (int)(Location.Y + Collision.Y + Collision.Height);
				var hit = Misc.CheckHit(x, y);
				if (hit != ColliderType.Air)
					return hit;
			}

			return ColliderType.Air;
			}

		/// <summary>
		/// 左の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionLeft()
		{
			for (var y = (int)(Location.Y + Collision.Top);
				y < (int)(Location.Y + Collision.Bottom);
				y += (int)Collision.Height / 8)
			{
				var x = (int)(Location.X + Collision.X);
				var hit = Misc.CheckHit(x, y);
				if (hit != ColliderType.Air)
					return hit;
				}
			return ColliderType.Air;
			}

		/// <summary>
		/// 右の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionRight()
			{
			for (var y = (int)(Location.Y + Collision.Top);
				y < (int)(Location.Y + Collision.Bottom);
				y += (int)Collision.Height / 8)
				{
				var x = (int)(Location.X + Collision.X + Collision.Width);
				var hit = Misc.CheckHit(x, y);
				if (hit != ColliderType.Air)
					return hit;
			}
			return ColliderType.Air;
		}

		/// <summary>
		/// 左の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionTopLeft()
		{
			for (var y = (int)(Location.Y + Collision.Top);
				y < (int)(Location.Y + Collision.Bottom / 2);
				y += (int)Collision.Height / 8)
			{
				var x = (int)(Location.X + Collision.X);
				var hit = Misc.CheckHit(x, y);
				if (hit != ColliderType.Air)
					return hit;
				}
			return ColliderType.Air;
			}

		/// <summary>
		/// 右の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionTopRight()
			{
			for (var y = (int)(Location.Y + Collision.Top);
				y < (int)(Location.Y + Collision.Bottom / 2);
				y += (int)Collision.Height / 8)
				{
				var x = (int)(Location.X + Collision.X + Collision.Width);
				var hit = Misc.CheckHit(x, y);
				if (hit != ColliderType.Air)
					return hit;
			}
			return ColliderType.Air;
		}

		public override void Kill()
		{
			var e = new PreEventArgs();
			EntityLiving.PreKill?.Invoke(this, e);
			if (e.IsCanceled) return;

			IsDying = true;
			DyingTick = DyingMax;
			if (IsDying)
				SetKilledAnime();
			if (IsCrushed)
				SetCrushedAnime();
			if (!IsCrushed && !IsFall)
				DESound.Play(KilledSound);
			Velocity = Vector.Zero;
		}

		public override void OnUpdate(Vector p, IDrawable d)
		{
			base.OnUpdate(p, d);
			if (IsDying)
			{
				OnDyingAnimation(d);
			}
		}

		public virtual void OnDyingAnimation(IDrawable d)
		{
			if (DyingMax == 0) return;
			var lerp = DFMath.Lerp((DyingMax - DyingTick) / (float)DyingMax, 1, 0);
			if (d is Sprite s)
			{
				s.Color = Color.FromArgb(255, (int)(lerp * 255), (int)(lerp * 255));
				Location += Vector.Right * lerp;
			}
		}

		public static event EventHandler<PreEventArgs>? PreKill;
	}
}