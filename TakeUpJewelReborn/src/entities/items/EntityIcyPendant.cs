using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("IcyPendant", -1)]
	public class EntityIcyPendant : EntityLiving
	{
		public EntityIcyPendant(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			SetGraphic(4);
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
					ep.PowerUp(PlayerForm.Ice);
					IsDead = true;
				}
			}
			base.OnUpdate();
		}
	}
}