namespace TakeUpJewel
{
    /// <summary>
    /// ゲームのアイテムを指定します。
    /// </summary>
    public enum Items
    {
        /// <summary>
        /// ライフアップするチョコ。
        /// </summary>
        SoulChocolate,

        /// <summary>
        /// ファイアーステッキ。
        /// </summary>
        FireWands,

        /// <summary>
        /// アイシーペンダント。
        /// </summary>
        IcyPendant,

        /// <summary>
        /// 魔導書。
        /// </summary>
        Grimoire,

        /// <summary>
        /// 毒キノコ。
        /// </summary>
        PoisonMushroom,

        /// <summary>
        /// 羽。
        /// </summary>
        Feather,

        /// <summary>
        /// 唐辛子、プレイヤーが小さければ枕。
        /// </summary>
        PepperOrPillow,

        /// <summary>
        /// 唐辛子、プレイヤーが小さければ枕。
        /// </summary>
        IceOrPillow,

        /// <summary>
        /// 唐辛子、プレイヤーが小さければ枕。
        /// </summary>
        LeafOrPillow,

        /// <summary>
        /// 唐辛子、羽状態でなければコイン。
        /// </summary>
        FeatherOrCoin,

        /// <summary>
        /// コイン。
        /// </summary>
        Coin
    }

    /// <summary>
    /// コインの挙動タイプを指定します。
    /// </summary>
    public enum WorkingType
    {
        /// <summary>
        /// メインキャラが触れると取得できるタイプ。
        /// </summary>
        Normal,

        /// <summary>
        /// ブロックから出てくるタイプ。
        /// </summary>
        FromBlock
    }
}