using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Document", -1)]
	public class EntityDocument : EntityFlying
	{
		public EntityDocument(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			CollisionAIs.Add(new AiKillDefender(this));
			SetAnime(2, 4, 4);
		}

		public override Texture2D[] ImageHandle => ResourceManager.FolderFly;


		public override EntityGroup MyGroup => EntityGroup.MonsterWeapon;

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void Kill()
		{
			IsDead = true;
		}

		public override void CheckCollision()
		{
		}

		public override void OnUpdate()
		{

			if ((Location.X < -Size.Width) || (Location.Y < -Size.Height) || (Location.X > Core.I.CurrentMap.Size.X * 16) ||
				(Location.Y > Core.I.CurrentMap.Size.Y * 16))
				Kill();

			base.OnUpdate();
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			return this;
		}
	}
}