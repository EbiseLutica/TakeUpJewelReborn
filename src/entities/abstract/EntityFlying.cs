namespace TakeUpJewel.Entities
{
    public abstract class EntityFlying : EntityLiving
    {
        public override void UpdateGravity()
        {
            if (IsDying)
                base.UpdateGravity();
        }
    }
}