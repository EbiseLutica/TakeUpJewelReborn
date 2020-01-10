using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Bunyo", 1)]
	public class EntityBunyo : EntityLiving
	{
		public EntityBunyo(Vector pnt, Tile[] obj, byte[,,] chps, EntityList par)
		{
			Mpts = obj;
			Map = chps;
			Parent = par;
			Location = pnt;
			MainAi = new AiWalk(this, 1, 0, 1, 4, 5);
			CollisionAIs.Add(new AiKillDefender(this));
			Size = new Size(16, 16);
		}

		public override Texture2D[] ImageHandle => ResourceManager.CommonMob;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override RectangleF Collision => new RectangleF(2, 2, 12, 14);

		public override void Move()
		{
			base.Move();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		public override void SetKilledAnime()
		{
			SetGraphic(2);
		}

		public override void SetCrushedAnime()
		{
			SetGraphic(3);
		}

		public override void OnDyingAnimation(IDrawable d)
		{
			if (!IsCrushed)
				base.OnDyingAnimation(d);
		}
	}
}