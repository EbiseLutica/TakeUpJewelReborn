using System;
using TakeUpJewel.Data;
using TakeUpJewel.Entities;

namespace TakeUpJewel.AI
{
    public class AiFlySine : AiBase
    {
        private readonly int _leftAnimeEndIndex;

        private readonly int _leftAnimeStartIndex;
        private readonly int _rightAnimeEndIndex;
        private readonly int _rightAnimeStartIndex;

        private readonly int _speed = 1;

        private int _deg;

        public AiFlySine(EntityLiving host, int spd, int lAnmStart, int lAnmEnd, int rAnmStart, int rAnmEnd)
            : base(host)
        {
            _speed = spd;
            _leftAnimeStartIndex = lAnmStart;
            _rightAnimeStartIndex = rAnmStart;
            _leftAnimeEndIndex = lAnmEnd;
            _rightAnimeEndIndex = rAnmEnd;
        }

        public override bool Use => !HostEntity.IsDying;

        public override void OnUpdate()
        {
            if ((HostEntity.CollisionLeft() == ObjectHitFlag.Hit) || (HostEntity.Location.X <= 0))
            {
                HostEntity.Velocity.X = _speed;
                HostEntity.SetAnime(_rightAnimeStartIndex, _rightAnimeEndIndex, 8);
            }
            if ((HostEntity.CollisionRight() == ObjectHitFlag.Hit) || (HostEntity.Location.X >= Core.I.CurrentMap.Size.X * 16 - 1))
            {
                HostEntity.Velocity.X = -_speed;
                HostEntity.SetAnime(_leftAnimeStartIndex, _leftAnimeEndIndex, 8);
            }
            HostEntity.Velocity.Y = (float)Math.Sin(_deg / 180.0 * Math.PI) * (HostEntity.Direction == Direction.Left ? -1 : 1);
            _deg = (_deg + 5) % 360;
        }

        public override void OnInit()
        {
            if (HostEntity.Parent.MainEntity.Location.X < HostEntity.Location.X)
            {
                HostEntity.Velocity.X = -_speed;
                HostEntity.SetAnime(_leftAnimeStartIndex, _leftAnimeEndIndex, 8);
            }
            else
            {
                HostEntity.Velocity.X = _speed;
                HostEntity.SetAnime(_rightAnimeStartIndex, _rightAnimeEndIndex, 8);
            }
            base.OnInit();
        }
    }
}