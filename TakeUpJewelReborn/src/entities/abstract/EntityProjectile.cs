using System;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public abstract class EntityProjectile : EntitySprite
	{
		private Vector _bcol;

		private double _rad;

		protected int Alive;

		public bool BIsInWater;

		public bool IsInWater;

		public bool IsStucked;

		/// <summary>
		/// この Entity の重力加速度を取得。基本的に変更せず、葉っぱなど空気抵抗があるものに対応させるときに、派生クラスでオーバーライドするべきです。
		/// </summary>
		public virtual float Gravity => 0.1f;

		public virtual Vector Collision
		{
			get { return _bcol = new Vector((float)Math.Cos(_rad) * 4, (float)Math.Sin(_rad) * 4); }
		}

		/// <summary>
		/// 移動を停止してから存在している時間を取得します。
		/// </summary>
		public virtual int AliveTime => 150;

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

			return Mpts[Map[x / 16, y / 16, 0]].CheckHit(x % 16, y % 16) == ObjectHitFlag.UnderWater;
		}

		public virtual void UpdateGravity()
		{
			Velocity.Y += Gravity;
			if (IsInWater)
				Velocity.Y -= 0.03f;
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

		public override void OnUpdate()
		{
			IsInWater = GetIsInWater();
			UpdateGravity();

			var x = (int)(Collision.X + Location.X);
			var y = (int)(Collision.Y + Location.Y);
			if (Mpts[Map[x / 16, y / 16, 0]].CheckHit(x % 16, y % 16) == ObjectHitFlag.Land)
			{
				Velocity = Vector.Zero;
				if (!IsStucked)
					OnStucked();
			}
			if (IsStucked)
			{
				Alive--;
				if (Alive <= 0)
					IsDead = true;
			}
			if (IsInWater && !BIsInWater)
				OnIntoWater();
			if (!IsInWater && BIsInWater)
				OnOutOfWater();
			base.OnUpdate();
		}


		public virtual void OnStucked()
		{
			IsStucked = true;
			Alive = AliveTime;
		}
	}
}