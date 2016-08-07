using UnityEngine;

public class Controller : MonoBehaviour
{
    public GameObject Gun1;
    public GameObject Gun2;

    public int CurWeapon;
    public float PlayerSpeed;

    private int floorMask;

    // ReSharper disable once UnusedMember.Local
    private void Start()
    {
        floorMask = LayerMask.GetMask("Floor");
    }

    // ReSharper disable once UnusedMember.Local
    private void FixedUpdate()
    {
        SelectGun();
        Movement();
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

    private void SelectGun()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 || Input.GetKeyDown("1"))
            // проверяем прокрутку колеса мыши вниз или нажатие цифры 1
        {
            CurWeapon -= 1; // уменьшаем переменую флага на 1
            Switch(); // вызываем функцию смены оружия
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0 || Input.GetKeyDown("2"))
            // проверяем прокрутку колеса мыши верх или нажатие цифры 2
        {
            CurWeapon += 1; // увеличиваем переменую флага на 1
            Switch(); // вызываем функцию смены оружия
        }
        switch (CurWeapon)
        {
            case 3:
                CurWeapon = 1; // в случае значения больше 3, уменьшаем до 1
                Switch();
                break;
            case 0:
                CurWeapon = 2; // в случае значения больше 3, уменьшаем до 1
                Switch();
                break;

        }
    }

    /*if (curWeapon > 2) { //проверка перемной флага, на значение не больше трех, для того что бы оружие постоянно менялось, не зависимо сколько прокручиваний сделано
			curWeapon = 1; // в случае значения больше 3, уменьшаем до 1
			Switch (); // вызываем функцию смены оружия
		}
		if (curWeapon <1) { //проверка перемной флага, на значение не меньше 1, для того что бы оружие постоянно менялось, не зависимо сколько прокручиваний сделано
			curWeapon = 2; // в случае значения меньше 1, увеличиваем  до  2
			Switch (); // вызываем функцию смены оружия*/





    private void Null()
    {
        // функция деактивации оружия, для того что бы предыдущее оружие пропадало, перед появлением нового
        Gun1.SetActive(false);
        Gun2.SetActive(false);
    }

    private void Switch()
    {
        //функция переключения оружия
        switch (CurWeapon)
        {
            case 1:
                Null(); //деактивация предыдущего оружия
                Gun1.SetActive(true); //активация нового оружия
                break;
            case 2:
                Null(); //деактивация предыдущего оружия
                Gun2.SetActive(true); //активация нового оружия}
                break;
        }
    }
}