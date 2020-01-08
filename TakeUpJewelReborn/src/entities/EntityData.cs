using System;

namespace TakeUpJewel
{
	/// <summary>
	/// エンティティの情報を表します。
	/// </summary>
	public class EntityData
	{
		/// <summary>
		/// この Entity の ID。
		/// </summary>
		public int EntityId;

		/// <summary>
		/// この Entity の一般名。
		/// </summary>
		public string EntityName = "";

		/// <summary>
		/// この Entity を表す Type。
		/// </summary>
		public Type EntityType = typeof(Entity);
	}
}
