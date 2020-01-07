using System;
using DotFeather;

namespace TakeUpJewelReborn
{
    public class Entry
    {
        static void Main(string[] args)
        {
            var game = new RoutingGameBase<TitleScene>(640, 480, "Take Up Jewel", 60, false, true);
            game.Root.Scale = Vector.One * 2;
            game.Run();
        }
    }
}
