using System.Drawing;
using DotFeather;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.Misc;

namespace TakeUpJewel.Entities
{
    [EntityRegistry("FireWands", 33)]
    public class EntityPepper : EntityLiving
    {
        public EntityPepper(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 16);
            SetAnime(0, 3, 8);
            Velocity = new Vector(0, -3.0f);
        }

        public override Texture2D[] ImageHandle => ResourceManager.Item;


        public static bool Nikaime { get; set; }
        public override EntityGroup MyGroup => EntityGroup.Stage;

        public override void SetKilledAnime()
        {
        }

        public override void SetCrushedAnime()
        {
        }

        public override void OnUpdate()
        {
            if (!Nikaime)
            {
                EventRuntime.AddScript(GenerateItemDescription(@"ほのおのつえ\nなくならない　燃料が　詰められた　つえ。",
                    "ファイアボールを　撃てるようになる。　ファイアボールは　向いている　方向に　発射でき、　上下キーで　向きを　調節可能。"));
                Nikaime = true;
            }


            foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
            {
                var ep = (EntityPlayer)entity;
                if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
                {
                    ep.PowerUp(PlayerForm.Fire);
                    IsDead = true;
                }
            }
            base.OnUpdate();
        }
    }
}