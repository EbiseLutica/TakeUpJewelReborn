using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotFeather;

namespace TakeUpJewel
{
	public class PrologueScene : Scene
	{
		public override void OnStart(Dictionary<string, object> args)
		{
			if (DF.Window.IsFocused && DFKeyboard.Enter)
			{
				hasToSkip = true;
				return;
			}

			BackgroundColor = Color.White;
			Core.I.BgmPlay("bgm_prologue.mid");
			var file = Core.I.CurrentGender == PlayerGender.Male ? "male" : "female";

			var prologue = File.ReadAllText($"Resources/Document/prolog-{file}.txt");

			text = new DEText(prologue, Color.Black);
			text.Location = new Vector(16, Const.Height + 16);
			Root.Add(text);
		}

		public override void OnUpdate()
		{
			if (hasToSkip)
			{
				Go();
				return;
			}
			if (!DF.Window.IsFocused) return;

			// この画面に移行した瞬間はZキーを押しているので、
			// 一回離してからZキーを押さないと、高速字送りしないようにする
			if (DFKeyboard.Z.IsKeyUp)
				keyUpPressed = true;

			text.Location += Vector.Up * (DFKeyboard.Z && keyUpPressed ? 64 : 8) * Time.DeltaTime;

			if (text.Location.Y < -text.Height - 8)
			{
				Go();
			}
		}

		private void Go()
		{
			Core.I.LoadLevel(1, 1);
			DF.Router.ChangeScene<PreStageScene>();
			Core.I.BgmStop();
		}

		private DEText? text;
		private bool keyUpPressed;
		private bool hasToSkip;
	}
}
