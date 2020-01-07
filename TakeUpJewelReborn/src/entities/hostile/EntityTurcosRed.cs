using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Turcos_Red", 10)]
    public class EntityTurcosRed : EntityTurcos
    {
        public EntityTurcosRed(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
            : base(pnt, obj, chips, par)
        {
            MainAi = new AiWalk(this, 1, 6, 7, 9, 10);
        }

        public override void Kill()
        {
            Parent.Add(new EntityTurcosShellRed(Location, Mpts, Map, Parent));
            IsDead = true;
        }
    }
}