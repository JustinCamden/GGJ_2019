using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMessageEvents : MonoBehaviour {

	float eTime = 0f;
	public newTextMessage[] textMessages;
	public PhoneManager phone;

	// Use this for initialization
	void Start () {
		phone = GameObject.FindWithTag("Phone").GetComponent<PhoneManager>();
	}
	
	// Update is called once per frame
	void Update () {
		eTime += Time.deltaTime;
		for(int i=0;i<textMessages.Length;i++){
			if(eTime > textMessages[i].timestamp){
				//send the message!
				if(!textMessages[i].activated){
					phone.AddMessage(textMessages[i].obj);
					textMessages[i].activated = true;
				}
			}
		}
	}
}

[System.Serializable]
public class newTextMessage{
	public GameObject obj;
	public float timestamp;
	public bool activated = false;
}
