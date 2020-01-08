using System.Drawing;
using DotFeather;

namespace TakeUpJewel
{
    public abstract class EntityVisible : Entity
    {
        public abstract IDrawable OnSpawn();

        public abstract void OnUpdate(Vector p, IDrawable drawable);
    }
}