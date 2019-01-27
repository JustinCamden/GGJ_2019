using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class EnableDisable : BaseInteraction {
	public GameObject[] toEnable;
	public GameObject[] toDisable;

	public override void Interact()
	{
		foreach (GameObject obj in toEnable)
		{
			obj.SetActive(true);
		}
		
		foreach (GameObject obj in toDisable)
		{
			obj.SetActive(false);
		}

	}
}
