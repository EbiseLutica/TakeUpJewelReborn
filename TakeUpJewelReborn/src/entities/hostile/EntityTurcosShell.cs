using System.Drawing;
using DotFeather;
using TakeUpJewel.AI;
using TakeUpJewel.Data;
using TakeUpJewel.Util;

namespace TakeUpJewel.Entities
{
	public class EntityTurcosShell : EntityLiving
	{
		private const int Speed = 3;


		private const int Mutekimax = 60;
		public bool IsRunning;

		protected AiKiller Killai;
		public int Mutekitime;

		public EntityTurcosShell(Vector pnt, Object[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Killai = new AiKiller(this);
			Size = new Size(16, 16);
			Mutekitime = Mutekimax;
			SetGraphic(0);
		}

		public override Texture2D[] ImageHandle => ResourceManager.TurcosShell;


		public override EntityGroup MyGroup => IsRunning ? EntityGroup.Enemy : EntityGroup.MonsterWeapon;

		public override RectangleF Collision => new RectangleF(2, 2, 12, 14);

		public EntityLiving Owner { get; set; }

		public override void CheckCollision()
		{
			if (!IsDying && ((Owner == null) || IsRunning))
				base.CheckCollision();
		}

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}


		public override void Kill()
		{
			if (Mutekitime > 0)
				return;
			if (IsDying)
				return;
			if (!IsCrushed || IsFall)
			{
				if (IsFall && !IsCrushed)
					IsDead = true;
				else if (!IsCrushed)
				{
					Velocity.Y = -3f;
					IsDying = true;
				}

				return;
			}
			if ((Owner != null) && DFKeyboard.ShiftLeft)
				return;
			SwitchMode();
		}

		protected virtual void SwitchMode()
		{
			if (!IsRunning)
			{
				IsRunning = true;
				CollisionAIs.Add(Killai);
				Mutekitime = Mutekimax;
				Owner = null;
				DESound.Play(Sounds.Killed);
				Velocity.X = Parent.MainEntity.Location.X < Location.X ? Speed : -Speed;
				//Location.X += Velocity.X * 8;
				Velocity.Y = 0;
			}
			else
			{
				IsRunning = false;
				Mutekitime = Mutekimax;
				if (CollisionAIs.Contains(Killai))
					CollisionAIs.Remove(Killai);
				DESound.Play(Sounds.Stepped);
				Velocity.X = 0;
				Velocity.Y = 0;
			}
		}

		public override void OnUpdate()
		{
			if (IsDying)
			{
				base.OnUpdate();
				return;
			}
			if (Mutekitime > 0)
				Mutekitime--;
			if ((CollisionLeft() == ObjectHitFlag.Hit) || (Location.X <= 0))
				Velocity.X = Speed;
			if ((CollisionRight() == ObjectHitFlag.Hit) || (Location.X >= Core.I.CurrentMap.Size.X * 16 - 1))
				Velocity.X = -Speed;

			if ((Owner != null) && !DFKeyboard.ShiftLeft)
			{
				SwitchMode();
				Owner = null;
			}
			if ((Owner != null) && !IsRunning)
				foreach (EntityLiving ep in Parent.FindEntitiesByType<EntityLiving>())
				{
					if ((ep == this) || ep.IsDying || (ep.MyGroup == EntityGroup.Friend) ||
						((ep.MyGroup != EntityGroup.Enemy) && !(ep is EntityTurcosShell)))
						continue;
					if (new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
						.CheckCollision(new Rectangle((int)Owner.Location.X - 4, (int)Owner.Location.Y - 4, Owner.Size.Width + 8,
							Owner.Size.Height + 8)))
					{
						ep.Kill();
						IsDead = true;
					}
				}
			if ((Mutekitime == 0) && !IsRunning && !DFKeyboard.ShiftLeft &&
				new RectangleF(Parent.MainEntity.Location.ToPoint(), Parent.MainEntity.Size).CheckCollision(new RectangleF(Location.ToPoint(), Size)))
				SwitchMode();
			base.OnUpdate();
		}

		public override void Move()
		{
			if (IsDying)
				return;
			if ((Owner != null) && DFKeyboard.ShiftLeft)
				Location = new Vector(Owner.Location.X + (Owner.Direction == Direction.Left ? -Size.Width : Owner.Size.Width),
					Owner.Location.Y + Owner.Size.Height / 2 - Size.Height / 2);
			if ((Owner == null) || IsRunning)
				base.Move();
		}
	}
}