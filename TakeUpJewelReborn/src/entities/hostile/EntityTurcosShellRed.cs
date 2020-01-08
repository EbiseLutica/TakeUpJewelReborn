using DotFeather;

namespace TakeUpJewel
{
    public class EntityTurcosShellRed : EntityTurcosShell
    {
        public EntityTurcosShellRed(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
            : base(pnt, obj, chips, par)
        {
            SetGraphic(1);
        }
    }
}