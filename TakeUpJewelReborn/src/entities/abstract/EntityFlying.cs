namespace TakeUpJewel
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