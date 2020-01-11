using DotFeather;

namespace TakeUpJewel
{
	public class PlayerMoveEventArgs : PreEventArgs
	{
		public Vector Velocity { get; set; }

		public PlayerMoveEventArgs(Vector velocity) => Velocity = velocity;
	}
}
