using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Densy", 88)]
	public class EntityDensy : EntityFlying
	{
		public EntityDensy(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			MainAi = new AiFlySearch(this, 2, 0, 3, 0, 3);
			CollisionAIs.Add(new AiKillDefender(this));
		}

		public override Texture2D[] ImageHandle => ResourceManager.Densy;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override void SetKilledAnime()
		{
			AnimeSpeed = 0;
		}

		public override void SetCrushedAnime()
		{
			IsCrushed = false;
			AnimeSpeed = 0;
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			return this;
		}
	}
}