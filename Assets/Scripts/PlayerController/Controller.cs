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
		if (Input.GetAxis ("Mouse ScrollWheel") < 0 || Input.GetKeyDown("1"))
		{
			curWeapon -= 1;
			Switch ();
		}
		if (Input.GetAxis ("Mouse ScrollWheel") > 0 || Input.GetKeyDown("2"))
		{
			curWeapon += 1;
			Switch ();
		}
		if (curWeapon > 2) {
			curWeapon = 1;
			Switch ();
		}
		if (curWeapon <1) {
			curWeapon = 2;
			Switch ();
		}

	

	}
	void Null(){
		gun1.SetActive (false);
		gun2.SetActive (false);
			}
	void Switch(){
		if (curWeapon == 1) 
		{
			Null ();
			gun1.SetActive (true); 
		}
		if (curWeapon == 2) 
		{
			Null ();
			gun2.SetActive (true); 
		}
	}

}
