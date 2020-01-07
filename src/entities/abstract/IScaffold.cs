using System.Drawing;
using DotFeather;

namespace TakeUpJewel.Entities
{
    public interface IScaffold
    {
        Vector Location { get; }

        RectangleF Collision { get; }
    }
}