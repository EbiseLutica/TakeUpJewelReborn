using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotFeather;

namespace TakeUpJewel
{

	public class Weapons : EntityFlying
	{
		public int Life = 40;

		public Weapons(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(8, 8);
			SetGraphic(2);
			Entity ent = null;
			var min = float.MaxValue;

			if (Parent.Count(en => en is Weapons) > 3)
				Kill();

			foreach (var e in from entity in Parent
							  where entity.MyGroup == EntityGroup.Enemy
							  select entity)
				if (Math.Abs(Location.X - e.Location.X) < min)
				{
					min = Math.Abs(Location.X - e.Location.X);
					ent = e;
				}

			var r = ent != null
				? Math.Atan2(Location.Y - ent.Location.Y, Location.X - ent.Location.X)
				: DFMath.ToRadian(Core.GetRand(360));

			float x = -(float)Math.Cos(r) * 5,
				y = -(float)Math.Sin(r) * 5;

			Velocity = new Vector(x, y);
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
			//TODO: ここにこの Entity が行う処理を記述してください。
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