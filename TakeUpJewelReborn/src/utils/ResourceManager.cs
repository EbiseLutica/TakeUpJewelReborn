using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// ゲームのリソースを取得し管理する静的クラスです。
	/// </summary>
	public static class ResourceManager
	{
		public const int TextureWidth = 256;
		public const int TextureHeight = 64;

		public static Texture2D BgBreakTime { get; set; }
		public static Texture2D BgJukeBox { get; set; }
		public static Texture2D[] FirePlayer { get; private set; }
		public static Texture2D[] IcePlayer { get; private set; }
		public static Texture2D[] MagicPlayer { get; private set; }
		public static Texture2D[] BigPlayer { get; private set; }
		public static Texture2D[] Queen { get; private set; }
		public static Texture2D[] King { get; private set; }
		public static Texture2D[] FirePlayerFemale { get; private set; }
		public static Texture2D[] IcePlayerFemale { get; private set; }
		public static Texture2D[] MagicPlayerFemale { get; private set; }
		public static Texture2D[] BigPlayerFemale { get; private set; }
		public static Texture2D[] CommonMob { get; private set; }
		public static Texture2D[] ModokeeGround { get; private set; }
		public static Texture2D[] ModokeeCave { get; private set; }
		public static Texture2D[] Daemon { get; private set; }
		public static Texture2D[] Archer { get; private set; }
		public static Texture2D[] Weapon { get; private set; }
		public static Texture2D[] Dwarf { get; private set; }
		public static Texture2D[] Spider { get; private set; }
		public static Texture2D[] Turcos { get; private set; }
		public static Texture2D[] TurcosShell { get; private set; }
		public static Texture2D[] Item { get; private set; }
		public static Texture2D[] Particle { get; private set; }
		public static Texture2D[] MapChip { get; private set; }
		public static Texture2D[] Boxer { get; private set; }
		public static Texture2D[] RollingRock { get; private set; }
		public static Texture2D[] Turbo { get; private set; }
		public static Texture2D[] Fighter { get; private set; }
		public static Texture2D[] Densy { get; private set; }
		public static Texture2D[] FolderFly { get; private set; }
		public static Texture2D[] BlackServer { get; private set; }
		public static Texture2D[] CameraMan { get; private set; }
		public static Texture2D[] Logo { get; private set; }
		public static Texture2D[] MapChipMini { get; private set; }
		public static Texture2D[] StrangeFlower { get; private set; }
		public static Texture2D TheEnd { get; private set; }
		public static Texture2D MesBox { get; private set; }

		public static Dictionary<string, IAudioSource> MusicList { get; } = new Dictionary<string, IAudioSource>();
		public static Dictionary<int, Texture2D[]> GraphicsList { get; } = new Dictionary<int, Texture2D[]>();
		public static Dictionary<int, Texture2D> GraphicList { get; } = new Dictionary<int, Texture2D>();

		public static void Init()
		{
			MagicPlayer = LoadAndSplit("Resources/Graphics/spplayer_magic.png", 18, 4, new VectorInt(16, 32));
			IcePlayer = LoadAndSplit("Resources/Graphics/spplayer_ice.png", 18, 4, new VectorInt(16, 32));
			FirePlayer = LoadAndSplit("Resources/Graphics/spplayer_fire.png", 18, 4, new VectorInt(16, 32));
			BigPlayer = LoadAndSplit("Resources/Graphics/spplayer.png", 18, 4, new VectorInt(16, 32));

			MagicPlayerFemale = LoadAndSplit("Resources/Graphics/spfemaleplayer_magic.png", 18, 4, new VectorInt(16, 32));
			IcePlayerFemale = LoadAndSplit("Resources/Graphics/spfemaleplayer_fire.png", 18, 4, new VectorInt(16, 32));
			BigPlayerFemale = LoadAndSplit("Resources/Graphics/spfemaleplayer.png", 18, 4, new VectorInt(16, 32));

			Queen = LoadAndSplit("Resources/Graphics/spqueen.png", 16, 2, new VectorInt(16, 32));
			King = LoadAndSplit("Resources/Graphics/spking.png", 16, 2, new VectorInt(16, 32));
			CommonMob = LoadAndSplit("Resources/Graphics/commonMob.png", 16, 4, new VectorInt(16, 16));
			ModokeeGround = LoadAndSplit("Resources/Graphics/spModokee.png", 8, 1, new VectorInt(32, 16));
			ModokeeCave = LoadAndSplit("Resources/Graphics/spCaveModokee.png", 8, 1, new VectorInt(32, 16));
			Daemon = LoadAndSplit("Resources/Graphics/spdaemon.png", 14, 1, new VectorInt(16, 16));
			Archer = LoadAndSplit("Resources/Graphics/sparcher.png", 8, 1, new VectorInt(32, 32));
			Weapon = LoadAndSplit("Resources/Graphics/spweapon.png", 16, 2, new VectorInt(16, 16));
			Dwarf = LoadAndSplit("Resources/Graphics/spdwarf.png", 5, 1, new VectorInt(16, 32));
			Fighter = LoadAndSplit("Resources/Graphics/spfighter.png", 2, 1, new VectorInt(16, 32));
			Turbo = LoadAndSplit("Resources/Graphics/spturbo.png", 6, 1, new VectorInt(16, 16));
			Boxer = LoadAndSplit("Resources/Graphics/spboxer.png", 2, 1, new VectorInt(32, 32));
			RollingRock = LoadAndSplit("Resources/Graphics/sprollingrock.png", 2, 1, new VectorInt(32, 32));

			Turcos = LoadAndSplit("Resources/Graphics/spTurcos.png", 11, 2, new VectorInt(24, 16));
			TurcosShell = LoadAndSplit("Resources/Graphics/spTurcosShell.png", 4, 1, new VectorInt(16, 16));
			Spider = LoadAndSplit("Resources/Graphics/spSpider.png", 1, 1, new VectorInt(16, 16));
			Logo = LoadAndSplit("Resources/Graphics/logo.png", 2, 1, new VectorInt(180, 101));
			Item = LoadAndSplit("Resources/Graphics/spitem.png", 14, 1, new VectorInt(16, 16));
			Densy = LoadAndSplit("Resources/Graphics/spDensy.png", 4, 1, new VectorInt(16, 16));
			FolderFly = LoadAndSplit("Resources/Graphics/spFolderFly.png", 5, 1, new VectorInt(16, 16));
			BlackServer = LoadAndSplit("Resources/Graphics/spblackserver.png", 10, 1, new VectorInt(16, 32));
			CameraMan = LoadAndSplit("Resources/Graphics/spCameraMan.png", 6, 1, new VectorInt(16, 32));
			Particle = LoadAndSplit("Resources/Graphics/spparticle.png", 15, 1, new VectorInt(8, 8));
			StrangeFlower = LoadAndSplit("Resources/Graphics/spstrangeflower.png", 5, 1, new VectorInt(48, 48));

			TheEnd = Load("Resources/Graphics/theend.png");
			MesBox = Load("Resources/Graphics/uimesbox.png");
			BgJukeBox = Load("Resources/Graphics/bgjukebox.png");
			BgBreakTime = Load("Resources/Graphics/bgbreaktime.png");
			foreach (var s in Directory.GetFiles("./Resources/Music", "*.mid"))
			{
				MusicList[Path.GetFileName(s)] = new GroorineAudioSource(File.OpenRead(s));
				logger.Info($"Loaded music '{Path.GetFileName(s)}'");
			}
		}

		/// <summary>
		/// 指定した画像ファイルを、指定した幅と高さで分割してキャッシュします。
		/// </summary>
		/// <param name="texture"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		/// <returns>GraphicList 辞書のキーである、テクスチャファイル名のハッシュ。</returns>
		public static Texture2D[] LoadTextures(string texture, int width, int height)
		{
			var xNum = TextureWidth / width;
			var yNum = TextureHeight / height;
			var hash = texture.GetHashCode();
			if (GraphicsList.ContainsKey(hash) && (GraphicsList[hash] != null))
			{
				logger.Info($"Use cached image {hash}");
				return GraphicsList[hash];
			}

			var buf = LoadAndSplit($"Resources/Graphics/{texture}", xNum, yNum, new VectorInt(width, height));

			GraphicsList[hash] = buf;
			if (loadingList.ContainsKey(hash) && (loadingList[hash] != null))
				return GraphicsList[hash];
			loadingList[hash] = new TextureLoadingInfo(texture, new Size(width, height));
			return GraphicsList[hash];
		}

		public static Texture2D LoadTexture(string texture)
		{
			var hash = texture.GetHashCode();
			if (GraphicList.ContainsKey(hash))
			{
				logger.Info($"Use cached image {hash}");
				return GraphicList[hash];
			}

			return GraphicList[hash] = Load($"Resources/Graphics/{texture}");
		}

		public static void ReloadTexture()
		{
			foreach (var kvp in GraphicsList)
				foreach (var i in kvp.Value)
					i.Dispose();
			GraphicsList.Clear();
			foreach (var p in loadingList)
			{
				var i = p.Value;
				LoadTextures(i.Texture, i.Size.Width, i.Size.Height);
			}
		}

		public static Texture2D[] GetMpt(string mptname)
		{
			MapChip = LoadAndSplit("Resources/Graphics/" + mptname + ".png", 16, 4, new VectorInt(16, 16));
			MapChipMini = LoadAndSplit("Resources/Graphics/" + mptname + ".png", 32, 8, new VectorInt(8, 8));

			return MapChip;
		}

		private static Texture2D Load(string path)
		{
			var t = Texture2D.LoadFrom(path);
			logger.Info($"Loaded an image '{path}'");
			return t;
		}

		private static Texture2D[] LoadAndSplit(string path, int horizontalCount, int verticalCount, VectorInt size)
		{
			var t = Texture2D.LoadAndSplitFrom(path, horizontalCount, verticalCount, size);
			logger.Info($"Loaded an image '{path}' and generate {horizontalCount * verticalCount} textures");
			return t;
		}

		private static readonly Dictionary<int, TextureLoadingInfo> loadingList = new Dictionary<int, TextureLoadingInfo>();

		private static readonly Logger logger = new Logger(nameof(ResourceManager));

		private class TextureLoadingInfo
		{
			public TextureLoadingInfo(string texture, Size size)
			{
				Texture = texture;
				Size = size;
			}

			public string Texture { get; }
			public Size Size { get; }
		}
	}
}