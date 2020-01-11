namespace TakeUpJewel
{
	public class PlayerPowerUpEventArgs : PreEventArgs
	{
		public PlayerForm Form { get; set; }

		public PlayerPowerUpEventArgs(PlayerForm form) => Form = form;
	}
}
