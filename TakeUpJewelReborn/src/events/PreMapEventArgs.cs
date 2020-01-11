namespace TakeUpJewel
{
	public class PreMapEventArgs : PreEventArgs
	{
		public MapData Map { get; set; }

		public PreMapEventArgs(MapData map) => Map = map;
	}
}
