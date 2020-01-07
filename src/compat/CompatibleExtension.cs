using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
    public static class CompatibleExtension
    {
        public static PointF ToPoint(this Vector vec) => new PointF(vec.X, vec.Y);
        public static Vector ToVector(this PointF vec) => new Vector(vec.X, vec.Y);

        public static Point ToPoint(this VectorInt vec) => new Point(vec.X, vec.Y);
        public static VectorInt ToVector(this Point vec) => new VectorInt(vec.X, vec.Y);
    }
}