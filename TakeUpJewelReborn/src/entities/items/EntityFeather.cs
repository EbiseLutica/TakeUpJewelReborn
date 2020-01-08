using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.Misc;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Feather", 37)]
    public class EntityFeather : EntityLiving
    {
        public EntityFeather(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 16);
            SetGraphic(5);
            Velocity = new Vector(0, -0.9f);
            MainAi = new AiWalk(this, 1, 5, 5, 5, 5);
        }

        public override Texture2D[] ImageHandle => ResourceManager.Item;


        public override EntityGroup MyGroup => EntityGroup.Stage;

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }

        public override void OnUpdate()
        {
            if (IsOnLand)
                Velocity.Y = -Velocity.Y;
            foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
            {
                var ep = (EntityPlayer)entity;
                if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
                {
                    ep.SetMuteki();
                    IsDead = true;
                }
            }
            base.OnUpdate();
        }
    }
}