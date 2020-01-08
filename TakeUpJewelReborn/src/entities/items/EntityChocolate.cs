using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("SoulChocolate", 32)]
	public class EntityChocolate : EntityFlying
	{
		public EntityChocolate(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			Velocity = new Vector(0, -2.4f);
			SetGraphic(5);
		}

		public static bool Nikaime { get; set; }
		public override Texture2D[] ImageHandle => ResourceManager.Item;


		public override EntityGroup MyGroup => EntityGroup.Stage;

		public override RectangleF Collision => new RectangleF(0, 0, 16, 16);

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
				EventRuntime.AddScript(Misc.GenerateItemDescription(@"ソウルチョコレート\n命の　すべてが　詰まった　フシギな　チョコレート。", "プレイヤーの　体力が　1だけ　回復する。"));
				Nikaime = true;
			}
			if (Velocity.Y < 0)
				Velocity.Y += 0.1f;
			else if (MainAi == null)
				MainAi = new AiFlySine(this, 0, 5, 5, 5, 5);
			foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
			{
				var ep = (EntityPlayer)entity;
				if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
				{
					ep.Life++;
					DESound.Play(Sounds.LifeUp);
					IsDead = true;
				}
			}
			base.OnUpdate();
		}
	}
}