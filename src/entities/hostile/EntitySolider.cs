using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("Solider", 6)]
    public class EntitySolider : EntityLiving
    {
        public EntitySolider(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 32);
            CollisionAIs.Add(new AiKillDefender(this));
            SetAnime(0, 3, 8);
        }

        public override Texture2D[] ImageHandle => ResourceUtility.Dwarf;


        public override EntityGroup MyGroup => EntityGroup.Enemy;

        public override void SetKilledAnime()
        {
            SetGraphic(4);
        }

        public override void SetCrushedAnime()
        {
            SetGraphic(0);
            IsCrushed = false;
        }
    }
}