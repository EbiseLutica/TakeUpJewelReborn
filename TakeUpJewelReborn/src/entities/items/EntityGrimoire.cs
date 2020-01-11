using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Grimoire", 34)]
	public class EntityGrimoire : EntityLiving
	{
		public EntityGrimoire(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			SetAnime(10, 13, 8);
			Velocity = new Vector(0, -2.0f);
		}

		public override Texture2D[] ImageHandle => ResourceManager.Item;

		public override float Gravity => 0.05f;

		public override EntityGroup MyGroup => EntityGroup.Stage;

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void OnUpdate()
		{
			Velocity.X *= 0.98f;
			foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
			{
				var ep = (EntityPlayer)entity;
				if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
				{
					ep.PowerUp(PlayerForm.Magic);
					IsDead = true;
				}
			}
			base.OnUpdate();
		}
	}
}