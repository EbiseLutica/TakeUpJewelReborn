using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public abstract class EntitySprite : EntityVisible
	{
		/// <summary>
		/// アニメーションの終点。
		/// </summary>
		public int AnimeEndIndex;

		/// <summary>
		/// アニメーションの速さ(単位は Tick)。
		/// </summary>
		public int AnimeSpeed;

		/// <summary>
		/// アニメーションの始点。
		/// </summary>
		public int AnimeStartIndex;

		/// <summary>
		/// 現在のループ回数。
		/// </summary>
		protected int Looptimes;

		/// <summary>
		/// 最大ループ回数。
		/// </summary>
		public int LoopTimes;

		/// <summary>
		/// 現在の画像のインデックス。
		/// </summary>
		protected int Ptranime;

		/// <summary>
		/// この Entity が使用する画像ハンドルを取得します。
		/// </summary>
		public abstract Texture2D[] ImageHandle { get; }

		public void SetAnime(int startindex, int endindex, int speed)
		{
			if ((AnimeStartIndex == startindex) && (AnimeEndIndex == endindex))
				return;
			Ptranime = AnimeStartIndex = startindex;
			AnimeEndIndex = endindex;
			AnimeSpeed = speed;
			LoopTimes = -1;
		}

		public void SetGraphic(int index)
		{
			Ptranime = AnimeStartIndex = AnimeEndIndex = index;
			AnimeSpeed = 0;
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			ControlAnime();
		}

		public override IDrawable OnSpawn()
		{
			return new Sprite(ImageHandle[Ptranime]);
		}

		public override void OnUpdate(Vector p, IDrawable drawable)
		{
			if (drawable is Sprite s)
			{
				s.Location = p;
				s.Texture = ImageHandle[Ptranime];
			}
		}

		/// <summary>
		/// アニメーションの制御を行います。
		/// </summary>
		public virtual void ControlAnime()
		{
			if ((AnimeSpeed > 0) && (Core.I.Tick % AnimeSpeed == 0))
			{
				if (Ptranime < AnimeStartIndex) // 現在位置が始点より小さければ始点に戻す。
					Ptranime = AnimeStartIndex;

				if (ImageHandle == null) // そもそも ImageHandle が指定されていなかったらなにもせず抜ける。
					return;

				Ptranime++;
				if (Ptranime > AnimeEndIndex) // アニメが最後まで終わったら、ループなどの設定に従って制御する。
				{
					Looptimes++;
					if ((Looptimes >= LoopTimes) && (LoopTimes != -1))
					{
						AnimeSpeed = 0;
						Ptranime--;
					}
					else
						Ptranime = AnimeStartIndex;
				}
			}
			if (AnimeSpeed == 0)
				Ptranime = AnimeStartIndex;
		}
	}
}