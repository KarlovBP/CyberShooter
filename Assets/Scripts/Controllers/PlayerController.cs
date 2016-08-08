using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public AbstractWeapon[] Weapons;
    public float PlayerSpeed;

    private int floorMask;
    private int currentWeaponIndex;
    private int CurrentWeaponIndex
    {
        get { return currentWeaponIndex; }
        set
        {
            if (value >= Weapons.Length)
                currentWeaponIndex = 0;
            else if (value < 0)
                currentWeaponIndex = Weapons.Length - 1;
            else
                currentWeaponIndex = value;
            DeactivateWeapons();
            ActivateCurrentWeapon();
        }
    }

    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
        CurrentWeaponIndex = 0;
        ActivateCurrentWeapon();
    }

    // ReSharper disable once UnusedMember.Local
    private void FixedUpdate()
    {
        Movement();
        ButtonInput();
    }

    private void Movement()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        if (Mathf.Abs(h) + Mathf.Abs(v) > 1)
        {
            h *= 0.5f;
            v *= 0.5f;
        }

        transform.position = transform.position + transform.forward*v*PlayerSpeed;
        transform.position = transform.position + transform.right*h*PlayerSpeed;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, floorMask))
        {
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0.0f;

            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);
            transform.rotation = newRotation;
        }
    }

    private void ButtonInput()
    {
        if (Input.GetButton("Fire1"))
        {
            Weapons[CurrentWeaponIndex].Shoot();
        }

        CurrentWeaponIndex += Input.GetAxis("Mouse ScrollWheel") < 0
            ? -1
            : Input.GetAxis("Mouse ScrollWheel") > 0 ? 1 : 0;

        for (int i = 0; i < Weapons.Length; i++)
        {
            if (Input.GetKeyDown((i + 1).ToString()))
                CurrentWeaponIndex = i;
        }
    }

    private void DeactivateWeapons()
    {
        foreach (AbstractWeapon abstractWeapon in Weapons)
        {
            abstractWeapon.gameObject.SetActive(false);
        }
    }

    private void ActivateCurrentWeapon()
    {
        Weapons[CurrentWeaponIndex].gameObject.SetActive(true);
    }
}