using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotFeather;

namespace TakeUpJewel
{
	public class EntityMagicWeapon : EntityFlying
	{
		public int Life = 40;

		public EntityMagicWeapon(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(8, 8);
			SetGraphic(2);

			// ウェポンは同時に3つまでしか打てない
			if (Parent.FindEntitiesByType<EntityMagicWeapon>().Count() > 3)
				Kill();

			// 対象を探す
			// 条件は、プレイヤーに最も近いエネミー
			Entity? target = Parent
				.Where(e => e.MyGroup == EntityGroup.Enemy)
				.OrderBy(e => MathF.Abs(e.Location.Distance(Location)))
				.FirstOrDefault();

			// 対象がいればその方向へ、いなければランダムに射出
			var r = target != null
				? MathF.Atan2(Location.Y - target.Location.Y, Location.X - target.Location.X)
				: DFMath.ToRadian(Core.GetRand(360));

			Velocity = new Vector(MathF.Cos(r), MathF.Sin(r)) * -5;
		}

		public override Texture2D[] ImageHandle => ResourceManager.Weapon;

		public override EntityGroup MyGroup => EntityGroup.DefenderWeapon;

		public override Sounds KilledSound => Sounds.Null;

		public override ObjectHitFlag CollisionBottom() => ObjectHitFlag.NotHit;
		public override ObjectHitFlag CollisionTop() => ObjectHitFlag.NotHit;
		public override ObjectHitFlag CollisionLeft() => ObjectHitFlag.NotHit;
		public override ObjectHitFlag CollisionRight() => ObjectHitFlag.NotHit;

		public sealed override void Kill()
		{
			base.Kill();
		}

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void OnUpdate()
		{
			UpdateBehavior();
			base.OnUpdate();
		}

		private void UpdateBehavior()
		{
			if (Location.X < 0)
				Kill();

			if (Location.X > Core.I.CurrentMap.Size.X * 16 - 16)
				Kill();

			if (Location.Y < 0)
				Kill();

			if (Location.Y > Core.I.CurrentMap.Size.Y * 16)
				Kill(true, false);

			foreach (EntityLiving e in new List<Entity>(Parent.FindEntitiesByType<EntityLiving>()))
				if (!e.IsDying && (e.MyGroup == EntityGroup.Enemy) &&
					new RectangleF(Location.ToPoint(), Size).CheckCollision(new RectangleF(e.Location.ToPoint(), e.Size)))
				{
					e.Kill();
					Kill();
				}
			if (Life < 1)
				Kill();
			if (Math.Abs(Location.X - Parent.MainEntity.Location.X) > Const.Width / 1.7)
				Kill();
			if (CollisionBottom() == ObjectHitFlag.Hit)
				Kill();
			if ((CollisionLeft() == ObjectHitFlag.Hit) || (CollisionRight() == ObjectHitFlag.Hit))
				Kill();
			Life--;
		}

		public override void Dying()
		{
			IsDead = true;
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			if (jsonobj.IsDefined("SpeedX"))
				Velocity.X = (float)jsonobj.SpeedX;
			if (jsonobj.IsDefined("SpeedY"))
				Velocity.Y = (float)jsonobj.SpeedY;
			return this;
		}
	}
}