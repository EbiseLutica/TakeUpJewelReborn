using DotFeather;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
	[EntityRegistry("BrokenBlock", -1)]
	public class EntityBrokenBlock : EntityParticleBase
	{
		public EntityBrokenBlock(Vector p, Object[] mpts, byte[,,] mp, EntityList par)
			: base(p, mpts, mp, par)
		{
			Velocity.X = Core.GetRand(4) - 2;
			Velocity.Y = -Core.GetRand(5) - 5;
			SetGraphic(Misc.GetRandom(18, 19, 50, 51));
		}

		public override Texture2D[] ImageHandle => ResourceManager.MapChipMini;

		public override void OnUpdate()
		{
			Velocity.Y += 0.3f;
			base.OnUpdate();
		}
	}
}