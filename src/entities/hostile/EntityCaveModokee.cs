﻿using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Modokee_Cave", 8)]
    public class EntityCaveModokee : EntityModokee
    {
        public EntityCaveModokee(Vector pnt, Object[] objs, byte[,,] chips, EntityList par)
            : base(pnt, objs, chips, par)
        {
            MainAi = new AiFlySearch(this, 1, 0, 1, 4, 5);
        }

        public override Texture2D[] ImageHandle => ResourceUtility.ModokeeCave;
    }
}