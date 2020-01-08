using System;
using DotFeather;

namespace TakeUpJewel
{
    /// <summary>
    /// エントリーポイント。
    /// </summary>
    public class Entry
    {
        static void Main(string[] args)
        {
            // ゲームの起動
            var game = new RoutingGameBase<TitleScene>(Const.Width * 2, Const.Height * 2, "Take Up Jewel", 60, false, true);
            // スケーリングを2倍にする
            game.Root.Scale = Vector.One * 2;

            game.Load += (s, e) =>
            {
                Core.I.Initialize();
            };

            game.Update += (s, e) =>
            {

            };

            game.Run();
        }
    }
}
