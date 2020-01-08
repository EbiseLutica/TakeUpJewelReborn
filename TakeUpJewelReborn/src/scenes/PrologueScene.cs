using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotFeather;

namespace TakeUpJewel
{
	public class PrologueScene : Scene
	{
		public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
		{
			BackgroundColor = Color.White;
			var file = Core.I.CurrentGender == PlayerGender.Male ? "male" : "female";

			var prologue = File.ReadAllText($"Resources/Document/prolog-{file}.txt");

			text = new DEText(prologue, Color.Black);
			text.Location = new Vector(16, Const.Height);
			Root.Add(text);
		}

		public override void OnUpdate(Router router, GameBase game, DFEventArgs e)
		{
			text.Location += Vector.Up * 16 * e.DeltaTime;
		}

		private DEText text;
	}
}
