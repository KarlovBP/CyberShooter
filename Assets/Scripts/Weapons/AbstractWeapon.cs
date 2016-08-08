using UnityEngine;

public abstract class AbstractWeapon : MonoBehaviour
{
    public int MaxBullets;
    public int MaxBulletsInHolder;
    public int Damage;
    protected int Bullets;
    protected int BulletsInHolder;

    public abstract void Shoot();

    public void Reload()
    {
        if (MaxBulletsInHolder - BulletsInHolder >= Bullets)
        {
            BulletsInHolder += Bullets;
            Bullets = 0;
        }
        else
        {
            Bullets -= MaxBulletsInHolder - BulletsInHolder;
            BulletsInHolder = MaxBulletsInHolder;
        }
    }
}