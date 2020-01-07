using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Turcos_Green", 9)]
    public class EntityTurcos : EntityLiving
    {
        public EntityTurcos(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;

            Size = new Size(24, 16);
            MainAi = new AiWalk(this, 1, 0, 1, 3, 4);
            CollisionAIs.Add(new AiKillDefender(this));
        }

        public override Texture2D[] ImageHandle => ResourceUtility.Turcos;


        public override EntityGroup MyGroup => EntityGroup.Enemy;

        public override RectangleF Collision => new RectangleF(2, 2, 20, 14);

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }

        public override void Kill()
        {
            if (IsDying)
                return;
            if (IsCrushed)
            {
                Parent.Add(new EntityTurcosShell(Location, Mpts, Map, Parent));
                IsDead = true;
            }
            else
            {
                IsDying = true;
            }
        }
    }
}