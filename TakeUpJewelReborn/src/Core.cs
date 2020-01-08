using System;
using DotFeather;

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

		public MapData? CurrentMap { get; set; }

		public int Tick { get; private set; }

		public int NextStage { get; set; }

		public int Time { get; set; }

		public int Coin { get; set; }

		public bool IsDebugMode { get; set; }

		public Vector Camera { get; set; }

		public EntityRegister EntityRegister { get; } = new EntityRegister();

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
			bgmPlayer.Play(ResourceManager.MusicList[id]);
		}

		public void BgmStop(int time = 0)
		{
			bgmPlayer.Stop(time / 1000f);
		}

		public void Load(int level, int area = 1)
		{

		}

		public static int GetRand(int max) => rnd.Next(max);

		internal void _SetTick(int tick) => Tick = tick;

		private static Random rnd = new Random();

		private AudioPlayer bgmPlayer = new AudioPlayer();
	}
}