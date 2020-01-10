using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// MainEntity が衝突した瞬間 スクリプトが発動するEntity。
	/// </summary>
	[EntityRegistry(nameof(EntityTrigger), 91)]
	public class EntityTrigger : Entity
	{
		private bool _bTriggered;
		private bool _executed;
		private string _myScript;
		private ScriptRepeatingOption _option;
		private bool _triggered;

		public EntityTrigger(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
		}

		public override EntityGroup MyGroup => EntityGroup.System;


		/// <summary>
		/// Tick 毎に呼ばれる Entity の処理イベントです。
		/// </summary>
		/// <param name="ks"></param>
		public override void OnUpdate()
		{
			var ep = Parent.MainEntity;
			base.OnUpdate();
			// プレイヤーと自分の当たり判定があったとき、スクリプト実行。
			// ReSharper disable once AssignmentInConditionalExpression
			if (_triggered = new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
				.CheckCollision(new Rectangle((int)Location.X, (int)Location.Y, Size.Width,
					Size.Height)))
			{
				// 実行済みなら実行しない
				if ((_option == ScriptRepeatingOption.NoRepeat) && _executed)
					return;
				if ((_option == ScriptRepeatingOption.RepeatWhenMainEntityLeaveAndReenter) && _bTriggered)
					return;

				EventRuntime.AddScript(new EventScript(_myScript));
				_executed = true;
			}
		}

		/// <summary>
		/// Entity 生成時にメタデータが渡されると、このメソッドが呼ばれます。
		/// </summary>
		/// <param name="jsonobj"></param>
		/// <returns></returns>
		public override Entity SetEntityData(dynamic jsonobj)
		{
			if (jsonobj.Script())
				_myScript = jsonobj.Script;

			base.SetEntityData((object)jsonobj);
			return this;
		}
	}
}