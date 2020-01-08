using System.Collections.Generic;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	public class AiKillMonster : AiBase
	{
		public AiKillMonster(EntityLiving host) : base(host) { }

		public override bool Use => !HostEntity.IsDying;

		public override void OnUpdate()
		{
			foreach (EntityLiving el in new List<Entity>(HostEntity.Parent.FindEntitiesByType<EntityLiving>()))
			{
				if (el.MyGroup != EntityGroup.Enemy)
					continue;
				if (el.IsDying)
					continue;
				if (
					new Rectangle((int)HostEntity.Location.X, (int)HostEntity.Location.Y + HostEntity.Size.Height - 1,
						HostEntity.Size.Width, 1).CheckCollision(new Rectangle((int)el.Location.X, (int)el.Location.Y, el.Size.Width,
						el.Size.Height / 4)))
				{
					if (DFKeyboard.Z)
						HostEntity.Velocity.Y = -3;
					else
						HostEntity.Velocity.Y = -1f;
					el.Kill(false, true);
					DESound.Play(Sounds.Stepped);
				}
			}
		}
	}
}