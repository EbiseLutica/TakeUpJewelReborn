﻿using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Turbo", 17)]
    public class EntityTurbo : EntityLiving
    {
        public EntityTurbo(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 16);
            CollisionAIs.Add(new AiKillDefender(this));
            SetAnime(0, 1, 8);
        }

        public override Texture2D[] ImageHandle => ResourceUtility.Turbo;


        public override EntityGroup MyGroup => EntityGroup.Enemy;

        public override void SetKilledAnime()
        {
            SetGraphic(2);
        }

        public override void SetCrushedAnime()
        {
            SetGraphic(2);
            IsCrushed = false;
        }

        public override void OnUpdate()
        {
            //TODO: ここにこの Entity が行う処理を記述してください。
            base.OnUpdate();
            Velocity.X = -2f;
            if (IsDying)
                Velocity = Vector.Zero;
        }

        public override Entity SetEntityData(dynamic jsonobj)
        {
            base.SetEntityData((object)jsonobj);
            return this;
        }
    }

    [EntityRegistry("RollingRock", 15)]
    public class EntityRollingRock : EntityTurbo
    {
        public EntityRollingRock(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
            : base(pnt, obj, chips, par)
        {
            Size = new Size(32, 32);
        }

        public override Texture2D[] ImageHandle => ResourceUtility.RollingRock;

        public override EntityGroup MyGroup => EntityGroup.Enemy;

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
            IsCrushed = false;
        }
    }
}