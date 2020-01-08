using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Archer", 4)]
	public class EntityArcher : EntityLiving
	{
		public EntityArcher(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(32, 32);
			MainAi = new AiArch(this);
			CollisionAIs.Add(new AiKillDefender(this));
		}

		public override Texture2D[] ImageHandle => ResourceManager.Archer;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override void SetKilledAnime()
		{
			SetGraphic(Direction == Direction.Left ? 0 : 4);
		}

		public override void SetCrushedAnime()
		{
			SetGraphic(Direction == Direction.Left ? 0 : 4);
			IsCrushed = false;
		}
	}

	[EntityRegistry("Arrow", 67)]
	public class EntityArrow : EntityProjectile
	{
		public EntityArrow(Vector pnt, Object[] obj, byte[,,] chps, EntityList par)
		{
			Mpts = obj;
			Map = chps;
			Parent = par;
			Location = pnt;
			SetGraphic(0);
			Velocity.X = -2;
		}

		public override Texture2D[] ImageHandle => ResourceManager.Weapon;

		public override EntityGroup MyGroup => EntityGroup.MonsterWeapon;

		public override void OnStucked()
		{
			DESound.Play(Sounds.StuckArrow);
			base.OnStucked();
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			if (jsonobj.IsDefined("Speed"))
				Velocity.X = (float)jsonobj.Speed;
			return this;
		}

		public override void OnUpdate()
		{
			if (!IsStucked)
				foreach (EntityPlayer ep in Parent.FindEntitiesByType<EntityPlayer>())
				{
					if (ep.IsDying)
						continue;
					if (new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
						.CheckCollision(new Rectangle((int)Location.X, (int)Location.Y, Size.Width, Size.Height)))
						ep.Kill();
				}
			base.OnUpdate();
		}
	}
}