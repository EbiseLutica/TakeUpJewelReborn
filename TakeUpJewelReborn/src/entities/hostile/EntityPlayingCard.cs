using DotFeather;

namespace TakeUpJewel
{
	public class EntityPlayingCard : EntityDocument
	{
		public int Life = 90;

		public EntityPlayingCard(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par) : base(pnt, obj, chips, par)
		{
			if (Core.GetRand(2) == 0)
				SetAnime(21, 24, 8);
			else
				SetAnime(25, 28, 8);
		}

		public override Texture2D[] ImageHandle => ResourceManager.Weapon;

		public override void OnUpdate()
		{
			if (Life-- < 0) Kill();

			base.OnUpdate();
		}
	}
}