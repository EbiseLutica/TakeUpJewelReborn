using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public class EntityPlayerGhost : EntitySprite
	{
		public EntityPlayerGhost(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
		}

		public override Texture2D[] ImageHandle => null;


		public override EntityGroup MyGroup => EntityGroup.Particle;


		/// <summary>
		/// Tick 毎に呼ばれる Entity の処理イベントです。
		/// </summary>
		/// <param name="ks"></param>
		public override void OnUpdate()
		{
			base.OnUpdate();
		}

		/// <summary>
		/// Entity 生成時にメタデータが渡されると、このメソッドが呼ばれます。
		/// </summary>
		/// <param name="jsonobj"></param>
		/// <returns></returns>
		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			return this;
		}
	}
}