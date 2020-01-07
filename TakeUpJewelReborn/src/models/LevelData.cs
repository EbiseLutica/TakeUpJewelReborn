namespace TakeUpJewel.Data
{
    /// <summary>
    /// レベルに関する情報を表します。
    /// </summary>
    public class LevelData
    {
        /// <summary>
        /// このレベルの最初のエリア番号を取得します。
        /// </summary>
        public int FirstArea { get; set; }

        /// <summary>
        /// このレベルに設定された時間を取得します。
        /// </summary>
        public int Time { get; set; }

        /// <summary>
        /// このレベルの説明を取得します。
        /// </summary>
        public string Desc { get; set; }
    }
}