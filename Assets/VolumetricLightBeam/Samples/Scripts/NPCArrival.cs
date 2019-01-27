using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCArrival : MonoBehaviour
{
	#region SINGLETON
	private static NPCArrival _instance;
	public static NPCArrival Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<NPCArrival>();

				if (_instance == null)
				{
					GameObject container = new GameObject("NPCArrival");
					_instance = container.AddComponent<NPCArrival>();
				}
			}

			return _instance;
		}
	}
	#endregion
	public GameObject birbToEnable;
	public AudioSource birbPeckingSound;
	public GameObject ghostToEnable;
	public AudioSource ghostSound;
	public GameObject friendsToEnable;
	public AudioSource carSounds;
	public int triggeredInteractions;

	public int birbThreshold;
	public int ghostThreshold;
	public int friendsThreshold ;

	private bool birbTriggered = false;
	private bool ghostTriggered = false;
	private bool friendsTriggered = false; 
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InteractionFinished()
	{
		StartCoroutine(finishedTrigger());
	}

	public IEnumerator finishedTrigger ()
	{
		yield return new WaitForSeconds(3f);
		
		triggeredInteractions++;
		
		if (triggeredInteractions == birbThreshold && !birbTriggered)
		{
			birbToEnable.SetActive(true);
			birbPeckingSound.Play();
			birbTriggered = true;
		}
		else if (triggeredInteractions == ghostThreshold && !ghostTriggered)
		{
			ghostToEnable.SetActive(true);
			ghostSound.Play();
			ghostTriggered = true; 
		}
		else if (triggeredInteractions == friendsThreshold && !friendsTriggered)
		{
			friendsToEnable.SetActive(true);
			carSounds.Play();
			friendsTriggered = true; 
		}
	}
}
