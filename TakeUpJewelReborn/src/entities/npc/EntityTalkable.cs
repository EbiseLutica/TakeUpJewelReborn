using System.Drawing;
using DotFeather;
using TakeUpJewel.Data;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.ResourceUtility;

namespace TakeUpJewel.Entities
{
    /// <summary>
    /// プレイヤーが話しかけられる Entity です。
    /// </summary>
    [EntityRegistry(nameof(EntityTalkable), 89)]
    public class EntityTalkable : EntityNpc
    {
        private bool _canExecuteScript;
        private string _myScript;

        public EntityTalkable(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
            : base(pnt, obj, chips, par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
        }

        public override EntityGroup MyGroup => EntityGroup.System;

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
            foreach (EntityPlayer ep in Parent.FindEntitiesByType<EntityPlayer>())
            {
                if (ep.IsDying)
                    continue;


                // プレイヤーと自分の当たり判定があり、上キーが押されたとき、スクリプト実行
                if ((_canExecuteScript = new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
                        .CheckCollision(new Rectangle((int)Location.X, (int)Location.Y, Size.Width,
                            Size.Height))) && DFKeyboard.Up)
                    try
                    {
                        EventRuntime.AddScript(new EventScript(_myScript));
                    }
                    catch (EventScript.EventScriptException ex)
                    {
                        EventRuntime.AddScript(new EventScript($@"[enstop]
[mesbox:down]
[mes:""エラー！\n{ex.Message.Replace(@"\", @"\\").Replace(@"""", @"\""")}""]
[mesend]
[enstart]"));
                    }
            }
            base.OnUpdate();
        }

        public override IDrawable OnSpawn()
        {
            return base.OnSpawn();
        }

        /// <summary>
        /// Entity 生成時にメタデータが渡されると、このメソッドが呼ばれます。
        /// </summary>
        /// <param name="jsonobj"></param>
        /// <returns></returns>
        public override Entity SetEntityData(dynamic jsonobj)
        {
            //TODO: メタデータに埋め込まれたスクリプトを取得する
            if (jsonobj.Script())
                _myScript = jsonobj.Script;
            base.SetEntityData((object)jsonobj);
            return this;
        }
    }

    [EntityRegistry(nameof(EntityNpc), 90)]
    public class EntityNpc : EntityLiving
    {
        public EntityNpc(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
        {
            Location = pnt;
            Mpts = obj;
            Map = chips;
            Parent = par;
        }


        public override Texture2D[] ImageHandle => textures;


        public override EntityGroup MyGroup => EntityGroup.System;

        public override RectangleF Collision => new RectangleF(default(PointF), Size);

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
        /// EntityNpc は死にません。
        /// </summary>
        public override void Kill()
        {
        }

        /// <summary>
        /// EntityNpc は死にません。
        /// </summary>
        public override void Kill(bool isfall, bool iscrushed)
        {
        }


        /// <summary>
        /// Entity 生成時にメタデータが渡されると、このメソッドが呼ばれます。
        /// </summary>
        /// <param name="jsonobj"></param>
        /// <returns></returns>
        public override Entity SetEntityData(dynamic jsonobj)
        {
            string textureFile = jsonobj.TextureFile;
            var width = (int)jsonobj.Width;
            var height = (int)jsonobj.Height;
            var startIndex = (int)jsonobj.StartIndex;
            var endIndex = (int)jsonobj.EndIndex;
            var speed = (int)jsonobj.Speed;
            textures = LoadTextures(textureFile, width, height);

            if (jsonobj.UseAnime)
                SetAnime(startIndex, endIndex, speed);
            else
                SetGraphic(startIndex);

            Size = new Size(width, height);

            base.SetEntityData((object)jsonobj);
            return this;
        }
        private Texture2D[] textures;
    }
}