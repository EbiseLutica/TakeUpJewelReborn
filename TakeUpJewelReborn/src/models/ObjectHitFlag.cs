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
		Air,

		/// <summary>
		/// 物体。
		/// </summary>
		Land,

		/// <summary>
		/// ダメージを与えるもの。
		/// </summary>
		NeedleLike,

		/// <summary>
		/// 即殺するもの。
		/// </summary>
		PoisonLike,

		/// <summary>
		/// 水中。
		/// </summary>
		UnderWater
	}
}