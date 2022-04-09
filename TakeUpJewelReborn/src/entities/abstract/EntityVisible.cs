using DotFeather;

namespace TakeUpJewel
{
	public abstract class EntityVisible : Entity
	{
		public abstract ElementBase OnSpawn();

		public abstract void OnUpdate(Vector p, ElementBase el);
	}
}
