using DotFeather;

namespace TakeUpJewel
{
    [EntityRegistry("Star", -1)]
    public class EntityStar : EntityParticleBase
    {
        public EntityStar(Vector p, Tile[] mpts, byte[,,] mp, EntityList par)
            : base(p, mpts, mp, par)
        {
            Velocity.X = Core.GetRand(4) - 2;
            Velocity.Y = -Core.GetRand(5) - 5;
            SetGraphic(1);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}