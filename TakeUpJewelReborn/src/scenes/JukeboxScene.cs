using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotFeather;

namespace TakeUpJewel
{
	public class JukeboxScene : Scene
	{
		public override void OnStart(Dictionary<string, object> args)
		{
			Root.Add(new Sprite(ResourceManager.LoadTexture("bgjukebox.png")));
			Core.I.BgmStop();
			menuItems = AudioList.Select((a, i) => new DEText("　" + a, Color.White)
			{
				Location = new Vector(32, 32 + i * 12),
			}).ToArray();

			Root.Add(new DEText("ジュークボックス", Color.White)
			{
				Location = new Vector(16, 16),
			});

			foreach (var item in menuItems)
				Root.Add(item);
		}

		public override void OnUpdate()
		{
			foreach (var (item, i) in menuItems.Select((item, index) => (item, index)))
			{
				if (selectedIndex == i)
				{
					item.Color = Color.Yellow;
					item.Text = "♪" + item.Text.Substring(1);
				}
				else
				{
					item.Color = Color.White;
					item.Text = "　" + item.Text.Substring(1);
				}
			}
			if (!DF.Window.IsFocused) return;

			if (DFKeyboard.Up.IsKeyDown)
				selectedIndex--;
			if (DFKeyboard.Down.IsKeyDown)
				selectedIndex++;
			if (selectedIndex < 0)
				selectedIndex = menuItems.Length - 1;
			if (selectedIndex > menuItems.Length - 1)
				selectedIndex = 0;
			if (DFKeyboard.Z.IsKeyDown)
			{
				if (selectedIndex == menuItems.Length - 1)
				{
					DESound.Play(Sounds.Back);
					Core.I.BgmStop();
					DF.Router.ChangeScene<TitleScene>();
				}
				else
				{
					Core.I.BgmPlay(AudioFileList[selectedIndex]);
				}
			}
			if (DFKeyboard.X.IsKeyDown)
			{
				Core.I.BgmStop(0);
			}
		}

		private int selectedIndex = 0;

		private DEText[]? menuItems;

		private static readonly string[] AudioList =
		{
			"スパイウェ団より",
			"レジスタ街",
			"シーケンシャル平野",
			"エンカプセル洞窟",
			"リファクタ山",
			"ゴール !",
			"スパイウェ団のアジト",
			"やられた!",
			"ゲームオーバー",
			"戦闘 ! クイーン",
			"戦闘 ! キング",
			"勝利 !",
			"Vサイン",
			"エンディング",
			"( 戻る )"
		};

		private static readonly string[] AudioFileList =
		{
			"bgm_prologue.mid",
			"bgm_resistor.mid",
			"bgm_sequencial.mid",
			"bgm_encapsule.mid",
			"bgm_refactor.mid",
			"jingle_goal.mid",
			"bgm_spyway.mid",
			"bgm_miss.mid",
			"bgm_gameover.mid",
			"icypulse_rec.mid",
			"bgm_vsking.mid",
			"jingle_defeatboss.mid",
			"omedeto.mid",
			"bgm_ending.mid",
		};
	}
}
