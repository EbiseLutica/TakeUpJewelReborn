using System;
using System.IO;
using DotFeather;
using SI = SixLabors.ImageSharp;
using System.Drawing;
using SixLabors.ImageSharp.PixelFormats;
using System.Collections.Generic;
using System.Linq;
using Codeplex.Data;
using System.Threading.Tasks;

namespace TakeUpJewel
{
	public class Core
	{
		public static Core I { get; } = new Core();

		public AudioPlayer player { get; } = new AudioPlayer();

		public PlayerGender CurrentGender { get; set; }

		public bool IsFreezing { get; set; }

		public bool IsGoal { get; set; }

		public Vector Middle { get; set; }

		public int Tick { get; private set; }

		public int NextLevel { get; set; }

		public int Time { get; set; }

		public int Coin { get; set; }

		public bool IsDebugMode { get; set; }

		public Vector Camera { get; set; }

		public EntityRegistry EntityRegistry { get; } = new EntityRegistry();

		public int CurrentLevel { get; private set; }
		public LevelData? CurrentLevelData { get; private set; }

		public int CurrentArea { get; private set; }
		public AreaInfo? CurrentAreaInfo { get; private set; }
		public MapData? CurrentMap { get; private set; }

		public EntityList Entities { get; private set; } = new EntityList();

		public Tile[] Tiles { get; private set; } = new Tile[0];

		public void Initialize()
		{
			ResourceManager.Init();
			DESound.Init();
			bgmPlayer.Gain = 1;
		}

		public void BgmPlay(string? id = null)
		{
			if (id == null)
				return;
			Task.Run(() => bgmPlayer.Play(ResourceManager.MusicList[id]));
		}

		public void BgmStop(int time = 0)
		{
			bgmPlayer.Stop(time / 1000f);
		}

		public void LoadLevel(int level, int area = 1)
		{
			var baseDir = Path.Combine("Resources/Levels", "Level " + level);
			var lvldatPath = Path.Combine(baseDir, "lvldat.json");
			var areaDir = Path.Combine(baseDir, "Area " + area);
			var areaPath = Path.Combine(areaDir, "area.json");
			var mapPath = Path.Combine(areaDir, "map.citmap");
			var spdataPath = Path.Combine(areaDir, "spdata.json");

			if (CurrentLevel != level)
			{
				// レベルが以前と異なる場合は、レベル情報の読み込みをする
				CurrentLevel = level;
				CurrentLevelData = DynamicJson.Parse(File.ReadAllText(lvldatPath));
				logger.Info($"Loaded Level {level} - {CurrentLevelData.Desc}");
			}

			// エリアを読み込む
			CurrentMap = MapLoader.Load(mapPath);
			CurrentArea = area;
			CurrentAreaInfo = DynamicJson.Parse(File.ReadAllText(areaPath));
			logger.Info("Loaded Area {area}");
			ResourceManager.GetMpt(CurrentAreaInfo.Mpt);
			LoadMasks(CurrentAreaInfo.Mpt);

			// エンティティを読み込む
			dynamic spdata = DynamicJson.Parse(File.ReadAllText(spdataPath));

			foreach (dynamic? entity in spdata)
			{
				if (entity == null) continue;
				int id = (int)entity.EntityID;
				var pos = new Vector((float)entity.PosX, (float)entity.PosY);
				string? tag = entity.Tag;
				int z = (int)entity.ZIndex;
				dynamic? data = entity.EntityData;

				var summoned = EntityRegistry.CreateEntity(id, pos, Tiles, CurrentMap.Chips, Entities, data);
				logger.Info($"Summoned entity ID:{id} at {pos} with data {data ?? null}");
			}
		}

		public static int GetRand(int max) => rnd.Next(max);

		private void LoadMasks(string name)
		{
			var path = $"Resources/Graphics/{name}_hj.png";
			using var img = SI.Image.Load<Rgba32>(path);
			Tiles = new Tile[64];

			var hits = Enumerable
				.Range(0, 5)
				.Select(i => img[i, 64])
				.Select(p => Color.FromArgb(p.A, p.R, p.G, p.B))
				.ToList();

			var mask = new byte[16, 16];
			for (var iy = 0; iy < 4; iy++)
				for (var ix = 0; ix < 16; ix++)
				{
					for (var y = 0; y < 16; y++)
						for (var x = 0; x < 16; x++)
						{
							var p = img[x + ix * 16, y + iy * 16];
							mask[x, y] = (byte)hits.IndexOf(Color.FromArgb(p.A, p.R, p.G, p.B));
						}
					Tiles[iy * 16 + ix] = new Tile(ResourceManager.MapChip[iy * 16 + ix], (byte[,])mask.Clone());
				}
		}

		internal void _SetTick(int tick) => Tick = tick;

		private static Random rnd = new Random();

		private AudioPlayer bgmPlayer = new AudioPlayer();

		private Logger logger = new Logger(nameof(Core));
	}
}