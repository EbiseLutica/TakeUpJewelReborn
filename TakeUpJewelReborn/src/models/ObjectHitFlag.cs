namespace TakeUpJewel
{
    /// <summary>
    /// オブジェクトの当たり判定フラグを指定します。
    /// </summary>
    public enum ObjectHitFlag
    {
        /// <summary>
        /// 何もなし。
        /// </summary>
        NotHit,

        /// <summary>
        /// 物体。
        /// </summary>
        Hit,

        /// <summary>
        /// ダメージを与えるもの。
        /// </summary>
        Damage,

        /// <summary>
        /// 即殺するもの。
        /// </summary>
        Death,

        /// <summary>
        /// 水中。
        /// </summary>
        InWater
    }
}