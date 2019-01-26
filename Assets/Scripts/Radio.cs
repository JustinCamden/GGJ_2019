using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : BaseInteraction
{
	public AudioSource radioSource;
	private int currentSong = 0;
	public List<AudioClip> songs;

	public override void Interact()
	{
		if (currentSong >= songs.Count)
		{
			currentSong = 0;
			radioSource.Stop();
		}
		else
		{
			radioSource.Stop();
			radioSource.clip = songs[currentSong];
			radioSource.Play();
			currentSong++;
		}
	}
}
