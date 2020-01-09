using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotFeather;

namespace TakeUpJewel
{
	public class PreStageScene : Scene
	{
		public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
		{
			BackgroundColor = Color.Black;
			text = new DEText($"Level {Core.I.CurrentLevel} {Core.I.CurrentLevelData?.Desc ?? "No Description"}", Color.White);
			text.Location = new Vector(Const.Width / 2 - text.Width / 2, Const.Height / 2 - text.Height);
			Root.Add(text);
			game.StartCoroutine(Main())
				.Then(_ => router.ChangeScene<StageScene>());
		}

		public IEnumerator Main()
		{
			yield return new WaitForSeconds(3);
		}

		private DEText text;
	}
}
