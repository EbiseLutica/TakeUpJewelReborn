using System;

namespace TakeUpJewel
{
	/// <summary>
	/// EntityRegister によって自動で登録される Entity クラスを指定します。このクラスは継承できません。
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public sealed class EntityRegistryAttribute : Attribute
	{
		public int Id;
		public string Name;

		public EntityRegistryAttribute(string name, int id)
		{
			Name = name;
			Id = id;
		}
	}
}
