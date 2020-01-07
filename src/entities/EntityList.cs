using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotFeather;

namespace TakeUpJewel.Entities
{
    /// <summary>
    /// エンティティのコレクションを表します。
    /// </summary>
    public class EntityList : EntityCollection
    {
        public Entity MainEntity { get; private set; }

        /// <summary>
        /// タグを使って Entity を探します。
        /// </summary>
        /// <param name="tag">タグ文字列。</param>
        /// <returns>Entity が存在すればそれらのコレクション、なければ空のコレクションが返ります。</returns>
        public IEnumerable<Entity> FindEntitiesByTag(string tag)
        {
            return this.Where(sp => tag == sp.Tag);
        }

        /// <summary>
        /// 型を指定して Entity を探します。
        /// </summary>
        /// <typeparam name="T">探す対象のデータ型。</typeparam>
        /// <returns></returns>
        public IEnumerable<T> FindEntitiesByType<T>()
        {
            return this.OfType<T>();
        }

        public override void Add(Entity item)
        {
            Add(item, false);
        }

        public void Add(Entity item, bool isMain)
        {
            Items.Add(item);
            if (item is EntityLiving living)
            {
                // AI を初期化する
                living.MainAi?.OnInit();
                foreach (var ai in living.CollisionAIs)
                    ai.OnInit();
            }

            if (item is EntityVisible visible)
            {
                // IDrawable のマッピングを行う
                drawablesMap[visible] = visible.OnSpawn();
            }

            if (isMain)
                MainEntity = item;
        }

        public void Update(ref byte[,,] chips)
        {
            //foreach だと削除・追加時にしぬので、forでやる
            for (var i = 0; i < Count; i++)
            {
                var item = this[i];
                if (item.IsDead) //死んだら消す
                    Remove(item);
                if (i >= Count)
                    break;
                item = this[i];
                if (Math.Abs(MainEntity.Location.X - item.Location.X) > Const.Width)
                    continue;
                if (!Game.I.IsFreezing)
                {
                    item.OnUpdate(); //更新処理をする
                    var living = item as EntityLiving;
                    if (living != null)
                    {
                        if ((living.MainAi != null) && !living.MainAi.IsInitialized)
                            living.MainAi.OnInit();
                        if ((living.MainAi != null) && living.MainAi.Use)
                            living.MainAi.OnUpdate();

                        foreach (var ai in living.CollisionAIs)
                        {
                            if (!ai.IsInitialized)
                                ai.OnInit();
                            if (ai.Use)
                                ai.OnUpdate();
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            foreach (var item in FindEntitiesByType<EntityVisible>().OrderBy(i => i.ZIndex))
            {
                if (Math.Abs(MainEntity.Location.X - item.Location.X) > Const.Width)
                    continue;
                item.OnUpdate(Game.I.Camera + item.Location, drawablesMap[item]);
            }
        }

        private Dictionary<EntityVisible, IDrawable> drawablesMap = new Dictionary<EntityVisible, IDrawable>();
    }
}