using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// Entity の情報はここに保存され、ゲーム内で使用されます。
	/// </summary>
	public class EntityRegistry : ICollection<EntityData>
	{
		public EntityRegistry()
		{
			logger = new Logger(nameof(EntityRegistry));

			Items = new List<EntityData>();
			foreach (var t in Assembly.GetExecutingAssembly().GetExportedTypes())
			{
				var attr = t.GetCustomAttributes(typeof(EntityRegistryAttribute), false);
				if (attr.Length > 0)
				{
					var entity = (EntityRegistryAttribute)attr[0];
					Add(t, entity.Name, entity.Id);
					logger.Info($"Registered {t.Name}#{entity.Id} as '{entity.Name}'");
				}
			}
		}

		public List<EntityData> Items { get; protected set; }

		public EntityData? this[int i] => GetDataById(i);

		public void Add(EntityData item)
		{
			if ((item.EntityId != -1) && (GetDataById(item.EntityId) != null))
				throw new InvalidOperationException("既に同じ ID のエンティティが登録されています。");

			if (Contains(item) == false)
				Items.Add(item);
		}


		public bool IsReadOnly => false;

		public void Clear()
		{
			Items.Clear();
		}

		public int Count => Items.Count;

		public bool Contains(EntityData item)
		{
			return Items.Contains(item);
		}

		public void CopyTo(EntityData[] array, int arrayIndex)
		{
			Items.CopyTo(array, arrayIndex);
		}

		public bool Remove(EntityData item)
		{
			return Items.Remove(item);
		}

		public IEnumerator<EntityData> GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return Items.GetEnumerator();
		}

		public Entity CreateEntity(string name, Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			var data = GetDataByName(name);
			if (data == null)
				throw new InvalidOperationException(name + " というエンティティは存在しません");
			var o = Activator.CreateInstance(data.EntityType, pnt, obj, chips, par);

			if (!((o != null) && o is Entity))
				throw new InvalidOperationException("Entity が存在しません。");

			return (Entity)o;
		}

		public Entity CreateEntity(string name, Vector pnt, Object[] obj, byte[,,] chips, EntityList par, dynamic jsonobj)
		{
			var e = CreateEntity(name, pnt, obj, chips, par);
			if (jsonobj == null)
				return e;
			e.SetEntityData(jsonobj);

			return e;
		}

		public Entity CreateEntity(int id, Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			var data = this[id];
			if (data == null)
				throw new InvalidOperationException("" + id + " 番のエンティティは存在しません");
			var o = Activator.CreateInstance(data.EntityType, pnt, obj, chips, par);

			if (!((o != null) && o is Entity))
				throw new InvalidOperationException("Entity が存在しません。");

			return (Entity)o;
		}

		public Entity CreateEntity(int id, Vector pnt, Object[] obj, byte[,,] chips, EntityList par, dynamic jsonobj)
		{
			var e = CreateEntity(id, pnt, obj, chips, par);
			if (jsonobj == null)
				return e;
			e.SetEntityData(jsonobj);

			return e;
		}

		public void Add(Type entitytype, string name, int id)
		{
			if ((id != -1) && (GetDataById(id) != null))
				throw new InvalidOperationException("既に同じ ID のエンティティが登録されています。");
			var item = new EntityData
			{
				EntityId = id,
				EntityType = entitytype,
				EntityName = name
			};

			if (Contains(item) == false)
				Items.Add(item);
		}

		public EntityData? GetDataById(int id)
		{
			foreach (var d in this.Where(et => et.EntityId == id))
				return d;
			return null;
		}

		public EntityData? GetDataByName(string name)
		{
			foreach (var d in this.Where(et => et.EntityName == name))
				return d;
			return null;
		}

		public Logger logger;
	}
}