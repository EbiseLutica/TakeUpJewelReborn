using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
	[EntityRegistry("SpiderString", 12)]
	public class EntityString : EntityVisible
	{
		private string _targetTag = "";

		public EntityString(Vector pnt, Tile[] obj, byte[,,] chips, EntityList par)
		{
			Location = pnt;
			Mpts = obj;
			Map = chips;
			Parent = par;
		}


		public override EntityGroup MyGroup => EntityGroup.Stage;

		public override Entity SetEntityData(dynamic jsonobj)
		{
			base.SetEntityData((object)jsonobj);
			if (jsonobj.IsDefined("TargetEntityTag"))
				_targetTag = jsonobj.TargetEntityTag;
			return this;
		}

		public override IDrawable OnSpawn()
		{
			// foreach (var te in Parent.FindEntitiesByTag(_targetTag))
			// {
			//     if (te is EntityLiving && ((EntityLiving)te).IsDying)
			//         continue;
			//     var pp = new Vector(ks.Camera.X + te.Location.X, ks.Camera.Y + te.Location.Y);
			//     DX.DrawLine((int)p.X + 8, (int)p.Y + 8, (int)pp.X + 8, (int)pp.Y + 8, DX.GetColor(255, 255, 255));
			// }
			return new Graphic();
		}

		public override void OnUpdate(Vector p, IDrawable drawable)
		{
			if (drawable is Graphic g)
			{
				g.Clear();
				foreach (var target in Parent.FindEntitiesByTag(_targetTag))
				{
					if (target is EntityLiving living && living.IsDying)
						continue;
					g.Line(
						(VectorInt)Location + VectorInt.One * 8,
						(VectorInt)target.Location + VectorInt.One * 8,
						Color.White
					);
				}
			}
		}
	}
}
