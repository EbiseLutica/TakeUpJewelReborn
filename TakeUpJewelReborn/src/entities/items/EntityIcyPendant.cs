using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("IcyPendant", -1)]
	public class EntityIcyPendant : EntityLiving
	{
		public EntityIcyPendant(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			SetGraphic(4);
		}

		public static bool Nikaime { get; set; }
		public override Texture2D[] ImageHandle => ResourceManager.Item;


		public override EntityGroup MyGroup => EntityGroup.Stage;

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void OnUpdate()
		{
			if (!Nikaime)
			{
				EventRuntime.AddScript(Misc.GenerateItemDescription(@"アイシーペンダント\n触ると　つめたい　アイシークリスタルで　つくられた　ペンダント。",
					"氷ブロックを　発射できるようになる。", "氷ブロックは　床を滑りながら　敵を倒し　滑り続けると　小さくなり　最終的には　消える。"));
				Nikaime = true;
			}

			foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
			{
				var ep = (EntityPlayer)entity;
				if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
				{
					ep.PowerUp(PlayerForm.Ice);
					IsDead = true;
				}
			}
			base.OnUpdate();
		}
	}
}