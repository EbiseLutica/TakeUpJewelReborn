using System;
using System.Collections.Generic;
using DotFeather;

namespace TakeUpJewel
{
	public class StageScene : Scene
	{
		public override void OnStart(Router router, GameBase game, Dictionary<string, object> args)
		{
			if (!(Core.I.CurrentAreaInfo is AreaInfo area && Core.I.CurrentMap is MapData map))
			{
				// todo: エラーシーンを作成してそこで表示するようにする
				throw new InvalidOperationException();
			}

			// Background
			var bg = new Sprite(ResourceManager.LoadTexture(area.BG));
			bg.ZOrder = -2;
			Root.Add(bg);

			// タイル
			var backTile = new Tilemap(Vector.One * 16);
			var foreTile = new Tilemap(Vector.One * 16);
			var chips = map.Chips;
			for (var y = 0; y < map.Size.Y; y++)
				for (var x = 0; x < map.Size.X; x++)
				{
					backTile[x, y] = Core.I.Mpts[chips[x, y, 1]];
					foreTile[x, y] = Core.I.Mpts[chips[x, y, 0]];
				}

			// エンティティ

			stage.Add(backTile);

			foreach (var d in Core.I.Entities.Drawables)
				stage.Add(d);

			stage.Add(foreTile);

			// Foreground
			if (area.FG is string fg)
			{
				var f = new Sprite(ResourceManager.LoadTexture(fg));
				f.ZOrder = 2;
				Root.Add(f);
			}

			if (Core.I.Middle != Vector.Zero)
			{
				Core.I.Entities.MainEntity.Location = Core.I.Middle;
			}

			var main = Core.I.Entities.MainEntity;

			if (main is EntityPlayer player)
			{
				player.Form = Core.I.CurrentForm;
			}

			Core.I.Camera = -main.Location;

			Core.I.BgmPlay(Core.I.CurrentAreaInfo.Music);
			Root.Add(stage);
		}

		public override void OnUpdate(Router router, GameBase game, DFEventArgs e)
		{
			ControlCamera();

			Core.I.Entities.Draw();
			Core.I.Entities.Update();

			stage.Location = Core.I.Camera;
		}

		private void ControlCamera()
		{
			var main = Core.I.Entities.MainEntity;
			if (main is EntityLiving living && living.IsDying) return;

			if (Core.I.CurrentMap == null) return;

			var (width, height) = Core.I.CurrentMap.Size;
			var (cx, cy) = Core.I.Camera;
			var (mx, my) = main.Location;

			if ((mx + cx > Const.Width / 2) && (cx > -width * 16 + Const.Width))
				Core.I.Camera = new Vector(-(int)mx + Const.Width / 2, cy);

			if ((width * 16 - mx > Const.Width / 2) && cx < 0)
				Core.I.Camera = new Vector(-(int)mx + Const.Width / 2, cy);

			if ((my + cy > Const.Height / 2) && (cy > -height * 16 + Const.Height))
				Core.I.Camera = new Vector(cx, -(int)my + Const.Height / 2);

			if ((height * 16 - my > Const.Height / 2) && cy < 0)
				Core.I.Camera = new Vector(cx, -(int)my + Const.Height / 2);

			if (cx > 0)
				Core.I.Camera = new Vector(0, cy);

			if (cy > 0)
				Core.I.Camera = new Vector(cx, 0);

			if (cx < -width * 16 + Const.Width)
				Core.I.Camera = new Vector(-width * 16 + Const.Width, cy);

			if (cy < -height * 16 + Const.Height)
				Core.I.Camera = new Vector(cx, -height * 16 + Const.Height);
		}

		private Container stage = new Container();
	}
}
