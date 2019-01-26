using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class PhoneManager : MonoBehaviour {

	bool active = false;

	//This stuff will get taken out i hope and replaced with incontrol stuff
	public KeyCode primaryButton;
	public KeyCode secondaryButton;

	//The primary phone screen is the initial 4 options
	public GameObject primaryPhoneScreen;
		//the messages screen
		public GameObject secondaryMessages;
		//the notes screen
		public GameObject secondaryNotes;
		//resume doesn't bring up any other screens
		//exit asks you to confirm
		public GameObject secondaryExit;

	//Indices
		int mainIndex = 0;
		int messageIndex = 0;
		int noteIndex = 0;
		bool exitIndex = true;

	//Content Arrays
	[Header("Place Content In Here")]
	public List<GameObject> messageList = new List<GameObject>();
	public List<notesEntry> noteList = new List<notesEntry>();

	[Header("Button Arrays")]
	public Button[] primaryButtons;
	public Button[] exitButtons;

	//Special Objects
	[Header("Notes Objects")]
	public Text noteTitleField;
	public Text noteContentField;

	//Content
	public GameObject contentTarget;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(active){
			if(primaryPhoneScreen.active){
				if(Input.GetKeyDown(primaryButton)){
					switch(mainIndex){
						case(0): //messages
							secondaryMessages.SetActive(true);
							primaryPhoneScreen.SetActive(false);
						break;
						case(1): //notes
							secondaryNotes.SetActive(true);
							primaryPhoneScreen.SetActive(false);
						break;
						case(2): //resume
							DeactivatePhone();
						break;
						case(3): //exit
							secondaryExit.SetActive(true);
							primaryPhoneScreen.SetActive(false);
						break;
					}
				}
				//let the player change which option they have selected.
				if(Input.GetKeyDown(KeyCode.W)){
					mainIndex -= 1;
					//reset main index if you need to
					if(mainIndex < 0){
						mainIndex = 3;
					}
					for(int i=0;i<4;i++){
						if(i != mainIndex){
								primaryButtons[i].interactable = false;
							} else{
								primaryButtons[i].interactable = true;
							}
					}
				} else if(Input.GetKeyDown(KeyCode.S)){
					mainIndex += 1;
					//reset main index if you need to
					if(mainIndex > 3){
						mainIndex = 0;
					}
					for(int i=0;i<4;i++){
						if(i != mainIndex){
								primaryButtons[i].interactable = false;
							} else{
								primaryButtons[i].interactable = true;
							}
					}
				}



			} else if(secondaryNotes.active){
				if(Input.GetKeyDown(KeyCode.A)){
					noteIndex -= 1;
					if(noteIndex < 0){
						noteIndex = noteList.Count - 1;
					}
				} else if(Input.GetKeyDown(KeyCode.D)){
					noteIndex += 1;
					if(noteIndex >= noteList.Count){
						noteIndex = 0;
					}
				}

				noteTitleField.text = noteList[noteIndex].noteTitle;
				noteContentField.text = noteList[noteIndex].noteContent;


			} else if(secondaryMessages.active){

				
				//instant all currently-unlocked text messages
				for(int i=0;i<messageList.Count;i++){
					if(!GameObject.Find(messageList[i].name)){
					GameObject newMsg = Instantiate(messageList[i],transform.position,Quaternion.identity);
					newMsg.name = messageList[i].name;
					newMsg.transform.localScale = new Vector3(1f,1f,1f);
					newMsg.transform.SetParent(contentTarget.transform,false);
					}
				}

			} else if(secondaryExit.active){

				if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S)){
					exitIndex = !exitIndex;
					if(exitIndex){
						exitButtons[0].interactable = false;
						exitButtons[1].interactable = true;
						} else{
						exitButtons[0].interactable = true;
						exitButtons[1].interactable = false;
						}
				}

				if(Input.GetKeyDown(primaryButton)){
					if(exitIndex){
							//confirm exit
							Application.Quit();
						} else{
							//cancel exit
							DeactivatePhone();
							ActivatePhone();
						}
				}

			}



			if(Input.GetKeyDown(secondaryButton)){
				DeactivatePhone();
			}
		} else{
			if(Input.GetKeyDown(secondaryButton)){
				ActivatePhone();
			}
		}	


	}

	void DeactivatePhone(){
		active = false;
		//this stuff all needs a time delay
		primaryPhoneScreen.SetActive(false);
		secondaryExit.SetActive(false);
		secondaryNotes.SetActive(false);
		secondaryMessages.SetActive(false);
	}

	void ActivatePhone(){
		active = true;
		mainIndex = 0;
		noteIndex = 0;
		exitIndex = false;

		exitButtons[0].interactable = true;
		exitButtons[1].interactable = false;

		primaryButtons[0].interactable = true;
		primaryButtons[1].interactable = false;
		primaryButtons[2].interactable = false;
		primaryButtons[3].interactable = false;

		primaryPhoneScreen.SetActive(true);
		secondaryExit.SetActive(false);
		secondaryNotes.SetActive(false);
		secondaryMessages.SetActive(false);
	}
}


[System.Serializable]
public class notesEntry{
	public string noteTitle;
	public string noteContent;
}
