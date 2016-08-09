using UnityEngine;

[RequireComponent(typeof (CharacterController))]
public class PlayerController : MonoBehaviour
{
    public AbstractWeapon[] Weapons;
    public float PlayerSpeed;
    public float PlayerRotationSpeed;

    private new Camera camera;
    private CharacterController controller;
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
        controller = GetComponent<CharacterController>();
        camera = Camera.main;
        LayerMask.GetMask("Floor");
        CurrentWeaponIndex = 0;
        ActivateCurrentWeapon();
    }

    // ReSharper disable once UnusedMember.Local
    private void Update()
    {
        Movement();
        ButtonInput();
    }

    private void Movement()
    {
        Vector3 mousePos = Input.mousePosition;

        mousePos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y,
            camera.transform.position.y - transform.position.y));

        var rotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
        transform.eulerAngles = Vector3.up
                                *Mathf.MoveTowardsAngle(transform.eulerAngles.y, rotation.eulerAngles.y,
                                    PlayerRotationSpeed*Time.deltaTime);

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxis("Vertical"));
        Vector3 motion = input;

        motion *= Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1
            ? 0.7f
            : 1f;
        motion *= PlayerSpeed;

        controller.Move(motion*Time.deltaTime);
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