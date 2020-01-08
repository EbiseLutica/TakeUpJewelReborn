using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
    //[EntityRegistry("", 127)]
    public class EntityTemplate : EntityLiving
    {
        public EntityTemplate(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
            Size = new Size(16, 16);
        }

        public override Texture2D[] ImageHandle => null;


        public override EntityGroup MyGroup => EntityGroup.Other;

        /// <summary>
        /// 死んでいるアニメーションを設定します。
        /// </summary>
        public override void SetKilledAnime()
        {
        }

        /// <summary>
        /// 踏みつけられたアニメーションを設定します。
        /// </summary>
        public override void SetCrushedAnime()
        {
        }

        /// <summary>
        /// Tick 毎に呼ばれる Entity の処理イベントです。
        /// </summary>
        /// <param name="ks"></param>
        public override void OnUpdate()
        {
            //TODO: ここにこの Entity が行う処理を記述してください。
            base.OnUpdate();
        }

        /// <summary>
        /// Entity 生成時にメタデータが渡されると、このメソッドが呼ばれます。
        /// </summary>
        /// <param name="jsonobj"></param>
        /// <returns></returns>
        public override Entity SetEntityData(dynamic jsonobj)
        {
            base.SetEntityData((object)jsonobj);
            return this;
        }
    }
}