using DotFeather;

namespace TakeUpJewel
{
    public class EntityExplosion : EntityFlying
    {
        public override EntityGroup MyGroup { get; }
        public override Texture2D[] ImageHandle { get; }

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }
    }
}