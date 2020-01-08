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

		public Player Groorine { get; }

		public GroorineAudioSource(Stream midi, int latency = 20, int sampleRate = 44100)
		{
			data = SmfParser.Parse(midi);
			Latency = latency;
			SampleRate = sampleRate;
			Groorine = new Player(sampleRate);
			Groorine.Load(data);
			buffer = Groorine.CreateBuffer(Latency);
		}

		public IEnumerable<(short left, short right)> EnumerateSamples(int? loopStart)
		{
			Groorine.Play();
			while (Groorine.IsPlaying)
			{
				Groorine.GetBuffer(buffer);
				for (var i = 0; i < buffer.Length; i += 2)
				{
					yield return (buffer[i], buffer[i + 1]);
				}
			}
			Groorine.Stop();
		}

		private MidiFile data;
		private short[] buffer;
	}
}
