using System;
using System.Collections.Generic;
using DotFeather;

namespace TakeUpJewel
{
	public class StageScene : Scene
	{
		public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
		{
			if (!(Core.I.CurrentAreaInfo is AreaInfo area))
			{
				// todo: エラーシーンを作成してそこで表示するようにする
				throw new InvalidOperationException();
			}

			// Background
			var bg = new Sprite(ResourceManager.LoadTexture(area.BG));
			bg.ZOrder = -2;
			Root.Add(bg);

			// タイル

			// エンティティ

			// タイル

			// Foreground
			if (area.FG is string fg)
			{
				var f = new Sprite(ResourceManager.LoadTexture(fg));
				f.ZOrder = 2;
				Root.Add(f);
			}
		}

		public override void OnUpdate(Router router, GameBase game, DFEventArgs e)
		{

		}
	}
}
