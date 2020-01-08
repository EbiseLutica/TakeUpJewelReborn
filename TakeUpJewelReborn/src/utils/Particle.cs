using System.Drawing;
using static TakeUpJewel.Core;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// パーティクルの処理をまとめた静的クラスです。
	/// </summary>
	public static class Particle
	{
		public static int Tick;

		/// <summary>
		/// 泡を出します。
		/// </summary>
		/// <param name="origin">泡を出すエンティティ。</param>
		public static void Bubble(Entity origin)
		{
			if (Tick % 65 == 0)
				origin.Parent.Add(Core.I.EntityRegistry.CreateEntity("Bubble",
					new Vector((int)origin.Location.X + origin.Size.Width / 2 - 4, (int)origin.Location.Y + origin.Size.Height / 2 - 4), origin.Mpts,
					origin.Map, origin.Parent));
			Tick = (Tick + 1) % 3600;
		}

		/// <summary>
		/// 水しぶきを上げます。
		/// </summary>
		/// <param name="origin">水しぶきを上げるエンティティ。</param>
		public static void WaterSplash(Entity origin)
		{
			for (var i = 0; i < 13 + GetRand(4); i++)
				origin.Parent.Add(Core.I.EntityRegistry.CreateEntity("WaterSplash",
					new Vector(origin.Location.X + GetRand(origin.Size.Width), origin.Location.Y + origin.Size.Height / 2), origin.Mpts, origin.Map,
					origin.Parent));
		}

		public static void BrokenBlock(Point pos, EntityList collection, Object[] mptobjects)
		{
			for (var i = 0; i < 3 + GetRand(3); i++)
				collection.Add(Core.I.EntityRegistry.CreateEntity("BrokenBlock",
					new Vector(pos.X + GetRand(8) - 4, pos.Y + GetRand(8) - 4), mptobjects, Core.I.CurrentMap.Chips, collection));
		}
	}
}