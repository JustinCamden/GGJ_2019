using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumController : MonoBehaviour {

	Vector3 vacPos;
	float speed = 5f;

	// Use this for initialization
	void Start () {
		vacPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(GameObject.FindWithTag("Dirt")){
			//game is ongoing
			vacPos += new Vector3(Input.GetAxis("Horizontal")*speed*Time.deltaTime,Input.GetAxis("Vertical")*speed*Time.deltaTime,0f);
			GetComponent<Rigidbody2D>().MovePosition(vacPos);
		} else{
			//end vacuum game
			//you got them all
			
		}
	}
}
