using System.Drawing;
using DotFeather;
using TakeUpJewel.Data;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("InfinitySpawner", 5)]
    public class EntityInfinitySpawner : Entity
    {
        private dynamic? _obj;

        private int _tick;

        public EntityInfinitySpawner(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
        }


        public override EntityGroup MyGroup => EntityGroup.Stage;

        public override void OnUpdate()
        {
            if (Parent.MainEntity != null && ((Parent.MainEntity.Location.X + 8 < Location.X - 32) ||
                 (Location.X + 48 < Parent.MainEntity.Location.X + 8)) &&
                (_tick > 120))
            {
                if (_obj!.IsDefined("Tag"))
                    _obj!.EntityData.Tag = _obj.Tag;
                int spid = (int)_obj!.EntityID, posx = (int)_obj.PosX, posy = (int)_obj.PosY;
                if (Core.I.EntityRegister.GetDataById(spid) != null)
                    Parent.Add(
                        Core.I.EntityRegister.CreateEntity(spid, new Vector(posx, posy), Mpts, Map, Parent, _obj.EntityData),
                        spid == 0);
                _tick = 0;
            }
            _tick++;
            base.OnUpdate();
        }

        public override Entity SetEntityData(dynamic jsonobj)
        {
            base.SetEntityData((object)jsonobj);
            if (jsonobj.IsDefined("TargetEntity"))
            {
                _obj = jsonobj.TargetEntity;
                _obj.PosX = Location.X;
                _obj.PosY = Location.Y;
            }
            return this;
        }
    }
}