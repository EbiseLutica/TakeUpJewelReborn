using DotFeather;
using TakeUpJewel;

DF.Window.Size = (Const.Width * 2, Const.Height * 2);
DF.Window.Title = "Take Up Jewel";
DF.Root.Scale = (2, 2);

DF.Window.Start += () =>
{
	Core.I.Initialize();
	if (args.Length > 0)
	{
		Core.I.RunningMode = args[0].ToLowerInvariant();
	}
	DF.Router.ChangeScene<TitleScene>();
};

DF.Window.Update += () =>
{
	if (DFKeyboard.R.IsKeyDown)
		Core.I.ReloadAudioPlayer();
};

return DF.Run();
