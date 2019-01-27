using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour {

	public GameObject activeRoom;
	public GameObject mainRoom;
	public GameObject[] allRooms;
	public GameObject[] nonMainRooms;

	Color roomDark;
	Color roomLight;
	Color roomTransparent;

	// Use this for initialization
	void Start () {
		roomDark = new Color(1f,1f,1f,0.25f);
		roomLight = new Color(1f,1f,1f,0.1f);
		roomTransparent = new Color(1f,1f,1f,0f);

		ChangeRooms();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Room"){
			activeRoom = other.gameObject;
			ChangeRooms();
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Room"){
			if(other.gameObject == mainRoom){
				//nonMainRooms.SetActive(true);
				for(int x=0;x<nonMainRooms.Length;x++){
						StartCoroutine(changeRoomLighting(nonMainRooms[x].GetComponent<MeshRenderer>(),nonMainRooms[x].GetComponent<MeshRenderer>().material.color,roomDark,3f));;
					}
			}
		}
	}

	void ChangeRooms(){
		for(int i=0;i<allRooms.Length;i++){
			MeshRenderer rends = allRooms[i].GetComponent<MeshRenderer>();
			if(!rends.enabled){
					for(int x=0;x<nonMainRooms.Length;x++){
						nonMainRooms[x].GetComponent<MeshRenderer>().enabled = true;
						rends.material.color = new Color(1f,1f,1f,0f);
					}
			}
			if(allRooms[i] != activeRoom){
				//darken the room
				if(activeRoom != mainRoom){
					StartCoroutine(changeRoomLighting(rends,rends.material.color,roomDark,3f));
				}
			} else{
				//brighten the room
				StartCoroutine(changeRoomLighting(rends,rends.material.color,roomLight,3f));
				if(activeRoom == mainRoom){
					//turn off the non-main rooms
					//nonMainRooms.SetActive(false);
					for(int x=0;x<nonMainRooms.Length;x++){
						StartCoroutine(changeRoomLighting(nonMainRooms[x].GetComponent<MeshRenderer>(),nonMainRooms[x].GetComponent<MeshRenderer>().material.color,roomTransparent,3f));;
					}

				}
			}
		}
	}

	private IEnumerator changeRoomLighting(MeshRenderer targ, Color startCol, Color endCol,float timeDelay){
		float elapsedTime = 0f;
		//targ.enabled = true;
		while(elapsedTime < timeDelay){
			targ.material.color = Color.Lerp(startCol,endCol,elapsedTime/timeDelay);
			elapsedTime += Time.deltaTime;
			if(endCol == roomTransparent){
				if(elapsedTime > 2.99f){
					targ.enabled = false;
				}
			}
			yield return null;
		}
	}
}
