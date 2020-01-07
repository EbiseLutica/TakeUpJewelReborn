namespace TakeUpJewel.Data
{
    /// <summary>
    /// エリアに関する情報を表します。
    /// </summary>
    public class AreaInfo
    {
        /// <summary>
        /// このエリアが使用するマップチップ名を取得します。
        /// </summary>
        public string Mpt { get; set; }

        /// <summary>
        /// このエリアの BGM 名を取得します。
        /// </summary>
        public string Music { get; set; }

        /// <summary>
        /// このエリアの BG 画像名を取得します。
        /// </summary>
        public string Bg { get; set; }

        /// <summary>
        /// このエリアの BG のスクロール速度を取得します。
        /// </summary>
        public int ScrollSpeed { get; set; }

        /// <summary>
        /// このエリアの FG 画像名を取得します。
        /// </summary>
        public string Fg { get; set; }

        /// <summary>
        /// このエリアの FG のスクロール速度を取得します。
        /// </summary>
        public int FgScrollSpeed { get; set; }
    }
}