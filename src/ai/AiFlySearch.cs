using System;
using TakeUpJewel.Entities;

namespace TakeUpJewel.AI
{
    public class AiFlySearch : AiBase
    {
        private readonly int _leftAnimeEndIndex;

        private readonly int _leftAnimeStartIndex;
        private readonly int _rightAnimeEndIndex;
        private readonly int _rightAnimeStartIndex;

        private int _deg = 0;

        private int _speed = 1;

        private int _tick = 60;

        public AiFlySearch(EntityLiving baseentity, int spd, int lAnmStart, int lAnmEnd, int rAnmStart, int rAnmEnd)
        {
            HostEntity = baseentity;
            _speed = spd;
            _leftAnimeStartIndex = lAnmStart;
            _rightAnimeStartIndex = rAnmStart;
            _leftAnimeEndIndex = lAnmEnd;
            _rightAnimeEndIndex = rAnmEnd;
        }

        public override bool Use => !HostEntity.IsDying;

        public override void OnUpdate()
        {
            _tick++;

            if (_tick > 60)
            {
                var atan =
                    (float)
                    Math.Atan2(HostEntity.Parent.MainEntity.Location.Y - HostEntity.Location.Y,
                        HostEntity.Parent.MainEntity.Location.X - HostEntity.Location.X);
                HostEntity.Velocity.X = (float)Math.Cos(atan);
                HostEntity.Velocity.Y = (float)Math.Sin(atan);
                _tick = 0;
            }
            if (HostEntity.Direction == Direction.Left)
                HostEntity.SetAnime(_leftAnimeStartIndex, _leftAnimeEndIndex, 8);
            else
                HostEntity.SetAnime(_rightAnimeStartIndex, _rightAnimeEndIndex, 8);
        }
    }
}