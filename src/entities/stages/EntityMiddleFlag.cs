using DotFeather;
using TakeUpJewel.Data;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("MiddleFlag", 83)]
    public class EntityMiddleFlag : Entity
    {
        private bool _flagged;
        private dynamic _obj = null;

        public EntityMiddleFlag(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
        }


        public override EntityGroup MyGroup => EntityGroup.Stage;

        public override void OnUpdate()
        {
            if ((Parent.MainEntity.Location.X > Location.X + 8) && !_flagged)
            {
                Game.I.Middle = new Vector(Location.X + 8, Location.Y + 8);
                _flagged = true;
            }
            base.OnUpdate();
        }
    }
}