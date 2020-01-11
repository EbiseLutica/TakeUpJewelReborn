namespace TakeUpJewel
{
	public class PlayerGodEventArgs : PreEventArgs
	{
		public int Time { get; set; }
		public bool ByItem { get; private set; }

		public PlayerGodEventArgs(int time, bool byItem) => (Time, ByItem) = (time, byItem);
	}
}
