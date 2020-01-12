using System;
using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("CameraMan", 87)]
	public class EntityCameraMan : EntityLiving
	{
		private bool _isjumping;
		private bool _isTension;

		private int _panime;

		private int _tick;

		/// <summary>
		/// アニメ用タイマー。
		/// </summary>
		private int _timer;

		public EntityCameraMan(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 32);
			CollisionAIs.Add(new AiKillDefender(this));
			Jump();
		}

		public override Texture2D[] ImageHandle => ResourceManager.CameraMan;


		public override EntityGroup MyGroup => EntityGroup.Enemy;

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
			IsCrushed = false;
			AnimeSpeed = 0;
		}

		public void Jump()
		{
			/*if (!IsJumping)
			{
				if (timer > 8)
				{
					timer = -1;
					panime++;
					if (panime == 3)
					{
						IsJumping = true;
						Velocity.Y = -3.4f;
					}
				}
				timer++;
			}
			else
			{
				if (CollisionBottom() == Data.ObjectHitFlag.Hit)
				{
					IsJumping = false;
					panime = 0;
					timer = 0;
				}
			}*/
			if (CollisionBottom() == ColliderType.Land)
			{
				//Velocity.Y = -3.4f;
				if (_isjumping)
				{
					_isjumping = false;
					_timer = -1;
					_panime = 0;
				}
				_timer++;
				if (_timer > 8)
				{
					_timer = 0;
					if (_panime < 3)
						_panime++;
					if (_panime == 3)
					{
						_isjumping = true;
						Velocity.Y = -3.4f;
					}
				}
			}
			SetGraphic(_panime);
		}

		public override void OnUpdate()
		{
			if (!IsDying)
				if (!_isTension)
				{
					if ((Math.Abs(Location.X - Parent.MainEntity.Location.X) < 64) && (Location.Y <= Parent.MainEntity.Location.Y) &&
						(Parent.MainEntity.Location.Y <= Location.Y + Size.Height))
					{
						_isTension = true;
						_tick = 0;
						Velocity.Y = 0.2f;
					}
				}
				else
				{
					if (_tick > 180)
						_tick = -1;
					_tick++;
					if (_tick == 0)
					{
						if (((Math.Abs(Location.X - Parent.MainEntity.Location.X) >= 64) && (Location.Y > Parent.MainEntity.Location.Y)) ||
							(Parent.MainEntity.Location.Y > Location.Y + Size.Height))
						{
							_isTension = false;
							_tick = 0;
							goto nuke;
						}
						//SoundUtility.PlaySound(Sounds.Focus);
						DESound.Play(Sounds.Shutter);
					}

					if ((_tick > 30) && (_tick < 60) && (_tick % 4 == 0))
					{
						Parent.Add(new EntityCameraRazer(Location, Mpts, Map, Parent));
						DESound.Play(Sounds.Razer);
					}

					if (_tick == 60)
						if (((Math.Abs(Location.X - Parent.MainEntity.Location.X) >= 64) && (Location.Y > Parent.MainEntity.Location.Y)) ||
							(Parent.MainEntity.Location.Y > Location.Y + Size.Height))
						{
							_isTension = false;
							_tick = 0;
							goto nuke;
						}


					SetGraphic(0);
				}
			nuke:
			if (!_isTension)
				Jump();
			base.OnUpdate();
		}

		// hack Take Up Jewel で登場しないのでとりあえず放置
		// public override void OnDraw(Vector p)
		// {
		//     base.OnDraw(p);
		//     if ((_tick > 0) && (_tick < 8) && _isTension)
		//         DX.DrawGraphF(p.X - 10, p.Y - 2, ImageHandle[4], 1);
		// }

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			return this;
		}
	}

	[EntityRegistry("CameraRazer", -1)]
	public class EntityCameraRazer : EntityFlying
	{
		public EntityCameraRazer(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 32);
			//CollisionAIs.Add(new AIKillDefender(this));
			SetGraphic(5);
			Velocity.X = -4f;
		}


		public override Texture2D[] ImageHandle => ResourceManager.CameraMan;


		public override EntityGroup MyGroup => EntityGroup.MonsterWeapon;

		public override void SetKilledAnime()
		{
		}

		public override void SetCrushedAnime()
		{
		}

		public override void CheckCollision()
		{
		}

		public override void Kill()
		{
			IsDead = true;
		}

		public override void OnUpdate()
		{
			if ((Location.X < -Size.Width) || (Location.Y < -Size.Height) || (Location.X > Core.I.CurrentMap.Size.X * 16) ||
				(Location.Y > Core.I.CurrentMap.Size.Y * 16))
				Kill();
			foreach (EntityPlayer ep in Parent.FindEntitiesByType<EntityPlayer>())
			{
				if (ep.IsDying)
					continue;
				if (new Rectangle((int)ep.Location.X, (int)ep.Location.Y, ep.Size.Width, ep.Size.Height)
					.CheckCollision(new RectangleF(Location.X, Location.Y + 4, 16, 2)))
					ep.Kill();
			}
			base.OnUpdate();
		}


		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			return this;
		}
	}
}