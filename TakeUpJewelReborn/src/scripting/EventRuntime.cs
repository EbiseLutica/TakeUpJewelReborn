using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public static class EventRuntime
	{
		public enum UpDown
		{
			Up,
			Down
		}

		private static readonly Queue<EventScript> EventScriptQueue = new Queue<EventScript>();

		public static EventScript? CurrentScript { get; private set; }

		public static bool MessageIsShowing { get; private set; }

		public static bool BalloonIsShowing { get; private set; }

		public static UpDown MesPos { get; private set; }

		public static string MesBuffer { get; private set; } = "";

		public static bool IsWaitingResponse { get; private set; }

		public static void AddScript(EventScript myScript)
		{
			if (myScript == null)
				return;

			EventScriptQueue.Enqueue(myScript);
		}

		public static IEnumerator Execute()
		{
			if ((CurrentScript == null) && (EventScriptQueue.Count > 0))
				CurrentScript = EventScriptQueue.Dequeue();

			if ((CurrentScript != null) && (CurrentScript.Current != null))
			{
				var args = CurrentScript.Current.Args;
				switch (CurrentScript.Current.Name)
				{
					case "bgm":
						if (args == null)
							Core.I.BgmPlay();
						else
							Core.I.BgmPlay(args[0]);
						break;
					case "bgmstop":

						int time;
						if (args == null)
							Core.I.BgmStop();
						else if (int.TryParse(args[0], out time))
							Core.I.BgmStop(time);
						break;
					case "se":
						int id;
						if (int.TryParse(args[0], out id))
						{
							DESound.Play(id);
						}
						else
						{
							Sounds snd;
							if (Enum.TryParse(args[0], out snd))
								DESound.Play(snd);
						}
						break;
					case "mesbox":
						BalloonIsShowing = true;
						goto case "messtart";
					case "messtart":
						if (args == null)
							MesPos = UpDown.Down;
						else
							switch (args[0])
							{
								case "up":
									MesPos = UpDown.Up;
									break;
								case "down":
									MesPos = UpDown.Down;
									break;
							}
						MessageIsShowing = true;
						break;
					case "mes":
						var mes = args[0];
						var tick = 2;
						if (args.Length >= 2)
							tick = int.Parse(args[1]);

						// DFKeyboard.Z = false;
						// DFKeyboard.Z.IsKeyDown = false;
						foreach (var c in mes)
						{
							MesBuffer += c;

							DESound.Play(Sounds.Speaking);
							for (var i = 0; i < tick / (DFKeyboard.ShiftLeft ? 2 : 1); i++)
								yield return null;
						}
						IsWaitingResponse = true;
						while (!DFKeyboard.Z.IsKeyDown)
							yield return null;
						DESound.Play(Sounds.Selected);
						yield return null;

						IsWaitingResponse = false;
						MesBuffer = "";
						break;
					case "mescont":
						mes = args[0];
						tick = 1;
						if (args.Length >= 2)
							tick = int.Parse(args[1]);

						foreach (var c in mes)
						{
							MesBuffer += c;
							DESound.Play(Sounds.Speaking);
							for (var i = 0; i < tick / (DFKeyboard.ShiftLeft ? 2 : 1); i++)
								yield return null;
						}
						break;
					case "mesnod":

						IsWaitingResponse = true;
						while (!DFKeyboard.Z.IsKeyDown)
							yield return null;
						// DFKeyboard.Z.IsKeyDown = false;

						yield return null;

						DESound.Play(Sounds.Selected);
						IsWaitingResponse = false;

						MesBuffer = "";
						break;
					case "mesend":
						MessageIsShowing = false;
						BalloonIsShowing = false;
						break;
					case "enstop":
						Core.I.IsFreezing = true;
						break;
					case "enstart":
						Core.I.IsFreezing = false;
						break;
					case "wait":
						for (var i = 0; i < int.Parse(args[0]); i++)
							yield return null;
						break;
					case "bgmvol":
						var ch = int.Parse(args[0]);
						var val = byte.Parse(args[1]);
						if (val > 127)
							val = 127;
						if (ch < 0)
							ch = 0;
						if (ch > 15)
							ch = 15;
						// todo
						// Seq.Sm.Channels[ch].Volume = val;
						break;
					case "mpt":
						if (Core.I.CurrentMap is MapData map)
						{
							int.TryParse(args[0], out var x);
							int.TryParse(args[1], out var y);
							var z = args[2];
							if ((z != "表") && (z != "裏"))
								break;
							if (x < 0)
								x = 0;
							if (x > map.Size.X - 1)
								x = map.Size.X - 1;
							if (y < 0)
								y = 0;
							if (y > map.Size.Y - 1)
								y = map.Size.Y - 1;

							byte.TryParse(args[3], out var chip);
							map.Chips[x, y, z == "表" ? 0 : 1] = chip;
						}

						break;
					case "teleport":
					case "tp":
						var e = new PreEventArgs();
						PreTeleport?.Invoke(e);
						if (e.IsCanceled) break;
						int level, area;
						Core.I.IsGoal = false;
						Core.I.Middle = VectorInt.Zero;
						switch (args.Length)
						{
							case 1:
								int.TryParse(args[0], out level);
								Core.I.LoadLevel(level);
								break;
							case 2:
								int.TryParse(args[0], out level);
								int.TryParse(args[1], out area);
								Core.I.LoadLevel(level, area);
								break;
						}
						PostTeleport?.Invoke(new EventArgs());
						break;
				}

				if (CurrentScript.IsEndOfScript)
				{
					Core.I.IsFreezing = false;
					MessageIsShowing = false;
					BalloonIsShowing = false;
					CurrentScript = null;
				}
				else
					CurrentScript.ProgramCounter++;
			}
		}
		public static event StaticEventHandler<PreEventArgs>? PreTeleport;
		public static event StaticEventHandler<EventArgs>? PostTeleport;
	}
}