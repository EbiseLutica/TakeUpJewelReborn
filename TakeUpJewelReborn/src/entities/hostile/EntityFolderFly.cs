using System;
using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using Object = TakeUpJewel.Data.Object;

namespace TakeUpJewel.Entities
{
	[EntityRegistry("FolderFly", 86)]
	public class EntityFolderFly : EntityFlying
	{
		private int _tick;

		public EntityFolderFly(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			MainAi = new AiFlySine(this, 1, 0, 1, 0, 1);
			CollisionAIs.Add(new AiKillDefender(this));
		}

		public override Texture2D[] ImageHandle => ResourceManager.FolderFly;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override void SetKilledAnime()
		{
			AnimeSpeed = 0;
		}

		public override void SetCrushedAnime()
		{
			AnimeSpeed = 0;
			IsCrushed = false;
		}

		public override void OnUpdate()
		{
			//TODO: ここにこの Entity が行う処理を記述してください。
			if ((_tick > 140) && !IsDying)
			{
				_tick = 0;
				DESound.Play(Sounds.ShootArrow);
				var r = Math.Atan2(Location.Y - Parent.MainEntity.Location.Y, Location.X - Parent.MainEntity.Location.X);
				float x = -(float)Math.Cos(r) * 2.2f,
					y = -(float)Math.Sin(r) * 2.2f;

				Parent.Add(new EntityDocument(Location, Mpts, Map, Parent) { Velocity = new Vector(x, y) });
			}
			_tick++;

			base.OnUpdate();
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			return this;
		}
	}
}