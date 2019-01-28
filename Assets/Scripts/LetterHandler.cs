using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Audio;
using InControl;

public class LetterHandler : MonoBehaviour {

    public AudioSource a;

    public RectTransform letter;
    public Image i;

    PlayerCharacterController pCC;

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(ShowLetter());
        pCC = GameObject.FindObjectOfType<PlayerCharacterController>();
        pCC.movementInputEnabled = false;
    }
	
    IEnumerator ShowLetter()
    {
        yield return new WaitForSeconds(1f);
        letter.DOMoveY(550, 3f, true);
        a.Play();
        yield return new WaitUntil(() => (InputManager.ActiveDevice.Action1.WasPressed == true) || Input.GetKeyDown(KeyCode.Space));
        letter.DOMoveY(-2000, 2f, true);
        yield return new WaitForSeconds(1f);
        i.DOFade(0, 3);
        yield return new WaitForSeconds(1f);
        pCC.movementInputEnabled = true;
    }
}
