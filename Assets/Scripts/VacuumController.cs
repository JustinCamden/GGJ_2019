using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

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
			//collect all of the dirt
			vacPos += new Vector3(HorizontalInput()*speed*Time.deltaTime,VerticalInput()*speed*Time.deltaTime,0f);
			GetComponent<Rigidbody2D>().MovePosition(vacPos);
		} else{
			//end vacuum game
			//you got them all
			if(GameObject.FindWithTag("VacuumManager")){
				//There is a manager in the scene
				GameObject[] removeItems;
				removeItems = GameObject.FindWithTag("VacuumManager").GetComponent<VacuumGameManager>().itemsToRemove;
				for(int i=0;i<removeItems.Length;i++){
					Destroy(removeItems[i]);
				}
				GameObject.FindWithTag("VacuumManager").GetComponent<VacuumGameManager>().EndMinigame();
			}
		}
	}

	float HorizontalInput(){
		if(Input.GetKey(KeyCode.A) || InputManager.ActiveDevice.LeftStickLeft.IsPressed){
			return -1f;
		} else if(Input.GetKey(KeyCode.D) || InputManager.ActiveDevice.LeftStickRight.IsPressed){
			return 1f;
		} else{
			return 0f;
		}
	}

	float VerticalInput(){
		if(Input.GetKey(KeyCode.W) || InputManager.ActiveDevice.LeftStickUp.IsPressed){
			return 1f;
		} else if(Input.GetKey(KeyCode.S) || InputManager.ActiveDevice.LeftStickDown.IsPressed){
			return -1f;
		} else{
			return 0f;
		}
	}
}
