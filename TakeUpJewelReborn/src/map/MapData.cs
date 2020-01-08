using DotFeather;

namespace TakeUpJewel
{
    public class MapData
    {
        public byte[,,] Chips { get; private set; }
        public VectorInt Size => new VectorInt(Chips.GetLength(0), Chips.GetLength(1));

        public MapData(byte[,,] chips) => Chips = chips;
    }
}