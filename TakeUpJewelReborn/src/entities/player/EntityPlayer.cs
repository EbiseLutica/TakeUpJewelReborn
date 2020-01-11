﻿using System;
using System.Drawing;
using DotFeather;
using Codeplex.Data;

namespace TakeUpJewel
{
	[EntityRegistry("Player", 0)]
	public class EntityPlayer : EntityLiving
	{
		private int _flowtimer;
		private bool _lshifted;
		private float _spdAddition;
		private float _spddivition;
		private float _spdlimit;

		/// <summary>
		/// アイテムによって無敵になったかどうか。
		/// </summary>
		public bool AteGodItem;

		/// <summary>
		/// 無敵時間。0のときは無敵ではないが、0以上の時は無敵である。
		/// </summary>
		public int GodTime;

		/// <summary>
		/// パワーアップ時間。0より大きいと、Big : Mini 間のパワーアップアニメを再生する。
		/// </summary>
		public int PowerupTime;

		public EntityPlayer(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			CollisionAIs.Add(new AiKillMonster(this));

			IsOnLand = true;
			Form = PlayerForm.Big;
			Size = new Size(12, 14);
		}

		public int Life { get; set; } = 5;

		/// <summary>
		/// 現在のパワーアップ状態を取得します。
		/// </summary>
		public PlayerForm Form { get; set; }

		public override Texture2D[] ImageHandle
		{
			get
			{
				switch (Form)
				{
					default:
						return Core.I.CurrentGender == PlayerGender.Male ? ResourceManager.BigPlayer : ResourceManager.BigPlayerFemale;
					case PlayerForm.Ice:
						return Core.I.CurrentGender == PlayerGender.Male ? ResourceManager.IcePlayer : ResourceManager.IcePlayerFemale;
					case PlayerForm.Magic:
						return Core.I.CurrentGender == PlayerGender.Male
							? ResourceManager.MagicPlayer
							: ResourceManager.MagicPlayerFemale;
					case PlayerForm.Fire:
						return Core.I.CurrentGender == PlayerGender.Male
							? ResourceManager.FirePlayer
							: ResourceManager.FirePlayerFemale;
				}
			}
		}

		public override RectangleF Collision => new RectangleF(new Vector(2, 2).ToPoint(), Size);

		public override EntityGroup MyGroup => EntityGroup.Friend;

		public override Sounds KilledSound => Sounds.PlayerMiss;

		public override void OnUpdate()
		{
			base.OnUpdate();
			Size = new Size(12, 30);
			if (!IsDying && !Core.I.IsGoal)
				InputControl();
			AnimeControl();
			if (!IsDying)
				ProcessGod();
		}

		public override void Move()
		{
			if (PowerupTime == 0)
				base.Move();
		}

		public override void UpdateGravity()
		{
			if (PowerupTime == 0)
				base.UpdateGravity();
		}

		private void ProcessGod()
		{
			if (PowerupTime > 0)
				PowerupTime--;
			if (GodTime > 0)
			{
				GodTime--;
				if ((GodTime == 90) && AteGodItem)
					DESound.Play(Sounds.WarningMuteki);
				if ((GodTime == 0) && AteGodItem)
				{
					AteGodItem = false;
					Core.I.BgmStop();
					if (Core.I.CurrentAreaInfo != null)
						Core.I.BgmPlay(Core.I.CurrentAreaInfo.Music);
				}
			}
		}

		private void AnimeControl()
		{
			switch (Direction)
			{
				case Direction.Left:
					if (IsOnLand)
						SetAnime(0, 3, 8);
					else
						SetGraphic(4);
					if (IsInWater)
						SetAnime(18, 21, 8);
					break;
				case Direction.Right:
					if (IsOnLand)
						SetAnime(6, 9, 8);
					else
						SetGraphic(10);
					if (IsInWater)
						SetAnime(22, 25, 8);
					break;
			}
			if ((Velocity.X < 0.1f) && (Velocity.X > -0.1f))
				AnimeSpeed = 0;
			else
				AnimeSpeed = (int)(10 - _spdlimit + (_spdlimit - Math.Abs(Velocity.X)) * 10);
		}

		public void InputControl()
		{
			if (DFKeyboard.Left)
			{
				Direction = Direction.Left;
				Velocity.X -= _spdAddition;
				if (Velocity.X < -_spdlimit)
					Velocity.X = -_spdlimit;
			}
			else if (DFKeyboard.Right)
			{
				Direction = Direction.Right;
				Velocity.X += _spdAddition;
				if (Velocity.X > _spdlimit)
					Velocity.X = _spdlimit;
			}
			else
			{
				Velocity.X *= _spddivition;
				if ((Velocity.X < 0.1f) && (Velocity.X > -0.1f))
					Velocity.X = 0;
			}

			if (DFKeyboard.Z.IsKeyDown)
			{
				if (!IsOnLand && DFKeyboard.Left && (CollisionLeft() == ObjectHitFlag.Hit))
				{
					Velocity.X = 3f;
					Velocity.Y = -4f;
					DESound.Play(Sounds.Destroy);
				}
				else if (!IsOnLand && DFKeyboard.Right && (CollisionRight() == ObjectHitFlag.Hit))
				{
					Velocity.X = -3f;
					Velocity.Y = -4f;
					DESound.Play(Sounds.Destroy);
				}
				else
				{
					if (IsInWater)
					{
						DESound.Play(Sounds.Swim);
						Velocity.Y = -2f;
						IsJumping = false;
						IsOnLand = true;
					}
					else if (IsOnLand || (_flowtimer < 10))
					{
						if (!IsJumping)
							DESound.Play(Sounds.BigJump);
						Velocity.Y = -3.6f - Math.Abs(Velocity.X) / 6.5f;
						Move();
					}
					IsJumping = true;
				}
			}
			if (IsOnLand)
				_flowtimer = 0;
			else
				_flowtimer++;

			if (!DFKeyboard.Z && IsJumping)
				Velocity.Y += 0.1f;

			if (DFKeyboard.ShiftLeft)
			{
				_spdAddition = 0.4f;
				_spddivition = 0.9f;
				_spdlimit = 2.7f;
				if (!_lshifted)
					switch (Form)
					{
						case PlayerForm.Fire:
							DESound.Play(Sounds.ShootFire);
							Parent.Add(
								new EntityFireWeapon(Location, Mpts, Map, Parent).SetEntityData(
									DynamicJson.Parse(@"{""SpeedX"": " +
													  (Direction == Direction.Right ? EntityFireWeapon.SpeedX : -EntityFireWeapon.SpeedX) + "}")));
							break;
						case PlayerForm.Ice:
							DESound.Play(Sounds.ShootFire);
							Parent.Add(
								new EntityIceWeapon(Location, Mpts, Map, Parent).SetEntityData(
									DynamicJson.Parse(@"{""SpeedX"": " +
													  (Direction == Direction.Right ? EntityIceWeapon.SpeedX : -EntityIceWeapon.SpeedX) + "}")));
							break;
						case PlayerForm.Magic:
							DESound.Play(Sounds.ShootFire);
							Parent.Add(new Weapons(Location, Mpts, Map, Parent));
							break;
					}

				foreach (EntityLiving entity in Parent.FindEntitiesByType<EntityLiving>())
				{
					var e = entity;
					if (e.IsDying)
						continue;
					if ((GodTime > 0) && AteGodItem &&
						((e.MyGroup == EntityGroup.Enemy) || (e.MyGroup == EntityGroup.MonsterWeapon)) &&
						new RectangleF(e.Location.X, e.Location.Y, e.Size.Width, e.Size.Height).CheckCollision(new RectangleF(Location.X,
							Location.Y, Size.Width, Size.Height)))
						e.Kill();
				}

				foreach (EntityTurcosShell e in Parent.FindEntitiesByType<EntityTurcosShell>())
				{
					if (e.IsRunning)
						if (new RectangleF(Location.ToPoint(), Size).CheckCollision(new RectangleF(e.Location.ToPoint(), e.Size)))
							e.Owner = this;
				}
				_lshifted = true;
			}
			else
			{
				_lshifted = false;
				_spdAddition = 0.2f;
				_spddivition = 0.9f;
				_spdlimit = 1.4f;
			}
		}

		public override void OnOutOfWater()
		{
			Velocity.Y = -3.2f;
			base.OnOutOfWater();
		}

		/// <summary>
		/// この EntityPlayer を殺害します。
		/// </summary>
		public override void Kill()
		{
			if (IsDying)
				return;
			if (IsFall)
			{
				base.Kill();
				Velocity = Vector.Zero;
				Life = 0;
				DESound.Play(Sounds.PlayerMiss);
				return;
			}
			if (Core.I.Time == 0)
			{
				SetGraphic(5);
				base.Kill();
				return;
			}
			if (GodTime > 0)
				return;
			DESound.Play(Sounds.PowerDown);
			GodTime = 240;
			Life--;

			if (Form != PlayerForm.Big)
				Form = PlayerForm.Big;

			if (Life < 1)
			{
				SetGraphic(5);
				base.Kill();
			}

			Velocity = Vector.Zero;
		}

		public override void OnUpdate(Vector p, IDrawable d)
		{
			if (!(d is Sprite sprite)) return;

			base.OnUpdate(p, d);

			var cond = (GodTime > 0) && (PowerupTime == 0) && GodTime % 8 < 4;

			sprite.Color = Color.FromArgb(cond ? 0 : 255, 255, 255, 255);
			sprite.Location = new Vector(p.X, p.Y + ((Core.I.Tick % 8 < 4) && (PowerupTime > 0) ? 16 : 0));
		}

		internal void PowerUp(PlayerForm f)
		{
			switch (f)
			{
				case PlayerForm.Fire:
				case PlayerForm.Magic:
				case PlayerForm.Ice:
					DESound.Play(Sounds.GetWeapon);
					Size = new Size(12, 30);
					break;
			}
			Form = f;
		}

		public override void SetKilledAnime()
		{
			SetGraphic(5);
		}

		public override void SetCrushedAnime()
		{
			SetGraphic(5);
		}

		internal void SetMuteki()
		{
			AteGodItem = true;
			GodTime = 600;

			DESound.Play(Sounds.PowerUp);
			Core.I.BgmPlay("bgm_god.mid");
		}
	}
}