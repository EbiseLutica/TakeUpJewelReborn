using System.Collections.Generic;
using System.Drawing;
using TakeUpJewel.Entities;
using TakeUpJewel.Util;

namespace TakeUpJewel.AI
{
    public class AiKiller : AiBase
    {
        public AiKiller(EntityLiving host) : base(host) { }

        public override bool Use => !HostEntity.IsDying;

        public override void OnUpdate()
        {
            foreach (EntityLiving ep in new List<Entity>(HostEntity.Parent.FindEntitiesByType<EntityLiving>()))
            {
                if ((ep == HostEntity) || ep.IsDying || ((ep.MyGroup != EntityGroup.Friend) && (ep.MyGroup != EntityGroup.Enemy)))
                    continue;
                if (HostEntity is EntityTurcosShell && (((EntityTurcosShell)HostEntity).Mutekitime > 0) &&
                    (ep.MyGroup == EntityGroup.Friend))
                    continue;
                if (new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
                    .CheckCollision(new Rectangle((int)HostEntity.Location.X, (int)HostEntity.Location.Y + 8, HostEntity.Size.Width,
                        HostEntity.Size.Height - 8)))
                {
                    if (ep.IsDying)
                        continue;
                    ep.Kill();
                }
            }
        }
    }
}