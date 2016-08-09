using UnityEngine;

public class GunWeapon : AbstractWeapon
{
    public override void Shoot()
    {
        Debug.Log("Gun pew-pew-pew");
    }
}
