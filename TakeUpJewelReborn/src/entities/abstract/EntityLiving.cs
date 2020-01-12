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
		/// 死んでいる途中かどうか。
		/// </summary>
		public bool IsDying;

		public bool IsInWater;

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


		public virtual void UpdateGravity()
		{
			Velocity.Y += Gravity;
			if (IsInWater)
				Velocity.Y -= 0.03f;
		}

		public override void OnUpdate()
		{
			UpdateGravity();
			CheckCollision();
			IsInWater = GetIsInWater();
			if (IsDying)
				Dying();
			if (IsInWater && !BIsInWater)
				OnIntoWater();
			if (!IsInWater && BIsInWater)
				OnOutOfWater();

			base.OnUpdate();
		}

		public override void Backup()
		{
			BIsInWater = IsInWater;
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
		/// 当たり判定を計算します。
		/// </summary>
		public virtual void CheckCollision()
		{
			if (Core.I.CurrentMap == null)
				return;
			if (Location.Y > Core.I.CurrentMap.Size.Y * 16)
				Kill(true, false);
			CollisionTop();
			CollisionBottom();
			CollisionLeft();
			CollisionRight();
		}

		/// <summary>
		/// 上の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionTop()
		{
			int x, y;
			var retval = ColliderType.Air;

			for (x = (int)(Location.X + Collision.Left) + (int)Collision.Width / 4;
				x < (int)(Location.X + Collision.Right);
				x += (int)Collision.Width / 2)
			{
				y = (int)(Location.Y + Collision.Y);
				var hit = Misc.CheckHit(x, y);
				switch (hit)
				{
					case ColliderType.Land:
						Location.Y++;
						Velocity.Y = 0;
						if (this is EntityPlayer)
							if (IsJumping && (Map[x / 16, y / 16, 0] == 9)) //ブロック破壊
							{
								DESound.Play(Sounds.Destroy);
								Map[x / 16, y / 16, 0] = 0;
								Particle.BrokenBlock(new Point(x, y), Parent, Mpts);
							}
						break;
					case ColliderType.NeedleLike:
						Kill();
						goto case ColliderType.Land;
					case ColliderType.PoisonLike:
						Kill();
						goto case ColliderType.Land;
				}
				if (Misc.CheckHit(x, y - 1) == ColliderType.Land)
					retval = ColliderType.Land;
			}
			foreach (IScaffold sc in Parent.FindEntitiesByType<IScaffold>())
			{
				if (sc == this)
					continue;
				if (new Rectangle((int)(Location.X + Collision.Left), (int)(Location.Y + Collision.Y), (int)Collision.Width, 1)
					.CheckCollision(
						new Rectangle((int)(sc.Location.X + sc.Collision.Left), (int)(sc.Location.Y + sc.Collision.Y),
							(int)sc.Collision.Width, (int)sc.Collision.Height)))
				{
					Location.Y++;
					Velocity.Y = 0;
				}
			}
			return retval;
		}

		/// <summary>
		/// 下の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionBottom()
		{
			int x, y;
			var retval = ColliderType.Air;
			IsOnLand = false;
			for (x = (int)(Location.X + Collision.Left) + (int)Collision.Width / 4;
				x < (int)(Location.X + Collision.Right);
				x += (int)Collision.Width / 2)
			{
				y = (int)(Location.Y + Collision.Y + Collision.Height);
				var hit = Misc.CheckHit(x, y);

				switch (hit)
				{
					case ColliderType.Land:
						if (Mpts[Map[x / 16, (y - 1) / 16, 0]].CheckHit(x % 16, (y - 1) % 16) == ColliderType.Land)
							Location.Y--;
						IsOnLand = true;
						IsJumping = false;
						retval = ColliderType.Land;
						if (Velocity.Y > 0)
							Velocity.Y = 0;
						break;
					case ColliderType.NeedleLike:
						Kill();
						goto case ColliderType.Land;
					case ColliderType.PoisonLike:
						Kill();
						goto case ColliderType.Land;
				}
				if (Misc.CheckHit(x, y + 1) == ColliderType.Land)
					retval = ColliderType.Land;
			}
			foreach (IScaffold sc in Parent.FindEntitiesByType<IScaffold>())
			{
				if (sc == this)
					continue;
				if (new Rectangle((int)(Location.X + Collision.Left), (int)(Location.Y + Collision.Bottom), (int)Collision.Width,
					1).CheckCollision(
					new Rectangle((int)(sc.Location.X + sc.Collision.Left), (int)(sc.Location.Y + sc.Collision.Y),
						(int)sc.Collision.Width, (int)sc.Collision.Height)))
				{
					Location.Y--;
					IsOnLand = true;
					if (Velocity.Y > 0)
						Velocity.Y = 0;
					retval = ColliderType.Land;
				}
			}
			return retval;
		}

		public abstract void SetKilledAnime();
		public abstract void SetCrushedAnime();


		/// <summary>
		/// 左の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionLeft()
		{
			int x, y;
			var retval = ColliderType.Air;

			for (y = (int)(Location.Y + Collision.Top) + Size.Height / 6;
				y < (int)(Location.Y + Collision.Bottom);
				y += (int)Collision.Height / 3)
			{
				x = (int)(Location.X + Collision.X);
				var hit = Misc.CheckHit(x, y);
				switch (hit)
				{
					case ColliderType.Land:
						if (Mpts[Map[x / 16, (y - 1) / 16, 0]].CheckHit(x % 16, (y - 1) % 16) == ColliderType.Air)
							Location.Y -= this is EntityPlayer && (((EntityPlayer)this).Form == PlayerForm.Big) ? 2 : 1;
						else
							Location.X++;
						Velocity.X = 0;
						retval = ColliderType.Land;
						break;
					case ColliderType.NeedleLike:
						Kill();
						goto case ColliderType.Land;
					case ColliderType.PoisonLike:
						Kill();
						goto case ColliderType.Land;
				}
				if (Misc.CheckHit(x - 1, y) == ColliderType.Land)
					retval = ColliderType.Land;
			}
			foreach (IScaffold sc in Parent.FindEntitiesByType<IScaffold>())
			{
				if (sc == this)
					continue;
				if (new Rectangle((int)(Location.X + Collision.Left), (int)(Location.Y + Collision.Y), 1, (int)Collision.Height)
					.CheckCollision(
						new Rectangle((int)(sc.Location.X + sc.Collision.Left), (int)(sc.Location.Y + sc.Collision.Y),
							(int)sc.Collision.Width, (int)sc.Collision.Height)))
				{
					Location.X++;
					Velocity.X = 0;
					retval = ColliderType.Land;
				}
			}
			return retval;
		}

		/// <summary>
		/// 右の当たり判定を計算します。
		/// </summary>
		public virtual ColliderType CollisionRight()
		{
			int x, y;
			var retval = ColliderType.Air;

			for (y = (int)(Location.Y + Collision.Top) + Size.Height / 6;
				y < (int)(Location.Y + Collision.Bottom);
				y += (int)Collision.Height / 3)
			{
				x = (int)(Location.X + Collision.X + Collision.Width);
				var hit = Misc.CheckHit(x, y);
				switch (hit)
				{
					case ColliderType.Air:
						break;
					case ColliderType.Land:
						if (Mpts[Map[x / 16, (y - 1) / 16, 0]].CheckHit(x % 16, (y - 1) % 16) == ColliderType.Air)
							Location.Y -= this is EntityPlayer && (((EntityPlayer)this).Form == PlayerForm.Big) ? 2 : 1;
						else
							Location.X--;
						Velocity.X = 0;
						retval = ColliderType.Land;
						break;
					case ColliderType.NeedleLike:
						Kill();
						goto case ColliderType.Land;
					case ColliderType.PoisonLike:
						Kill();
						goto case ColliderType.Land;
				}
				if (Misc.CheckHit(x + 1, y) == ColliderType.Land)
					retval = ColliderType.Land;
			}
			foreach (IScaffold sc in Parent.FindEntitiesByType<IScaffold>())
			{
				if (sc == this)
					continue;
				if (new Rectangle((int)(Location.X + Collision.Left), (int)(Location.Y + Collision.Y), 1, (int)Collision.Height)
					.CheckCollision(
						new Rectangle((int)(sc.Location.X + sc.Collision.Left), (int)(sc.Location.Y + sc.Collision.Y),
							(int)sc.Collision.Width, (int)sc.Collision.Height)))
				{
					Location.X--;
					Velocity.X = 0;
					retval = ColliderType.Land;
				}
			}
			return retval;
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