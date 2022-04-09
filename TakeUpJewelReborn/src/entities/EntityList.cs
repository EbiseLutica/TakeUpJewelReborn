using System;
using System.Collections.Generic;
using System.Linq;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// エンティティのコレクションを表します。
	/// </summary>
	public class EntityList : EntityCollection
	{
		public Entity? MainEntity { get; private set; }

		public IEnumerable<ElementBase> Drawables => drawablesMap.Values;

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

		public ElementBase? GetDrawableByEntity(EntityVisible entity) => drawablesMap.TryGetValue(entity, out var d) ? d : null;

		public override void Add(Entity item)
		{
			Add(item, false);
		}

		public override bool Remove(Entity item)
		{
			EntityRemoved?.Invoke(this, item);
			if (item is EntityVisible visible)
				drawablesMap.Remove(visible);
			return base.Remove(item);
		}

		public override void Clear()
		{
			base.Clear();
			drawablesMap.Clear();
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
				visible.OnUpdate(visible.Location, drawablesMap[visible]);
			}

			if (isMain)
				MainEntity = item;

			EntityAdded?.Invoke(this, item);
		}

		public void Update()
		{
			//foreach だと削除・追加時にしぬので、forでやる
			for (var i = 0; i < Count; i++)
			{
				var item = this[i];
				if (item.IsDead) //死んだら消す
				{
					Remove(item);
				}
				if (i >= Count)
					break;
				item = this[i];

				var living = item as EntityLiving;
				if (MainEntity != null && Math.Abs(MainEntity.Location.X - item.Location.X) > Const.Width && (living == null || !living.IsDying))
				{
					if (item.MyGroup == EntityGroup.DefenderWeapon || item.MyGroup == EntityGroup.MonsterWeapon)
					{
						item.Kill();
						Remove(item);
					}
					continue;
				}

				if (!Core.I.IsFreezing)
				{
					item.OnUpdate(); //更新処理をする
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
				if (MainEntity != null && Math.Abs(MainEntity.Location.X - item.Location.X) > Const.Width)
					continue;
				item.OnUpdate(item.Location, drawablesMap[item]);
			}
		}

		public event EventHandler<Entity>? EntityAdded;

		public event EventHandler<Entity>? EntityRemoved;

		private Dictionary<EntityVisible, ElementBase> drawablesMap = new Dictionary<EntityVisible, ElementBase>();
	}
}
