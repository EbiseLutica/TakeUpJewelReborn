using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DotFeather;
using Groorine;
using Groorine.DataModel;

namespace DotFeather
{
	public class GroorineAudioSource : IAudioSource
	{
		// サンプル数は計算不可能なのでnull
		public int? Samples => null;

		// MIDI音源なので常にステレオ
		public int Channels => 2;

		public int Bits => 16;

		public int SampleRate { get; }

		public int Latency { get; }

		public GroorineAudioSource(Stream midi, int latency = 20, int sampleRate = 44100)
		{
			data = SmfParser.Parse(midi);
			Latency = latency;
			SampleRate = sampleRate;
		}

		public IEnumerable<(short left, short right)> EnumerateSamples(int? loopStart)
		{
			var player = new Player(SampleRate);
			var buffer = player.CreateBuffer(Latency);
			player.Load(data);
			player.Play();
			while (player.IsPlaying)
			{
				player.GetBuffer(buffer);
				for (var i = 0; i < buffer.Length; i += 2)
				{
					yield return (buffer[i], buffer[i + 1]);
				}
			}
			player.Stop();
		}

		private MidiFile data;
	}
}
