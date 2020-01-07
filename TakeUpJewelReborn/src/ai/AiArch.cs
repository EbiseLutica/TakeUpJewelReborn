using Newtonsoft.Json.Linq;
using TakeUpJewel.Entities;
using TakeUpJewel.Util;

namespace TakeUpJewel.AI
{

    public class AiArch : AiBase
    {
        private int _nowstatus;

        private int _tick;

        public AiArch(EntityLiving host) : base(host) { }

        public override bool Use => !HostEntity.IsDying;

        public override void OnUpdate()
        {
            if (((_nowstatus != 2) && (_nowstatus != 0) && (_tick == 15)) || (_tick == 30))
            {
                if (_nowstatus == 2)
                {
                    DESound.Play(Sounds.ShootArrow);

                    // TODO: Arrow を実装したら、ここのコメントアウトを外す
                    var speed = HostEntity.Direction == Direction.Right ? Game.GetRand(4) + 1 : -Game.GetRand(4) - 1;
                    // HostEntity.Parent.Add(Game.I.EntityRegister.CreateEntity("Arrow",
                    //     new Vector(HostEntity.Location.X + (HostEntity.Direction == Direction.Left ? 0 : HostEntity.Size.Width),
                    //         HostEntity.Location.Y + HostEntity.Size.Height / 2), GameEngine.Mptobjects, Game.I.CurrentMap.Chips, HostEntity.Parent,
                    //     JObject.Parse("{\"Speed\": " + speed + " }")));
                }
                _tick = -1;
                _nowstatus = (_nowstatus + 1) % 4;
            }
            if (HostEntity.Parent.MainEntity.Location.X < HostEntity.Location.X)

                HostEntity.SetGraphic(_nowstatus + (HostEntity.Direction == Direction.Right ? 4 : 0));
            _tick++;
        }
    }
}