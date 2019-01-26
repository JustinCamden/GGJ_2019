using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : BaseInteraction
{
	private AudioSource radioSource;
	private int currentSong = 0;
	public List<AudioClip> songs;

	void Start()
	{
		
	}
	
	public override void Interact()
	{
		radioSource.clip = songs[currentSong];
		radioSource.Play();
		
		if (currentSong == songs.Count)
		{
			currentSong = 0;
		}
		else
		{
			currentSong++;
		}
	}
}
