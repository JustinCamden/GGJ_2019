using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class BaseInteraction : MonoBehaviour {

	public GameObject interactSprite;
	[Tooltip("Whether or not this interaction starts a minigame")]
	public bool shouldStartMinigame;
	[Tooltip("Whether or not this interaction has associated dialogue")]
	public bool shouldRunDialogue;
	[Tooltip("used for seeing if the player should be able to hit space and interact")]
	public bool interactEnabled = true;

	public List<DialogueRunner.ConversationWindow> conversationWindows;
	
	
	private InputDevice controller;
	
	public virtual void Interact () {
		// do stuff here!
		// turn lights on/off, play sound, trigger animations
		// enable/disable GameObjects
	}

	public virtual void StartMinigame () {
		// start a minigame here 
	}

	void Update()
	{
		//grab controller
		controller = InputManager.ActiveDevice;
	}
	
	private void OnTriggerEnter(Collider other)
    {
		// once player enters, set sprite on 
        interactSprite.SetActive(true);
    }

	private void OnTriggerStay (Collider other)
	{
		// waits for user input 
		if (interactEnabled)
		{
			if (!interactSprite.activeInHierarchy)
			{
				interactSprite.SetActive(true);
			}

			if (Input.GetKeyDown(KeyCode.Space))
			{
				// checks to see if it should Start a Minigame, Start a Dialogue, or just Interact. 
				if (shouldStartMinigame)
				{
					StartMinigame();
				}
				else if (shouldRunDialogue)
				{
					DialogueStart();
					interactEnabled = false;
				}
				else
				{
					Interact();
				}
			}
		}
		else
		{
			if (interactSprite.activeInHierarchy)
			{
				interactSprite.SetActive(false);
			}
		}
		

	}

    private void OnTriggerExit(Collider other)
    {
        // once exits sets arrow to false 
        interactSprite.SetActive(false);
    }

	public void DialogueStart()
	{
		DialogueRunner.Instance.UIWindow.SetActive(true);
		DialogueRunner.Instance.currentWindows = conversationWindows;
		DialogueRunner.Instance.RunConversation();
		DialogueRunner.Instance.currentWindow = 0;

	}

	public void DialogueInteract()
	{
		if (shouldStartMinigame)
		{
			StartMinigame();
		}
		else
		{
			Interact();
		}
	}
}
