using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Smoke", -1)]
	public class EntitySmoke : EntityParticleBase
	{
		public EntitySmoke(Vector p, Tile[] mpts, byte[,,] mp, EntityList par)
			: base(p, mpts, mp, par)
		{
			SetAnime(8, 14, 9);
			LoopTimes = 0;
			Velocity = Vector.Up * 0.7f;
		}

		public override float Gravity => 0;

		public override Texture2D[] ImageHandle => ResourceManager.Particle;

		public override void OnUpdate()
		{
			if (Ptranime == 14)
				Kill();
			base.OnUpdate();
		}
	}
}
