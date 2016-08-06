using UnityEngine;
using System.Collections;
using System.Collections.Specialized;

public class Controller : MonoBehaviour {


	public GameObject gun1;
	public GameObject gun2;

	public int curWeapon;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		SelectGun ();
	
	}
	void SelectGun(){
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 || Input.GetKeyDown("1")) // проверяем прокрутку колеса мыши вниз или нажатие цифры 1
		{
			curWeapon -= 1; // уменьшаем переменую флага на 1
			Switch (); // вызываем функцию смены оружия
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 || Input.GetKeyDown("2")) // проверяем прокрутку колеса мыши верх или нажатие цифры 2
		{
			curWeapon += 1; // увеличиваем переменую флага на 1
			Switch (); // вызываем функцию смены оружия
		}
		if (curWeapon > 2) { //проверка перемной флага, на значение не больше трех, для того что бы оружие постоянно менялось, не зависимо сколько прокручиваний сделано
			curWeapon = 1; // в случае значения больше 3, уменьшаем до 1
			Switch (); // вызываем функцию смены оружия
		}
		if (curWeapon <1) { //проверка перемной флага, на значение не меньше 1, для того что бы оружие постоянно менялось, не зависимо сколько прокручиваний сделано
			curWeapon = 2; // в случае значения меньше 1, увеличиваем  до  2
			Switch (); // вызываем функцию смены оружия
		}

	

	}
	void Null(){ // функция деактивации оружия, для того что бы предыдущее оружие пропадало, перед появлением нового
		gun1.SetActive (false);
		gun2.SetActive (false);
			}
	void Switch(){ //функция переключения оружия
		if (curWeapon == 1) //проверка на то, что перемная флага должа быть 1(оружие 1)
		{
			Null (); //деактивация предыдущего оружия
			gun1.SetActive (true); //активация нового оружия
		}
		if (curWeapon == 2) //проверка на то, что перемная флага должа быть 2(оружие 2)
		{
			Null (); //деактивация предыдущего оружия
			gun2.SetActive (true); //активация нового оружия
		}
	}

}
