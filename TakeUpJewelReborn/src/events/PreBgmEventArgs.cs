namespace TakeUpJewel
{
	public class PreBgmEventArgs : PreEventArgs
	{
		public string Bgm { get; set; }

		public PreBgmEventArgs(string bgm) => Bgm = bgm;
	}
}
