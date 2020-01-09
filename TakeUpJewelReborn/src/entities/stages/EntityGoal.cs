using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Goal", 18)]
	public class EntityGoal : Entity
	{
		private int _next = 1;
		private dynamic? _obj = null;

		public EntityGoal(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
		}


		public override EntityGroup MyGroup => EntityGroup.Stage;

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			_next = (int)jsonobj.NextStage;
			return this;
		}

		public override void OnUpdate()
		{
			if (Parent.MainEntity != null && Parent.MainEntity.Location.X > Location.X + 8)
			{
				Core.I.NextLevel = _next;
				Core.I.IsGoal = true;
			}
			base.OnUpdate();
		}
	}
}