using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
    [EntityRegistry("Bubble", -1)]
    public class EntityBubble : EntityParticleBase
    {
        public EntityBubble(Vector p, Object[] mpts, byte[,,] mp, EntityList par)
            : base(p, mpts, mp, par)
        {
            Velocity.Y = -1;
            SetGraphic(2);
        }

        public override void OnUpdate()
        {
            var judge = new Vector(Location.X + Size.Width / 2, Location.Y + Size.Height / 2);
            if (Mpts[Map[(int)judge.X / 16, (int)judge.Y / 16, 0]].CheckHit((int)judge.X % 16, (int)judge.Y % 16) !=
                ObjectHitFlag.InWater)
                Kill();
            base.OnUpdate();
        }
    }
}