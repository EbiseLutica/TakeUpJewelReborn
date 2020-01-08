using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TakeUpJewel
{
	public class Logger
	{
		public string Name { get; }


		public void Debug(object data)
		{
			Output(data, "[DEBUG]");
		}

		public Logger(string name = "NO NAME")
		{
			Name = name;
		}

		public void Write(object data)
		{
			Output(data);
		}

		public void Info(object data)
		{
			Output(data, "[INFO]");
		}

		public void Warn(object data)
		{
			Output(data, "[WARN]");
		}

		public void Error(object data)
		{
			Output(data, "[ERROR]");
		}

		protected void Output(object data, string prefix = "")
		{
			LoggerServer.Instance.Output(data, $"{NamePrefix}{prefix ?? ""}");
		}

		protected string NamePrefix => string.IsNullOrEmpty(Name) ? "" : $"[{Name}]";

		protected class LoggerServer : IDisposable
		{
			protected LoggerServer()
			{
				var streams = new List<Stream>();

				// if (Config.Instance.UseFileLogging)
				// {
				// 	// ファイルロギング
				// 	if (!Directory.Exists(Config.Instance.LogPath))
				// 		Directory.CreateDirectory(Config.Instance.LogPath);
				// 	streams.Add(File.OpenWrite(Path.Combine(Config.Instance.LogPath, DateTime.Now.ToString(@"yyMMdd-HHmmss-fff.lo\g"))));
				// }

				// if (Config.Instance.UseConsoleLogging)
				// {
				streams.Add(Console.OpenStandardOutput());
				// }

				loggingStreams = streams.Select(s => new StreamWriter(s)).ToArray();
			}

			public void Dispose()
			{
				loggingStreams?.ToList().ForEach(l => l.Close());
			}

			public void Output(object obj, string prefix = "")
			{
				loggingStreams?.ToList().ForEach(l =>
				{
					l.WriteLine($"{DateTime.Now.ToString("[HH:mm:ss]")}{prefix}: {obj ?? "null"}");
					l.Flush();
				});
			}

			internal static LoggerServer Instance { get; } = new LoggerServer();

			private StreamWriter[] loggingStreams;
		}
	}
}