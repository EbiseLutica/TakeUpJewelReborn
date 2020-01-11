using System;

namespace TakeUpJewel
{
	public class PreEventArgs : EventArgs
	{
		public bool IsCanceled { get; protected set; }

		public void Cancel() => IsCanceled = true;
	}
}
