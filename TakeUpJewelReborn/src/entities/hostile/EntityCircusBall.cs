using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.DevelopmentUtility;
using Object = TakeUpJewel.Data.Object;

namespace TakeUpJewel.Entities
{
    public class EntityCircusBall : EntityLiving
    {
        public int Life = 3;

        public EntityCircusBall(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Velocity.Y = -3.0f;
            Size = new Size(16, 16);
            MainAi = new AiKillDefender(this);
            SetGraphic(GetRandom(17, 18, 19));
        }

        public override EntityGroup MyGroup => EntityGroup.Enemy;
        public override Texture2D[] ImageHandle => ResourceUtility.Weapon;

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }

        public override Entity SetEntityData(dynamic jsonobj)
        {
            base.SetEntityData((object)jsonobj);
            if (jsonobj.IsDefined("SpeedX"))
                Velocity.X = (float)jsonobj.SpeedX;
            return this;
        }

        public override void OnUpdate()
        {
            if (CollisionBottom() == ObjectHitFlag.Hit)
            {
                Life--;
                Location.Y += Velocity.Y = -4.3f;
                DESound.Play(Sounds.Dumping);
            }

            if ((CollisionLeft() == ObjectHitFlag.Hit) || (CollisionRight() == ObjectHitFlag.Hit))
            {
                Life--;
                Location.X += Velocity.X *= -1;
                DESound.Play(Sounds.Dumping);
            }

            if (Life < 0)
                Kill();
            base.OnUpdate();
        }

        public override void Kill()
        {
            DESound.Play(Sounds.BalloonBroken);
            IsDead = true;
        }
    }
}