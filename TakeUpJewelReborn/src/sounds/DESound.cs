using System;
using System.Collections.Generic;
using System.IO;
using DotFeather;

namespace TakeUpJewel.Util
{
    /// <summary>
    /// 効果音データを取得し、再生する機能を提供する静的クラスです。
    /// </summary>
    public static class DESound
    {
        /// <summary>
        /// SoundUtility を初期化します。使用前に必ず行ってください。
        /// </summary>
        public static void Init()
        {
            soundList.Clear();
            player.Gain = 0.25f;
            string file;
            for (var i = 0; File.Exists(file = $"Resources/Sounds/{i}.wav"); i++)
            {
                var handle = new WaveAudioSource(file);
                soundList.Add(handle);
            }
        }

        /// <summary>
        /// 指定されたサウンドを再生します。
        /// </summary>
        /// <param name="snd">再生するサウンド。</param>
        public static void Play(Sounds snd)
        {
            if (snd == Sounds.Null)
                return;
            snd--;
            Play((int)snd);
        }

        public static void Play(int snd)
        {
            if (snd == -1)
                return;
            player.PlayOneShotAsync(soundList[snd]);
        }


        // /// <summary>
        // /// 細かいパラメーターを設定し、指定されたサウンドを再生します。
        // /// </summary>
        // /// <param name="snd">再生するサウンド。</param>
        // /// <param name="freq">周波数(100 ~ 100000)。</param>
        // /// <param name="volume">音量(0 ~ 255)。</param>
        // /// <param name="pan">パンポット(-255 ~ 255)。</param>
        // public static void PlaySound(Sounds snd, int freq, int volume, int pan)
        // {
        //     if (snd == Sounds.Null)
        //         return;
        //     snd--;
        //     SetFrequencySoundMem(freq, soundList[(int)snd]);
        //     ChangeNextPlayVolumeSoundMem(volume, soundList[(int)snd]);
        //     ChangeNextPlayPanSoundMem(pan, soundList[(int)snd]);
        //     if (PlaySoundMem(soundList[(int)snd], DX_PLAYTYPE_BACK) == -1)
        //         throw new Exception("再生に失敗しました");
        //     ChangeNextPlayVolumeSoundMem(255, soundList[(int)snd]);
        //     ChangeNextPlayPanSoundMem(0, soundList[(int)snd]);
        // }

        private static readonly List<IAudioSource> soundList = new List<IAudioSource>();

        private static readonly AudioPlayer player = new AudioPlayer();
    }
}