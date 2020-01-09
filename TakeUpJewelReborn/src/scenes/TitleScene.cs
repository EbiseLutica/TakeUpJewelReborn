using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;
using static TakeUpJewel.ResourceManager;

namespace TakeUpJewel
{
	public class TitleScene : Scene
	{
		public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
		{
			this.router = router;
			this.game = game;

			menuItems = new (DEText, Action)[]
			{
				(new DEText(" はじめる", Color.Yellow), () =>
				{
					state = State.Gender;
					selectedIndex = 0;
					HideMenuItems();
					ShowGenderSelector();
				}),
				(new DEText(" ジュークボックス", Color.White), () =>
				{
					router.ChangeScene<JukeboxScene>();
				}),
				(new DEText(" ヘルプ", Color.White), () =>
				{
					Root.Add(helpImage);
					state = State.Help;
				}),
				(new DEText(" おわる", Color.White), () => game.Exit(0)),
			};

			helpImage = new Sprite(LoadTexture("kbactgame.png"))
			{
				ZOrder = 2
			};

			genderSelectorPrompt = new DEText("どちらであそぶ？", Color.White);
			genderSelectorItems[0] = new DEText(" アレン", Color.White);
			genderSelectorItems[1] = new DEText(" ルーシィ", Color.White);
			genderSelectorItems[2] = new DEText(" もどる", Color.White);

			game.StartCoroutine(OpeningAnimation(game));
		}

		public override void OnUpdate(Router router, GameBase game, DFEventArgs e)
		{
			if (isFirstUpdate)
			{
				switch (Core.I.RunningMode)
				{
					case "debug-stage":
						Core.I.LoadLevel(1, 1);
						router.ChangeScene<PreStageScene>();
						return;
				}
				isFirstUpdate = false;
			}

			if (!openingFinished) return;

			switch (state)
			{
				case State.Menu:
					UpdateMenu();
					break;

				case State.Gender:
					UpdateGenderSelector();
					break;

				case State.Help:
					if (game.IsFocused && DFKeyboard.Z.IsKeyDown)
					{
						Root.Remove(helpImage);
						state = State.Menu;
					}
					break;
			}
		}

		private void UpdateMenu()
		{
			if (!game.IsFocused) return;

			if (DFKeyboard.Up.IsKeyDown)
			{
				selectedIndex--;
				DESound.Play(Sounds.Selected);
			}
			if (DFKeyboard.Down.IsKeyDown)
			{
				selectedIndex++;
				DESound.Play(Sounds.Selected);
			}
			if (selectedIndex < 0)
				selectedIndex = menuItems.Length - 1;
			if (selectedIndex > menuItems.Length - 1)
				selectedIndex = 0;

			for (var i = 0; i < menuItems.Length; i++)
			{
				var item = menuItems[i].text;
				item.Color = selectedIndex == i ? Color.Yellow : Color.White;
				item.Text = (selectedIndex == i ? ">" : " ") + item.Text.Substring(1);
			}

			if (DFKeyboard.Z.IsKeyDown)
			{
				menuItems[selectedIndex].onclick();
				DESound.Play(Sounds.Pressed);
			}
		}

		private void UpdateGenderSelector()
		{
			if (!game.IsFocused) return;

			if (DFKeyboard.Up.IsKeyDown)
			{
				selectedIndex--;
				DESound.Play(Sounds.Selected);
			}
			if (DFKeyboard.Down.IsKeyDown)
			{
				selectedIndex++;
				DESound.Play(Sounds.Selected);
			}
			if (selectedIndex < 0)
				selectedIndex = genderSelectorItems.Length - 1;
			if (selectedIndex > genderSelectorItems.Length - 1)
				selectedIndex = 0;

			for (var i = 0; i < genderSelectorItems.Length; i++)
			{
				var item = genderSelectorItems[i];
				item.Color = selectedIndex == i ? Color.Yellow : Color.White;
				item.Text = (selectedIndex == i ? ">" : " ") + item.Text.Substring(1);
			}

			genderSelectorPrompt.Text = selectedIndex != 2 ? "どちらであそぶ?" : "もどるの?";

			if (DFKeyboard.Z.IsKeyDown)
			{
				DESound.Play(selectedIndex == 2 ? Sounds.Back : Sounds.Pressed);
				switch (selectedIndex)
				{
					case 0:
						Core.I.CurrentGender = PlayerGender.Male;
						router.ChangeScene<PrologueScene>();
						break;
					case 1:
						Core.I.CurrentGender = PlayerGender.Female;
						router.ChangeScene<PrologueScene>();
						break;
					case 2:
						state = State.Menu;
						selectedIndex = 0;
						HideGenderSelector();
						ShowMenuItems();
						break;
				}
			}
		}

		private IEnumerator OpeningAnimation(GameBase game)
		{
			var title = new Sprite(Logo[0])
			{
				Location = new Vector(Const.Width / 2 - Logo[0].Size.X / 2, Const.Height),
				ZOrder = 1,
			};
			Root.Add(title);

			while (title.Location.Y > 16)
			{
				title.Location += Vector.Up;
				yield return null;
				if (DFKeyboard.Z.IsKeyDown)
					break;
			}

			title.Location = new Vector(Const.Width / 2 - Logo[0].Size.X / 2, 16);

			yield return DFKeyboard.Z ? null : new WaitForSeconds(0.5f);

			DESound.Play(Sounds.Flash);

			var flash = game.StartCoroutine(Flash(title));

			var time = 0f;

			do
			{
				yield return null;
				time += Time.DeltaTime;
			} while (time < 2.5f && !DFKeyboard.Z.IsKeyDown);

			game.StopCoroutine(flash);

			title.Texture = Logo[0];

			Root.Add(new Sprite(LoadTexture("bglvl2area1.png")));

			ShowMenuItems();

			var copyright = new DEText("(C)2016 ＣCitringo\n(C)2020 Xeltica", Color.White, true);
			copyright.Location = new Vector(0, Const.Height - copyright.Height);
			Root.Add(copyright);

			var engine = new DEText("Made with DotFeather 2.5.0", Color.White, true);
			engine.Location = new Vector(Const.Width - engine.Width, Const.Height - engine.Height);
			Root.Add(engine);

			Core.I.BgmPlay("hometownv2.mid");
			yield return null;
			openingFinished = true;
		}

		private void ShowMenuItems()
		{
			var y = 16 + 100 + 16;
			var x = 128;

			foreach (var item in menuItems)
			{
				item.text.Location = new Vector(x, y);
				Root.Add(item.text);
				y += 18;
			}
		}

		private IEnumerator Flash(Sprite title)
		{
			while (true)
			{
				title.Texture = Logo[1];
				yield return new WaitForSeconds(0.0625f);
				title.Texture = Logo[0];
				yield return new WaitForSeconds(0.0625f);
			}
		}

		private void HideMenuItems()
		{
			foreach (var item in menuItems)
			{
				Root.Remove(item.text);
			}
		}

		private void ShowGenderSelector()
		{
			var y = 16 + 100 + 16;
			var x = 128;

			genderSelectorPrompt.Location = new Vector(x, y);
			y += 18;
			Root.Add(genderSelectorPrompt);

			foreach (var item in genderSelectorItems)
			{
				item.Location = new Vector(x, y);
				Root.Add(item);
				y += 18;
			}
		}

		private void HideGenderSelector()
		{
			Root.Remove(genderSelectorPrompt);

			foreach (var item in genderSelectorItems)
			{
				Root.Remove(item);
			}
		}

		private bool openingFinished = false;

		private State state = State.Menu;
		private int selectedIndex = 0;

		private (DEText text, Action onclick)[] menuItems = new (DEText text, Action onclick)[0];

		private DEText genderSelectorPrompt;
		private DEText[] genderSelectorItems = new DEText[3];

		private Sprite helpImage;

		private Router router;
		private GameBase game;

		private bool isFirstUpdate = true;

		enum State
		{
			Menu, Gender, Help
		}
	}
}
