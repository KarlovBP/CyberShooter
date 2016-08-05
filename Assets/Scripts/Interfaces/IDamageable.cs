namespace Assets.Scripts.Interfaces
{
    interface IDamageable
    {
        float Health { get; set; }

        void GetDamage(float dmg);
    }
}
