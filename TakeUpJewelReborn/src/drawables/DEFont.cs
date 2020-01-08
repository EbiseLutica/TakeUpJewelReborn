using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using DotFeather;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SF = SixLabors.Fonts;
using SI = SixLabors.ImageSharp;

namespace TakeUpJewel
{
	/// <summary>
	/// フォントデータを取得し、文字列描画機能を提供する静的クラスです。
	/// </summary>
	public class DEText : Container
	{
		public string Text
		{
			get => text;
			set
			{
				text = value;
				Render();
			}
		}

		public Color Color
		{
			get => color;
			set
			{
				color = value;
				Render();
			}
		}

		public bool IsSmallFont
		{
			get => isSmallFont;
			set
			{
				isSmallFont = value;
				Render();
			}
		}

		public DEText(string text, Color color, bool isSmallFont = false)
		{
			this.text = text;
			this.color = color;
			this.isSmallFont = isSmallFont;

			if (!isInitialized)
			{
				font = Texture2D.LoadAndSplitFrom("Resources/Graphics/font.png", 16, 24, new VectorInt(10, 10));
				smallFont = Texture2D.LoadAndSplitFrom("Resources/Graphics/font_mini.png", 16, 20, new VectorInt(8, 8));

				//0番目は、存在しない文字があったときに表示する文字なので、1から始まる
				var i = 1;
				foreach (var c in File.ReadAllText("Resources/Document/char.txt"))
				{
					//文字番号を char によって指定できるよう登録する
					fontMap[c] = i++;
				}
				nativeFontMap.Clear();

				var collection = new SF.FontCollection();
				collection.Install("Resources/Font/Font.ttf");
				var family = collection.Families.First();
				nativeFont = new SF.Font(family, 10, SF.FontStyle.Regular);

				isInitialized = true;
			}

			Render();
		}

		private static Texture2D GetNativeFont(char c)
		{
			if (nativeFontMap.ContainsKey(c))
				return nativeFontMap[c];
			var (x, y, w, h) = SF.TextMeasurer.MeasureBounds(c.ToString(), new SF.RendererOptions(nativeFont));
			using var img = new SI.Image<Rgba32>((int)(x + w), (int)(y + h));
			img.Mutate((ctx) => ctx.DrawText(c.ToString(), nativeFont, SI.Color.White, default));
			var tex = Texture2D.LoadFrom(img);
			nativeFontMap[c] = tex;
			return tex;
		}

		private void Render()
		{
			Clear();
			int tx = 0, ty = 0;
			for (var i = 0; i < Text.Length; i++)
			{
				int target;
				if (fontMap.ContainsKey(Text[i]))
					//ある文字ならそれを指定
					target = fontMap[Text[i]];
				else if ((Text[i] == '\r') || (Text[i] == '\n'))
				{
					if ((i + 1 < Text.Length) && (Text[i] == '\r') && (Text[i + 1] == '\n'))
						i++;
					tx = 0;
					ty += IsSmallFont ? 8 : 10;
					continue;
				}
				else if ((Text[i] == ' ') || (Text[i] == '　'))
				{
					//空白文字はないので飛ばす(空白は開く)
					tx += IsSmallFont ? 6 : 8;
					continue;
				}
				else
					//それ以外なら、存在しない文字として出力する
					target = 0;

				var texture =
					target == 0 && !isSmallFont
						? GetNativeFont(Text[i])
						: (isSmallFont ? smallFont : font)[target];

				Add(new Sprite(texture)
				{
					Location = new Vector(tx, ty),
					Color = Color,
				});

				tx += IsSmallFont ?
					(Text[i] < 128 ? 6 : 8) : (Text[i] < 128 ? 8 : 10);
			}
			Width = tx;
			Height = ty + (IsSmallFont ? 8 : 10);
		}

		public static Size GetDrawingSize(string txt)
		{
			if (!isInitialized)
				throw new Exception("Font Utility が初期化されていません。");
			int w = 0, h = 0;
			var bw = 0;
			for (var i = 0; i < txt.Length; i++)
			{
				if (!fontMap.ContainsKey(txt[i]))
					switch (txt[i])
					{
						case '\r':
						case '\n':
							if (bw < w)
								bw = w;
							if ((i + 1 < txt.Length) && (txt[i] == '\r') && (txt[i + 1] == '\n'))
								i++;
							w = 0;
							h += 10;
							continue;
						case ' ':
						case '　':
							w += 8;
							continue;
					}

				w += txt[i] < 128 ? 8 : 10;
			}
			if (bw < w)
				bw = w;
			w = bw;
			h += 10;
			return new Size(w, h);
		}

		public static Size GetMiniDrawingSize(string txt)
		{
			if (!isInitialized)
				throw new Exception("Font Utility が初期化されていません。");
			int w = 0, h = 0, bw = 0;
			for (var i = 0; i < txt.Length; i++)
			{
				if (!fontMap.ContainsKey(txt[i]))
					switch (txt[i])
					{
						case '\r':
						case '\n':
							if (bw < w)
								bw = w;
							if ((i + 1 < txt.Length) && (txt[i] == '\r') && (txt[i + 1] == '\n'))
								i++;
							w = 0;
							h += 8;
							continue;
						case ' ':
						case '　':
							w += 6;
							continue;
					}
				w += txt[i] < 128 ? 6 : 8;
			}
			if (bw < w)
				bw = w;
			w = bw;
			h += 8;

			return new Size(w, h);
		}

		private static readonly Dictionary<char, int> fontMap = new Dictionary<char, int>();
		private static readonly Dictionary<char, Texture2D> nativeFontMap = new Dictionary<char, Texture2D>();
		private static Texture2D[] font = new Texture2D[0];
		private static Texture2D[] smallFont = new Texture2D[0];
		private static SixLabors.Fonts.Font nativeFont;
		private static bool isInitialized;
		private string text;
		private Color color;
		private bool isSmallFont;
	}
}