using DotFeather;
using TakeUpJewel.Data;

namespace TakeUpJewel.Entities
{
    public class EntityTurcosShellRed : EntityTurcosShell
    {
        public EntityTurcosShellRed(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
            : base(pnt, obj, chips, par)
        {
            SetGraphic(1);
        }
    }
}