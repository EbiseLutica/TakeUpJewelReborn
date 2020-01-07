using System.Drawing;
using DotFeather;
using Object = TakeUpJewel.Data.Object;

namespace TakeUpJewel.Entities
{
    public abstract class Entity
    {
        /// <summary>
        /// 前フレームでの場所。
        /// </summary>
        public Vector PrevLocation;

        /// <summary>
        /// 前フレームでの速度。
        /// </summary>
        public Vector PrevVelocity;

        public Direction Direction;

        /// <summary>
        /// 踏み潰されたかどうか。
        /// </summary>
        public bool IsCrushed;

        /// <summary>
        /// マップ上から削除されるフラグ。
        /// </summary>
        public bool IsDead;

        /// <summary>
        /// 落下によって死んだかどうか。
        /// </summary>
        public bool IsFall;

        /// <summary>
        /// 地面についているかどうか。
        /// </summary>
        public bool IsOnLand;

        /// <summary>
        /// 自分の座標。
        /// </summary>
        public Vector Location;

        /// <summary>
        /// 現在のマップデータ。
        /// </summary>
        public byte[,,] Map;

        /// <summary>
        /// 現在のマップチップ。
        /// </summary>
        public Object[] Mpts;

        /// <summary>
        /// 自分の親である EntityList。
        /// </summary>
        public EntityList Parent;

        /// <summary>
        /// 自分につけられたタグ。
        /// </summary>
        public string Tag;

        /// <summary>
        /// 自分の速度。
        /// </summary>
        public Vector Velocity;

        /// <summary>
        /// 描画優先順位。
        /// </summary>
        public int ZIndex;

        /// <summary>
        /// 自分の所属しているグループを取得します。
        /// </summary>
        public abstract EntityGroup MyGroup { get; }

        /// <summary>
        /// 自分の大きさ。
        /// </summary>
        public Size Size { get; protected set; }

        /// <summary>
        /// この Entity に、エンティティデータを設定します。
        /// </summary>
        /// <param name="jsonobj"></param>
        /// <returns></returns>
        public virtual Entity SetEntityData(dynamic jsonobj)
        {
            if (jsonobj == null)
                return this;
            if (jsonobj.ZIndex())
                ZIndex = (int)jsonobj.ZIndex;
            if (jsonobj.IsDefined("Tag"))
                Tag = jsonobj.Tag;
            return this;
        }

        public virtual void OnReload()
        {
        }

        /// <summary>
        /// この Entity を殺します。
        /// </summary>
        public virtual void Kill()
        {
            IsDead = true;
        }

        /// <summary>
        /// オプションを指定して、この Entity を殺します。
        /// </summary>
        /// <param name="isfall">落下によって死んだかどうか。</param>
        /// <param name="iscrushed">踏み潰されたかどうか。</param>
        public virtual void Kill(bool isfall, bool iscrushed)
        {
            IsFall = isfall;
            IsCrushed = iscrushed;
            Kill();
        }

        /// <summary>
        /// Entity の場所を更新します。
        /// </summary>
        public virtual void Move()
        {
            Location += Velocity;
        }

        /// <summary>
        /// フレーム毎に呼ばれ、Entity のアップデート処理をします。
        /// </summary>
        public virtual void OnUpdate()
        {
            Move();

            Backup();
        }

        /// <summary>
        /// 変数をバックアップし、次フレームに持ち越す処理をします。
        /// </summary>
        public virtual void Backup()
        {
            PrevLocation = Location;
            PrevVelocity = Velocity;
        }
    }
}