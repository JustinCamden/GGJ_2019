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

	[Tooltip("whether or not this interaction should play a sound!")]
	public bool soundShouldPlay;
	
	[Tooltip("whether or not this interaction should cause the screen to blackout and fade back in")]
	public bool shouldFadeInOut;

	[Tooltip("does this interaction advance the game?")]
	public bool isKeyInteraction;
	
	public List<DialogueRunner.ConversationWindow> conversationWindows;
	
	
	private InputDevice controller;
	private  AudioSource source;

    public void Awake()
    {
        if (interactSprite)
        {
            interactSprite.SetActive(false);
        }
        if (soundShouldPlay)
        {
            source = GetComponent<AudioSource>();
        }
    }

	public void TryInteract()
	{
		// checks to see if it should Start a Minigame, Start a Dialogue, Darken the screen, or just Interact. 
		if (interactEnabled)
		{
	

			if (shouldStartMinigame)
			{
				StartMinigame();
				if (soundShouldPlay)
				{
					source.Play();
				}
			}
			else if (shouldRunDialogue)
			{
				DialogueStart();
				interactEnabled = false;
			}
			else if (shouldFadeInOut)
			{
				ScreenDarkener.Instance.Darken(this);
				interactEnabled = false;
			}
			else
			{
				Interact();
				if (soundShouldPlay)
				{
					source.Play();
				}
			}

			if (isKeyInteraction)
			{
				NPCArrival.Instance.InteractionFinished();
			}
		}
	}
	
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
		if (!interactEnabled)
		{
			interactSprite.SetActive(false);
		}
	}

    public void OnSelected()
    {
	    if (interactEnabled)
	    {
		    interactSprite.SetActive(true);
	    }
    }

    public void OnDeselected()
    {
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
			if (soundShouldPlay)
			{
				source.Play();
			}
		}
		else if (shouldFadeInOut)
		{
			if (soundShouldPlay)
			{
				source.Play();
			}
			ScreenDarkener.Instance.Darken(this);
			interactEnabled = false;
		}
		else
		{
			Interact();
			if (soundShouldPlay)
			{
				source.Play();
			}
		}
		
	}
}
