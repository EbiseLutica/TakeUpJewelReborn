using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public class PreStageScene : Scene
	{
		public override void OnStart(Dictionary<string, object> args)
		{
			BackgroundColor = Color.Black;
			text = new DEText($"Level {Core.I.CurrentLevel} {Core.I.CurrentLevelData?.Desc ?? "No Description"}", Color.White);
			text.Location = new Vector(Const.Width / 2 - text.Width / 2, Const.Height / 2 - text.Height);
			Root.Add(text);
			CoroutineRunner.Start(Main())
				.Then(_ => DF.Router.ChangeScene<StageScene>());
		}

		public IEnumerator Main()
		{
			yield return new WaitForSeconds(3);
		}

		private DEText? text;
	}
}
