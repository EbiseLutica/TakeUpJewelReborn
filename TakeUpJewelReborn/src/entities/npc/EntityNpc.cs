using System.Drawing;
using DotFeather;
using static TakeUpJewel.ResourceManager;

namespace TakeUpJewel
{
	[EntityRegistry(nameof(EntityNpc), 90)]
	public class EntityNpc : EntityLiving
	{
		public EntityNpc(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
		}


		public override Texture2D[] ImageHandle => textures;


		public override EntityGroup MyGroup => EntityGroup.System;

		public override RectangleF Collision => new RectangleF(default(PointF), Size);

		/// <summary>
		/// 死んでいるアニメーションを設定します。
		/// </summary>
		public override void SetKilledAnime()
		{
		}

		/// <summary>
		/// 踏みつけられたアニメーションを設定します。
		/// </summary>
		public override void SetCrushedAnime()
		{
		}

		/// <summary>
		/// EntityNpc は死にません。
		/// </summary>
		public override void Kill()
		{
		}

		/// <summary>
		/// EntityNpc は死にません。
		/// </summary>
		public override void Kill(bool isfall, bool iscrushed)
		{
		}


		/// <summary>
		/// Entity 生成時にメタデータが渡されると、このメソッドが呼ばれます。
		/// </summary>
		/// <param name="jsonobj"></param>
		/// <returns></returns>
		public override Entity SetEntityData(dynamic jsonobj)
		{
			string textureFile = jsonobj.TextureFile;
			var width = (int)jsonobj.Width;
			var height = (int)jsonobj.Height;
			var startIndex = (int)jsonobj.StartIndex;
			var endIndex = (int)jsonobj.EndIndex;
			var speed = (int)jsonobj.Speed;
			textures = LoadTextures(textureFile, width, height);

			if (jsonobj.UseAnime)
				SetAnime(startIndex, endIndex, speed);
			else
				SetGraphic(startIndex);

			Size = new Size(width, height);

			base.SetEntityData((object)jsonobj);
			return this;
		}
		private Texture2D[] textures;
	}
}
