namespace TakeUpJewel
{
    /// <summary>
    /// エンティティが属するグループを指定します。この値は、エンティティの制御で、他のエンティティに敵対するか、味方するか、無視するかなどを判定するために利用するのが目的です。
    /// </summary>
    public enum EntityGroup
    {
        /// <summary>
        /// 主人公に味方する生き物であることを表します。
        /// </summary>
        Friend,

        /// <summary>
        /// 主人公に敵対する生き物であることを表します。
        /// </summary>
        Enemy,

        /// <summary>
        /// 制御のためのエンティティ (スクリプト実行など...) であることを表します。
        /// </summary>
        System,

        /// <summary>
        /// ステージの仕掛け (背景や絵画や仕掛けなど...) であることを表します。
        /// </summary>
        Stage,

        /// <summary>
        /// 味方側の武器であることを表します。
        /// </summary>
        DefenderWeapon,

        /// <summary>
        /// 敵側の武器であることを表します。
        /// </summary>
        MonsterWeapon,

        /// <summary>
        /// パーティクルを表します。
        /// </summary>
        Particle,

        /// <summary>
        /// その他を表します。
        /// </summary>
        Other
    }
}