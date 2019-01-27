using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInteraction : BaseInteraction
{

	private AudioSource source;
	
	[Tooltip("Whether or not this is a looping sound that should be activated & deactivated")]
	public bool isLooping;
	
	private bool active;
	
	void Awake()
	{
		source = GetComponent<AudioSource>();
	}
	
	public override void Interact()
	{
		if (isLooping)
		{
			
		}
		else
		{
			
		}
	}
}
