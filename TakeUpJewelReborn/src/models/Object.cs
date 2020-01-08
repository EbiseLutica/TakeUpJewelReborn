namespace TakeUpJewel
{
    /// <summary>
    /// mpt の最小単位を表します。
    /// </summary>
    public class Object
    {
        public Object(int handle, byte[,] mask)
        {
            ImageHandle = handle;
            HitMask = mask;
        }

        public int ImageHandle { get; set; }

        /// <summary>
        /// オブジェクトの当たり判定をビットマップで指定します。
        /// ～記法～
        /// 0...当たらない
        /// 1...当たる
        /// 2...当たるとダメージ
        /// 3...当たると即死
        /// 4...当たると水中
        /// </summary>
        public byte[,] HitMask { get; set; }

        public ObjectHitFlag CheckHit(int x, int y)
        {
            return (ObjectHitFlag)HitMask[x, y];
        }
    }
}