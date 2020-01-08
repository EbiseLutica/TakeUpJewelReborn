﻿using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Bunyo", 1)]
    public class EntityBunyo : EntityLiving
    {
        public EntityBunyo(Vector pnt, Object[] obj, byte[,,] chps, EntityList par)
        {
            Mpts = obj;
            Map = chps;
            Parent = par;
            Location = pnt;
            MainAi = new AiWalk(this, 1, 0, 1, 4, 5);
            CollisionAIs.Add(new AiKillDefender(this));
            Size = new Size(16, 16);
        }

        public override Texture2D[] ImageHandle => ResourceManager.CommonMob;


        public override EntityGroup MyGroup => EntityGroup.Enemy;

        public override RectangleF Collision => new RectangleF(2, 2, 12, 14);

        public override void Move()
        {
            base.Move();
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
        }

        public override void SetKilledAnime()
        {
            SetGraphic(2);
        }

        public override void SetCrushedAnime()
        {
            SetGraphic(3);
        }
    }
}