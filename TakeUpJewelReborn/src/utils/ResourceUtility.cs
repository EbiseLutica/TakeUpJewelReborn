using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DotFeather;

namespace TakeUpJewel.Util
{
    /// <summary>
    /// ゲームのリソースを取得し管理する静的クラスです。
    /// </summary>
    public static class ResourceUtility
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
        public static Texture2D[] MiniPlayer { get; private set; }
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
        public static Dictionary<int, Texture2D[]> GraphicList { get; } = new Dictionary<int, Texture2D[]>();

        public static void Init()
        {
            MagicPlayer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spplayer_magic.png", 18, 4, new VectorInt(16, 32));
            IcePlayer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spplayer_ice.png", 18, 4, new VectorInt(16, 32));
            FirePlayer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spplayer_fire.png", 18, 4, new VectorInt(16, 32));
            BigPlayer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spplayer.png", 18, 4, new VectorInt(16, 32));
            MiniPlayer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spplayermini.png", 18, 4, new VectorInt(16, 16));

            MagicPlayerFemale = Texture2D.LoadAndSplitFrom("Resources/Graphics/spfemaleplayer_magic.png", 18, 4, new VectorInt(16, 32));
            IcePlayerFemale = Texture2D.LoadAndSplitFrom("Resources/Graphics/spfemaleplayer_fire.png", 18, 4, new VectorInt(16, 32));
            BigPlayerFemale = Texture2D.LoadAndSplitFrom("Resources/Graphics/spfemaleplayer.png", 18, 4, new VectorInt(16, 32));

            Queen = Texture2D.LoadAndSplitFrom("Resources/Graphics/spqueen.png", 16, 2, new VectorInt(16, 32));
            King = Texture2D.LoadAndSplitFrom("Resources/Graphics/spking.png", 16, 2, new VectorInt(16, 32));
            CommonMob = Texture2D.LoadAndSplitFrom("Resources/Graphics/commonMob.png", 16, 4, new VectorInt(16, 16));
            ModokeeGround = Texture2D.LoadAndSplitFrom("Resources/Graphics/spModokee.png", 8, 1, new VectorInt(32, 16));
            ModokeeCave = Texture2D.LoadAndSplitFrom("Resources/Graphics/spCaveModokee.png", 8, 1, new VectorInt(32, 16));
            Daemon = Texture2D.LoadAndSplitFrom("Resources/Graphics/spdaemon.png", 14, 1, new VectorInt(16, 16));
            Archer = Texture2D.LoadAndSplitFrom("Resources/Graphics/sparcher.png", 8, 1, new VectorInt(32, 32));
            Weapon = Texture2D.LoadAndSplitFrom("Resources/Graphics/spweapon.png", 16, 2, new VectorInt(16, 16));
            Dwarf = Texture2D.LoadAndSplitFrom("Resources/Graphics/spdwarf.png", 5, 1, new VectorInt(16, 32));
            Fighter = Texture2D.LoadAndSplitFrom("Resources/Graphics/spfighter.png", 2, 1, new VectorInt(16, 32));
            Turbo = Texture2D.LoadAndSplitFrom("Resources/Graphics/spturbo.png", 6, 1, new VectorInt(16, 16));
            Boxer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spboxer.png", 2, 1, new VectorInt(32, 32));
            RollingRock = Texture2D.LoadAndSplitFrom("Resources/Graphics/sprollingrock.png", 2, 1, new VectorInt(32, 32));

            Turcos = Texture2D.LoadAndSplitFrom("Resources/Graphics/spTurcos.png", 11, 2, new VectorInt(24, 16));
            TurcosShell = Texture2D.LoadAndSplitFrom("Resources/Graphics/spTurcosShell.png", 4, 1, new VectorInt(16, 16));
            Spider = Texture2D.LoadAndSplitFrom("Resources/Graphics/spSpider.png", 1, 1, new VectorInt(16, 16));
            Logo = Texture2D.LoadAndSplitFrom("Resources/Graphics/logo.png", 2, 1, new VectorInt(180, 101));
            Item = Texture2D.LoadAndSplitFrom("Resources/Graphics/spitem.png", 14, 1, new VectorInt(16, 16));
            Densy = Texture2D.LoadAndSplitFrom("Resources/Graphics/spDensy.png", 4, 1, new VectorInt(16, 16));
            FolderFly = Texture2D.LoadAndSplitFrom("Resources/Graphics/spFolderFly.png", 5, 1, new VectorInt(16, 16));
            BlackServer = Texture2D.LoadAndSplitFrom("Resources/Graphics/spblackserver.png", 10, 1, new VectorInt(16, 32));
            CameraMan = Texture2D.LoadAndSplitFrom("Resources/Graphics/spCameraMan.png", 6, 1, new VectorInt(16, 32));
            Particle = Texture2D.LoadAndSplitFrom("Resources/Graphics/spparticle.png", 8, 1, new VectorInt(8, 8));
            StrangeFlower = Texture2D.LoadAndSplitFrom("Resources/Graphics/spstrangeflower.png", 5, 1, new VectorInt(48, 48));

            TheEnd = Texture2D.LoadFrom("Resources/Graphics/theend.png");
            MesBox = Texture2D.LoadFrom("Resources/Graphics/uimesbox.png");
            BgJukeBox = Texture2D.LoadFrom("Resources/Graphics/bgjukebox.png");
            BgBreakTime = Texture2D.LoadFrom("Resources/Graphics/bgbreaktime.png");
            // todo Groorine を持ってきてから作業する
            foreach (var s in Directory.GetFiles("./Resources/Music"))
                MusicList[Path.GetFileName(s)] = new GroorineAudioSource(File.OpenRead(s));
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
            if (GraphicList.ContainsKey(hash) && (GraphicList[hash] != null))
                return GraphicList[hash];

            var buf = Texture2D.LoadAndSplitFrom($"Resources/Graphics/{texture}", xNum, yNum, new VectorInt(width, height));

            GraphicList[hash] = buf;
            if (loadingList.ContainsKey(hash) && (loadingList[hash] != null))
                return GraphicList[hash];
            loadingList[hash] = new TextureLoadingInfo(texture, new Size(width, height));
            return GraphicList[hash];
        }

        public static Texture2D LoadTexture(string texture)
        {
            return Texture2D.LoadFrom($"Resources/Graphics/{texture}");
        }

        public static void ReloadTexture()
        {
            foreach (var kvp in GraphicList)
                foreach (var i in kvp.Value)
                    i.Dispose();
            GraphicList.Clear();
            foreach (var p in loadingList)
            {
                var i = p.Value;
                LoadTextures(i.Texture, i.Size.Width, i.Size.Height);
            }
        }

        public static Texture2D[] GetMpt(string mptname)
        {
            MapChip = Texture2D.LoadAndSplitFrom("Resources/Graphics/" + mptname + ".png", 16, 4, new VectorInt(16, 16));
            MapChipMini = Texture2D.LoadAndSplitFrom("Resources/Graphics/" + mptname + ".png", 32, 8, new VectorInt(8, 8));

            return MapChip;
        }

        private static readonly Dictionary<int, TextureLoadingInfo> loadingList = new Dictionary<int, TextureLoadingInfo>();

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