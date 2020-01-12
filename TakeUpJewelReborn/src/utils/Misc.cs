using System;
using System.Drawing;
using System.Text;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// 開発に便利な機能が用意されています。
	/// </summary>
	public static class Misc
	{
		/// <summary>
		/// 指定した値から一つをランダムに選んで返します。
		/// </summary>
		/// <param name="dat">値の集合。</param>
		/// <returns>値の集合から一つ選ばれたもの。</returns>
		public static T GetRandom<T>(params T[] dat)
		{
			var rnd = new Random();
			return dat[rnd.Next(dat.Length)];
		}

		/// <summary>
		/// ピクセル単位の座標を指定して、当たり判定を算出します。
		/// </summary>
		public static ColliderType CheckHit(int x, int y)
		{
			return CheckHit(new Vector(x, y));
		}

		/// <summary>
		/// ピクセル単位の座標を指定して、当たり判定を算出します。
		/// </summary>
		public static ColliderType CheckHit(Vector position)
		{
			if (IsOutOfRange(position) || Core.I.CurrentMap == null)
				return ColliderType.Air;
			var (x, y) = (VectorInt)position;
			return Core.I.Tiles[Core.I.CurrentMap.Chips[x / 16, y / 16, 0]].CheckHit(x % 16, y % 16);
		}

		/// <summary>
		/// 指定した座標が、マップの範囲から外れているかどうか判定します。
		/// </summary>
		/// <param name="pnt">座標。</param>
		/// <returns>マップの範囲から外れていれば true が返されます。</returns>
		public static bool IsOutOfRange(this Point pnt) => (pnt.X < 0) || (pnt.X > Core.I.CurrentMap.Size.X * 16 - 1) ||
														   (pnt.Y < 0) || (pnt.Y > Core.I.CurrentMap.Size.Y * 16 - 1);

		/// <summary>
		/// 指定した座標が、マップの範囲から外れているかどうか判定します。
		/// </summary>
		/// <param name="pnt">座標。</param>
		/// <returns>マップの範囲から外れていれば true が返されます。</returns>
		public static bool IsOutOfRange(this Vector pnt) => (pnt.X < 0) || (pnt.X > Core.I.CurrentMap.Size.X * 16 - 1) ||
															(pnt.Y < 0) || (pnt.Y > Core.I.CurrentMap.Size.Y * 16 - 1);

		/// <summary>
		/// 矩形同士の当たり判定を計算します。
		/// </summary>
		/// <param name="rect1">矩形。</param>
		/// <param name="rect2">矩形。</param>
		/// <returns>当たっているかどうか。</returns>
		public static bool CheckCollision(this Rectangle rect1, RectangleF rect2)
			=> (rect1.X < rect2.X + rect2.Width) && (rect2.X < rect1.X + rect1.Width) &&
			   (rect1.Y < rect2.Y + rect2.Height) && (rect2.Y < rect1.Y + rect1.Height);

		/// <summary>
		/// 矩形同士の当たり判定を計算します。
		/// </summary>
		/// <param name="rect1">矩形。</param>
		/// <param name="rect2">矩形。</param>
		/// <returns>当たっているかどうか。</returns>
		public static bool CheckCollision(this RectangleF rect1, RectangleF rect2)
			=> (rect1.X < rect2.X + rect2.Width) && (rect2.X < rect1.X + rect1.Width) &&
			   (rect1.Y < rect2.Y + rect2.Height) && (rect2.Y < rect1.Y + rect1.Height);

		/// <summary>
		/// 点と矩形の当たり判定を計算します。
		/// </summary>
		/// <param name="p">点。</param>
		/// <param name="r">矩形。</param>
		/// <returns>当たっているかどうか。</returns>
		public static bool CheckCollision(this Point p, RectangleF r)
			=> (r.X < p.X) && (p.X < r.X + r.Width) && (r.Y < p.Y) && (p.Y < r.Y + r.Height);

		/// <summary>
		/// 点と矩形の当たり判定を計算します。
		/// </summary>
		/// <param name="p">点。</param>
		/// <param name="r">矩形。</param>
		/// <returns>当たっているかどうか。</returns>
		public static bool CheckCollision(this Vector p, RectangleF r)
			=> (r.X < p.X) && (p.X < r.X + r.Width) && (r.Y < p.Y) && (p.Y < r.Y + r.Height);
	}
}