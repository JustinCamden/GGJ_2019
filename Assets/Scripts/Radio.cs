using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : BaseInteraction
{
	public AudioSource radioSource;
	private int currentSong = 0;
	public List<AudioClip> songs;
	public Animator radioAnim;
	public ParticleSystem parts;

	public override void Interact()
	{
		if (currentSong >= songs.Count)
		{
			currentSong = 0;
			radioSource.Stop();
			parts.Stop();
			radioAnim.enabled = false;

		}
		else
		{
			radioSource.Stop();
			radioSource.clip = songs[currentSong];
			radioSource.Play();
			parts.Play();
			radioAnim.enabled = true;
			currentSong++;
		}

		
	}

	void Update()
	{
		interactEnabled = true;
	}
}
