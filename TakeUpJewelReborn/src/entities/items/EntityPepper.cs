using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("FireWands", 33)]
	public class EntityPepper : EntityLiving
	{
		public EntityPepper(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			SetAnime(0, 3, 8);
			Velocity = new Vector(0, -3.0f);
		}

		public override Texture2D[] ImageHandle => ResourceManager.Item;

		public override EntityGroup MyGroup => EntityGroup.Stage;

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void OnUpdate()
		{
			foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
			{
				var ep = (EntityPlayer)entity;
				if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
				{
					ep.PowerUp(PlayerForm.Fire);
					IsDead = true;
				}
			}
			base.OnUpdate();
		}
	}
}