using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseInteraction : MonoBehaviour {

	public GameObject interactSprite;
	public bool shouldStartMinigame;

	public virtual void Interact () {
		// do stuff here!
		// turn lights on/off, play sound, trigger animations
		// enable/disable GameObjects
	}

	public virtual void StartMinigame () {
		// start a minigame here 
	}

	private void OnTriggerEnter(Collider other)
    {
		// once player enters, set sprite on 
        interactSprite.SetActive(true);
    }

	private void OnTriggerStay (Collider other)
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Interact();

			if (shouldStartMinigame)
			{
				StartMinigame();
			}
		}
	}

    private void OnTriggerExit(Collider other)
    {
        // once exits sets arrow to false 
        interactSprite.SetActive(true);
    }
}
