using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Modokee_Cave", 8)]
	public class EntityCaveModokee : EntityModokee
	{
		public EntityCaveModokee(Vector pnt, Tile[] objs, byte[,,] chips, EntityList par)
			: base(pnt, objs, chips, par)
		{
			MainAi = new AiFlySearch(this, 1, 0, 1, 4, 5);
		}

		public override Texture2D[] ImageHandle => ResourceManager.ModokeeCave;
	}
}