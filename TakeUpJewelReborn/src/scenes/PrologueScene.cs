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
			if (!game.IsFocused) return;

			// この画面に移行した瞬間はZキーを押しているので、
			// 一回離してからZキーを押さないと、高速字送りしないようにする
			if (DFKeyboard.Z.IsKeyUp)
				keyUpPressed = true;

			text.Location += Vector.Up * (DFKeyboard.Z && keyUpPressed ? 128 : 32) * e.DeltaTime;

			if (text.Location.Y < -text.Height)
			{
				Core.I.LoadLevel(1, 1);
				router.ChangeScene<PreStageScene>();
				Core.I.BgmStop();
			}
		}

		private DEText text;
		private bool keyUpPressed;
	}
}
