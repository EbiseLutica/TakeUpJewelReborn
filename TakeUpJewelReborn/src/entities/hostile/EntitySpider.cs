using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Spider", 11)]
	public class EntitySpider : EntityFlying
	{
		protected float Accel = 0.1f;

		public EntitySpider(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = new Vector(pnt.X, pnt.Y + 64);
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			CollisionAIs.Add(new AiKillDefender(this));
		}

		public override Texture2D[] ImageHandle => ResourceManager.Spider;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override void UpdatePhysics(ColliderType top, ColliderType bottom, ColliderType left, ColliderType right)
		{
			if (!IsDying)
				base.UpdatePhysics(top, bottom, left, right);
		}

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void OnUpdate()
		{
			if (!IsDying)
			{
				Velocity.Y += Accel;
				if (Velocity.Y >= 0.8f)
					Accel = -0.01f;
				else if (Velocity.Y <= -0.8f)
					Accel = 0.01f;
			}
			base.OnUpdate();
		}

		public override void Kill()
		{
			base.Kill();
			Velocity.Y = 0;
		}
	}
}