using System;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public class EntityFireWeapon : EntityLiving
	{
		public const int SpeedX = 4;

		private int _defspeed;
		public int Life = 20;

		public EntityFireWeapon(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(8, 8);
			SetAnime(3, 6, 8);
			var vec = new Vector();
			if (DFKeyboard.Up)
				vec.Y = -1;
			if (DFKeyboard.Down)
				vec.Y = 1;
			if (DFKeyboard.Left)
				vec.X = -1;
			else if (DFKeyboard.Right)
				vec.X = 1;

			vec *= 4f;

			Velocity = vec;
		}

		public override Texture2D[] ImageHandle => ResourceManager.Particle;


		public override EntityGroup MyGroup => EntityGroup.DefenderWeapon;

		public override Sounds KilledSound => Sounds.Null;

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
			if ((int)Velocity.X == 0)
				Velocity.X = _defspeed * 4f;

			base.OnUpdate();
		}

		private void UpdateBehavior()
		{
			if (Location.X < 0)
				Kill();

			if (Location.X > Core.I.CurrentMap.Size.X * 16 - 16)
				Kill();

			if (Location.Y < 0)
			{
				Velocity.Y = 0;
				Location.Y = 0;
			}

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
			if (CollisionBottom() == ObjectHitFlag.Hit)
			{
				Velocity.Y = -2.5f;
				Life--;
			}
			if ((CollisionLeft() == ObjectHitFlag.Hit) || (CollisionRight() == ObjectHitFlag.Hit))
				Kill();
		}

		public override void Dying()
		{
			IsDead = true;
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			if (jsonobj.IsDefined("SpeedX"))
				_defspeed = Math.Sign((float)jsonobj.SpeedX);
			return this;
		}
	}
}