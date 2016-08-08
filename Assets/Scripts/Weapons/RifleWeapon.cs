using UnityEngine;

public class RifleWeapon : AbstractWeapon
{
    public override void Shoot()
    {
        Debug.Log("Rifle pew-pew-pew");
    }
}