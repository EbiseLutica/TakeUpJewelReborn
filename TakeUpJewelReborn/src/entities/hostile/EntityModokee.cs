﻿using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Modokee_Ground", 2)]
	public class EntityModokee : EntityFlying
	{
		public EntityModokee(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(32, 16);
			MainAi = new AiFlySine(this, 1, 0, 1, 4, 5);
			CollisionAIs.Add(new AiKillDefender(this));
		}

		public override Texture2D[] ImageHandle => ResourceManager.ModokeeGround;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override void SetKilledAnime()
		{
			if (Direction == Direction.Left)
				SetGraphic(2);
			else
				SetGraphic(6);
		}

		public override void SetCrushedAnime()
		{
			if (Direction == Direction.Left)
				SetGraphic(3);
			else
				SetGraphic(7);
		}

		public override void OnDyingAnimation(IDrawable d)
		{
			if (!IsCrushed)
				base.OnDyingAnimation(d);
		}
	}
}