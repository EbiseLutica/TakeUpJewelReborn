namespace TakeUpJewel
{
    public enum ScriptRepeatingOption
    {
        /// <summary>
        /// 1度きり実行する。
        /// /// </summary>
        NoRepeat,

        /// <summary>
        /// 衝突中はスクリプトを常時実行(ランタイムが処理中でなければ)
        /// </summary>
        RepeatWhileMainEntityOnMe,

        /// <summary>
        /// 一旦離れて再び入ったら再度実行
        /// </summary>
        RepeatWhenMainEntityLeaveAndReenter
    }
}