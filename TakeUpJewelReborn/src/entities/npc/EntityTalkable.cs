﻿using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	/// <summary>
	/// プレイヤーが話しかけられる Entity です。
	/// </summary>
	[EntityRegistry(nameof(EntityTalkable), 89)]
	public class EntityTalkable : EntityNpc
	{
		private bool _canExecuteScript;
		private string _myScript;

		public EntityTalkable(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
			: base(pnt, obj, chips, par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
		}

		public override EntityGroup MyGroup => EntityGroup.System;

		/// <summary>
		/// 死んでいるアニメーションを設定します。
		/// </summary>
		public override void SetKilledAnime()
		{
		}

		/// <summary>
		/// 踏みつけられたアニメーションを設定します。
		/// </summary>
		public override void SetCrushedAnime()
		{
		}

		/// <summary>
		/// Tick 毎に呼ばれる Entity の処理イベントです。
		/// </summary>
		/// <param name="ks"></param>
		public override void OnUpdate()
		{
			foreach (EntityPlayer ep in Parent.FindEntitiesByType<EntityPlayer>())
			{
				if (ep.IsDying)
					continue;


				// プレイヤーと自分の当たり判定があり、上キーが押されたとき、スクリプト実行
				if ((_canExecuteScript = new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
						.CheckCollision(new Rectangle((int)Location.X, (int)Location.Y, Size.Width,
							Size.Height))) && DFKeyboard.Up)
					try
					{
						EventRuntime.AddScript(new EventScript(_myScript));
					}
					catch (EventScript.EventScriptException ex)
					{
						EventRuntime.AddScript(new EventScript($@"[enstop]
[mesbox:down]
[mes:""エラー！\n{ex.Message.Replace(@"\", @"\\").Replace(@"""", @"\""")}""]
[mesend]
[enstart]"));
					}
			}
			base.OnUpdate();
		}

		public override ElementBase OnSpawn()
		{
			return base.OnSpawn();
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
