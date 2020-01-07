using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;
using TakeUpJewel.Util;
using static TakeUpJewel.Util.ResourceUtility;

namespace TakeUpJewel
{
    public class TitleScene : Scene
    {
        public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
        {
            menuItems = new (DEText, Action)[]
            {
                (new DEText(" はじめる", Color.Yellow), () => { }),
                (new DEText(" ジュークボックス", Color.White), () => { }),
                (new DEText(" ヘルプ", Color.White), () => { }),
                (new DEText(" おわる", Color.White), () => game.Exit(0)),
            };

            game.StartCoroutine(OpeningAnimation());
        }

        public override void OnUpdate(Router router, GameBase game, DFEventArgs e)
        {
            if (!openingFinished) return;

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

        private IEnumerator OpeningAnimation()
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

            for (var i = 0; i < 16; i++)
            {
                title.Texture = Logo[1];
                yield return new WaitForSeconds(0.0625f);
                title.Texture = Logo[0];
                yield return new WaitForSeconds(0.0625f);
                if (DFKeyboard.Z.IsKeyDown)
                    break;
            }

            Root.Add(new Sprite(LoadTexture("bglvl2area1.png")));

            var y = title.Location.Y + title.Height + 16;
            var x = 128;

            foreach (var item in menuItems)
            {
                item.text.Location = new Vector(x, y);
                Root.Add(item.text);
                y += 18;
            }

            var copyright = new DEText("(C)2016 ＣCitringo\n(C)2020 Xeltica", Color.White, true);
            copyright.Location = new Vector(0, Const.Height - 16);
            Root.Add(copyright);
            Game.I.BgmPlay("hometownv2.mid");
            openingFinished = true;
        }

        private (DEText text, Action onclick)[] menuItems = new (DEText text, Action onclick)[0];

        private bool openingFinished = false;

        private int selectedIndex = 0;
    }
}
