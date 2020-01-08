using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.Misc;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("PoisonMushroom", 36)]
    public class EntityPoisonMushroom : EntityLiving
    {
        private const float Spdmax = 2;

        public EntityPoisonMushroom(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 16);
            MainAi = new AiWalk(this, -1, 4, 4, 4, 4);
        }

        public override Texture2D[] ImageHandle => ResourceManager.Item;

        public override EntityGroup MyGroup => EntityGroup.Stage;

        public override RectangleF Collision => new RectangleF(2, 2, 12, 14);

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }

        public override void OnUpdate()
        {
            foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
            {
                var ep = (EntityPlayer)entity;
                if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
                {
                    ep.Kill();
                    IsDead = true;
                }
            }
            if ((Parent.MainEntity.Location.X < Location.X) && (Velocity.X > -Spdmax))
                Velocity.X -= 0.2f;
            if ((Parent.MainEntity.Location.X > Location.X) && (Velocity.X < Spdmax))
                Velocity.X += 0.2f;

            base.OnUpdate();
        }
    }
}