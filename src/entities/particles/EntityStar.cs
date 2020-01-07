using DotFeather;
using TakeUpJewel.Data;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Star", -1)]
    public class EntityStar : EntityParticleBase
    {
        public EntityStar(Vector p, Object[] mpts, byte[,,] mp, EntityList par)
            : base(p, mpts, mp, par)
        {
            Velocity.X = Game.GetRand(4) - 2;
            Velocity.Y = -Game.GetRand(5) - 5;
            SetGraphic(1);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }
    }
}