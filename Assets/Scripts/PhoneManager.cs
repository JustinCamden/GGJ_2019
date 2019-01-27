using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;
using InControl;

public class PhoneManager : MonoBehaviour {

	bool active = false;

	//This stuff will get taken out i hope and replaced with incontrol stuff
	public KeyCode primaryButton;
	public KeyCode secondaryButton;

	//You instantiate this alert whenever you receive a new text message
	public GameObject messageAlert;

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
	//Primary Buttons: the four buttons on the primary phone screen
	public Button[] primaryButtons;
	//Exit Buttons: the two buttons you can choose from for exiting the game
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
			if(primaryPhoneScreen.activeInHierarchy){
				if(InputManager.ActiveDevice.Action1.WasPressed || Input.GetKeyDown(primaryButton)){
					switch(mainIndex){
						case(0): //messages
							OpenMessages();
						break;
						case(1): //notes
							OpenNotes();
						break;
						case(2): //resume
							ResumeGame();
						break;
						case(3): //exit
							OpenExitPrompt();
						break;
					}
				}
				//let the player change which option they have selected.
				if(Input.GetKeyDown(KeyCode.W) || InputManager.ActiveDevice.LeftStickUp.WasPressed){
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
				} else if(Input.GetKeyDown(KeyCode.S) || InputManager.ActiveDevice.LeftStickDown.WasPressed){
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



			} else if(secondaryNotes.activeInHierarchy){
				if(Input.GetKeyDown(KeyCode.A) || InputManager.ActiveDevice.LeftStickLeft.WasPressed){
					noteIndex -= 1;
					if(noteIndex < 0){
						noteIndex = noteList.Count - 1;
					}
				} else if(Input.GetKeyDown(KeyCode.D) || InputManager.ActiveDevice.LeftStickRight.WasPressed){
					noteIndex += 1;
					if(noteIndex >= noteList.Count){
						noteIndex = 0;
					}
				}


				//Display the currently-indexed note
				noteTitleField.text = noteList[noteIndex].noteTitle;
				noteContentField.text = noteList[noteIndex].noteContent;


			} else if(secondaryMessages.activeInHierarchy){

				
				//instantiate all text messages
				for(int i=0;i<messageList.Count;i++){
					if(!GameObject.Find(messageList[i].name)){
						GameObject newMsg = Instantiate(messageList[i],transform.position,Quaternion.identity);
							newMsg.name = messageList[i].name;
							newMsg.transform.localScale = new Vector3(1f,1f,1f);
							newMsg.transform.SetParent(contentTarget.transform,false);
					}
				}

			} else if(secondaryExit.activeInHierarchy){
				//Player is trying to exit
				if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || InputManager.ActiveDevice.LeftStickUp.WasPressed || InputManager.ActiveDevice.LeftStickDown.WasPressed){
					//Change between "confirm" and "cancel"
					exitIndex = !exitIndex;
					if(exitIndex){
						exitButtons[0].interactable = false;
						exitButtons[1].interactable = true;
						} else{
						exitButtons[0].interactable = true;
						exitButtons[1].interactable = false;
						}
				}
				//Actually press "Space" or whatever primary button
				if(Input.GetKeyDown(primaryButton) || InputManager.ActiveDevice.Action1.WasPressed){
					if(!exitIndex){
							//confirm exit
							ExitGame();
					} else{
							//cancel exit
							CancelExitGame();
					}
				}

			}



			if(Input.GetKeyDown(secondaryButton) || InputManager.ActiveDevice.Command.WasPressed){
				//If you hit escape:
				
				if(secondaryExit.activeInHierarchy || secondaryNotes.activeInHierarchy || secondaryMessages.activeInHierarchy){
					//Phone has already been deactivated, but you want to go back to the start screen, so reactivate it.
					ActivatePhone();
				} else{
					DeactivatePhone();
				}
			}
		} else{
			if(Input.GetKeyDown(secondaryButton) || InputManager.ActiveDevice.Command.WasPressed){
				//If the phone is not already active and you hit escape, open the phone
				ActivatePhone();
			}
		}	


	}

	//public voids to use for actual Unity Buttons
	public void OpenMessages(){
		secondaryMessages.SetActive(true);
		primaryPhoneScreen.SetActive(false);
	}

	public void OpenNotes(){
		secondaryNotes.SetActive(true);
		primaryPhoneScreen.SetActive(false);
	}

	public void ResumeGame(){
		DeactivatePhone();
	}

	public void OpenExitPrompt(){
		secondaryExit.SetActive(true);
		primaryPhoneScreen.SetActive(false);
	}

	public void CancelExitGame(){
		ActivatePhone();
	}

	public void ExitGame(){
		Application.Quit();
	}

	//When you close the pause menu, turn everything off!!
	void DeactivatePhone(){
		active = false;
		//this stuff all needs a time delay
		primaryPhoneScreen.SetActive(false);
		secondaryExit.SetActive(false);
		secondaryNotes.SetActive(false);
		secondaryMessages.SetActive(false);
	}

	//When you open the pause menu, make sure to set certain things to true and to reset any indices
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

	//Call this from anywhere to add in a newMessage Prefab
	//Sorry to keep these as prefabs, could redo as something more robust. more similar to the notes system
	public void AddMessage(GameObject newMessage){
		if(messageList.Count > 2){
			messageList.RemoveAt(0);
		}
		messageList.Add(newMessage);
		Instantiate(messageAlert,transform.position,Quaternion.identity);
	}

	//Call this from anywhere to add notes to the player's phone! They will be kept in the order that the player 'unlocks' them
	public void AddNotes(string newNoteTitle,string newNoteContent){
		notesEntry newNote = new notesEntry();
		newNote.noteTitle = newNoteTitle;
		newNote.noteContent = newNoteContent;
		noteList.Add(newNote);
	}
}


[System.Serializable]
public class notesEntry{
	public string noteTitle;
	public string noteContent;
}
