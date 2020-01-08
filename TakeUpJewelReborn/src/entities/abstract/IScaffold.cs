using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
    public interface IScaffold
    {
        Vector Location { get; }

        RectangleF Collision { get; }
    }
}