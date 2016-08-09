using UnityEngine;
using System.Collections;

public class camMovement : MonoBehaviour {
	Transform selfTransform,mainCamTransform; //сохраняем трансформ нашего объекта и камеры 
	[SerializeField]
	Camera mainView;    //вешаем сюда нашу камеру
	Vector3 wantedPosition;
	// Use this for initialization
	void Start () {
		mainCamTransform = mainView.transform;
		selfTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		wantedPosition = new Vector3 (selfTransform.position.x, mainCamTransform.position.y, mainCamTransform.position.z); 	
		mainCamTransform.position = Vector3.Lerp (mainCamTransform.position, wantedPosition, Time.deltaTime * 5.0f); //плавно сдвигает камеру. В нашем случае по X 
	}
}
