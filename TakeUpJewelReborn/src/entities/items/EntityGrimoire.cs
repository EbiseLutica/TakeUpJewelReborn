using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("Grimoire", 34)]
	public class EntityGrimoire : EntityLiving
	{
		public EntityGrimoire(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
			SetAnime(10, 13, 8);
			Velocity = new Vector(0, -2.0f);
		}

		public override Texture2D[] ImageHandle => ResourceManager.Item;

		public static bool Nikaime { get; set; }

		public override float Gravity => 0.05f;

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
				EventRuntime.AddScript(Misc.GenerateItemDescription("グリモワール(魔導書)\n魔法の　すべてが　記されている。",
					"魔法弾が撃てるようになる。", "魔法弾は　射程距離が　短いが　最も近くの　モンスターを　たおす。"));
				Nikaime = true;
			}
			Velocity.X *= 0.98f;
			foreach (var entity in Parent.FindEntitiesByType<EntityPlayer>())
			{
				var ep = (EntityPlayer)entity;
				if (!ep.IsDying && new RectangleF(ep.Location.ToPoint(), ep.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
				{
					ep.PowerUp(PlayerForm.Magic);
					IsDead = true;
				}
			}
			base.OnUpdate();
		}
	}
}