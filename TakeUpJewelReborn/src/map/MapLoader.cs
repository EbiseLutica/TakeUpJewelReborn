using System;
using System.IO;
using System.Linq;

namespace TakeUpJewel
{
    public static class MapLoader
    {
        public static void Save(MapData map, string path)
        {
            var (w, h) = map.Size;

            var bw = new BinaryWriter(new FileStream(path, FileMode.Create));
            bw.Write("CITCHIP".ToArray());
            bw.Write(w);
            bw.Write(h);

            for (var z = 0; z < 2; z++)
                for (var y = 0; y < h; y++)
                    for (var x = 0; x < w; x++)
                        bw.Write(map.Chips[x, y, z]);

            bw.Close();
        }

        public static MapData Load(string path)
        {
            var br = new BinaryReader(new FileStream(path, FileMode.Open));

            if (new string(br.ReadChars(7)) != "CITCHIP")
            {
                br.Close();
                throw new Exception("指定したファイルは、有効な Defender Story マップファイルではありません。");
            }
            var w = br.ReadInt32();
            var h = br.ReadInt32();

            var array = new byte[w, h, 2];

            for (var z = 0; z < 2; z++)
                for (var y = 0; y < h; y++)
                    for (var x = 0; x < w; x++)
                        array[x, y, z] = br.ReadByte();
            br.Close();
            return new MapData(array);
        }
    }
}