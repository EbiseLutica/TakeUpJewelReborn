using System.Collections.Generic;
using System.Drawing;
using DotFeather;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using Object = TakeUpJewel.Data.Object;

namespace TakeUpJewel.Entities
{
    public class EntityIceWeapon : EntityLiving
    {
        public const int SpeedX = 4;

        private readonly int _defspeed = 0;
        public int Life = 100;

        public EntityIceWeapon(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 16);
            SetGraphic(14);
        }

        public override Texture2D[] ImageHandle => ResourceUtility.Weapon;


        public override EntityGroup MyGroup => EntityGroup.DefenderWeapon;

        public override Sounds KilledSound => Sounds.Null;

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }

        public override void OnUpdate()
        {
            //TODO: ここにこの Entity が行う処理を記述してください。
            UpdateBehavior();
            if ((int)Velocity.X == 0)
                Velocity.X = _defspeed * 4f;

            base.OnUpdate();
        }

        private void UpdateBehavior()
        {
            if (Location.X < 0)
                Kill();

            if (Location.X > Game.I.CurrentMap.Size.X * 16 - 16)
                Kill();

            if (Location.Y < 0)
            {
                Velocity.Y = 0;
                Location.Y = 0;
            }

            if (Location.Y > Game.I.CurrentMap.Size.Y * 16)
                Kill(true, false);

            foreach (EntityLiving e in new List<Entity>(Parent.FindEntitiesByType<EntityLiving>()))
                if (!e.IsDying && (e.MyGroup == EntityGroup.Enemy) &&
                    new RectangleF(Location.ToPoint(), Size).CheckCollision(new RectangleF(e.Location.ToPoint(), e.Size)))
                    e.Kill();
            if (Life < 1)
                Kill();

            if (Life == 70)
            {
                SetGraphic(15);
                Velocity.X *= 0.5f;
            }
            if (Life == 35)
            {
                SetGraphic(16);
                Velocity.X *= 0.5f;
            }

            if ((CollisionLeft() == ObjectHitFlag.Hit) || (CollisionRight() == ObjectHitFlag.Hit))
                Velocity.X *= -1;
            Life--;
        }

        public override void Dying()
        {
            IsDead = true;
        }

        public override Entity SetEntityData(dynamic jsonobj)
        {
            base.SetEntityData((object)jsonobj);
            if (jsonobj.IsDefined("SpeedX"))
                Velocity.X = (float)jsonobj.SpeedX;
            return this;
        }
    }
}