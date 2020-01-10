using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using DotFeather;
using Codeplex.Data;

namespace TakeUpJewel
{
	[EntityRegistry("ItemSpawner", 22)]
	public class EntityItemSpawner : Entity
	{
		private Items _item;

		public EntityItemSpawner(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
			Size = new Size(16, 16);
		}

		public override EntityGroup MyGroup => EntityGroup.Stage;


		public override void OnUpdate()
		{
			var lst = new List<Entity>(Parent.FindEntitiesByType<EntityPlayer>());
			foreach (var entity in lst)
			{
				var ep = (EntityPlayer)entity;
				if (ep.IsDying)
					continue;
				if (
					new RectangleF(ep.Location.X, ep.Location.Y + 1, ep.Size.Width, ep.Size.Height - 1).CheckCollision(
						new RectangleF(Location.X + 2, Location.Y + 4, 12, 12)) && ep.Velocity.Y < 0)
					OpenItem(ep);
			}
			foreach (var entity in new List<Entity>(Parent.FindEntitiesByType<EntityTurcosShell>()))
			{
				var m = (EntityTurcosShell)entity;
				if (m.IsRunning &&
					new RectangleF(Location.X - 4, Location.Y + 8, 24, 8).CheckCollision(new RectangleF(m.Location.X, m.Location.Y, m.Size.Width, m.Size.Height)))
				{
					try
					{
						OpenItem((EntityPlayer)Parent.First(s => s is EntityPlayer));
					}
					catch
					{
						// 握りつぶす
					}
					break;
				}
			}
		}

		public override Entity SetEntityData(dynamic jsonobj)
		{
			if (jsonobj.IsDefined("EntityType"))
				_item = (Items)jsonobj.EntityType;
			return base.SetEntityData((object)jsonobj);
		}

		private void OpenItem(EntityPlayer player)
		{
			switch (_item)
			{
				case Items.Coin:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("Coin", Location, Mpts, Map, Parent,
						DynamicJson.Parse(@"{""WorkingType"": 1}")));
					break;
				case Items.SoulChocolate:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("SoulChocolate", new Vector(Location.X, Location.Y - 16), Mpts,
						Map, Parent));
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.Grimoire:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("Grimoire", new Vector(Location.X, Location.Y - 16), Mpts, Map,
						Parent));
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.FireWands:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("FireWands", new Vector(Location.X, Location.Y - 16), Mpts, Map,
						Parent));
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.PepperOrPillow:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("FireWands", new Vector(Location.X, Location.Y - 16), Mpts, Map,
							Parent)
					);
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.IcyPendant:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("IcyPendant", new Vector(Location.X, Location.Y - 6), Mpts, Map,
							Parent)
					);
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.IceOrPillow:
					//Parent.Add((player.Form == PlayerForm.Mini) ?
					//	Game.I.EntityRegister.CreateEntity("SoulChocolate", new Vector(Location.X, Location.Y - 16), Mpts, Map, Parent) :
					//	Game.I.EntityRegister.CreateEntity("IcyPendant", new Vector(Location.X, Location.Y - 16), Mpts, Map, Parent)
					//	);
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.LeafOrPillow:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("Grimoire", new Vector(Location.X, Location.Y - 16), Mpts, Map,
							Parent)
					);
					DESound.Play(Sounds.ItemSpawn);
					break;
				case Items.Feather:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("Feather", new Vector(Location.X, Location.Y - 16), Mpts, Map,
						Parent));
					DESound.Play(Sounds.ItemSpawn);
					break;

				case Items.FeatherOrCoin:
					if (player.GodTime != 0)
					{
						Parent.Add((player.GodTime > 0) && player.AteGodItem
								? Core.I.EntityRegistry.CreateEntity("Feather", new Vector(Location.X, Location.Y - 16), Mpts, Map, Parent)
								: Core.I.EntityRegistry.CreateEntity("Coin", new Vector(Location.X, Location.Y - 16), Mpts, Map, Parent,
									DynamicJson.Parse(@"{""WorkingType"": 1}"))
						);
						DESound.Play(Sounds.ItemSpawn);
					}
					else
						Parent.Add(Core.I.EntityRegistry.CreateEntity("Coin", Location, Mpts, Map, Parent,
							DynamicJson.Parse(@"{""WorkingType"": 1}")));
					break;
				case Items.PoisonMushroom:
					Parent.Add(Core.I.EntityRegistry.CreateEntity("PoisonMushroom", new Vector(Location.X, Location.Y - 16), Mpts,
						Map, Parent));
					DESound.Play(Sounds.ItemSpawn);
					break;
			}
			Map[(int)(Location.X / 16), (int)(Location.Y / 16), 0] = 10;
			Kill(); //役目が終わったので殺す
		}
	}
}