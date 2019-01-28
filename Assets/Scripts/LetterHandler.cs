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

	// Use this for initialization
	void Start () 
    {
        StartCoroutine(ShowLetter());
	}
	
    IEnumerator ShowLetter()
    {
        yield return new WaitForSeconds(1f);
        letter.DOMoveY(550, 3f, true);
        yield return new WaitForSeconds(1f);
        a.Play();
        yield return new WaitForSeconds(5f);
        //yield return StartCoroutine(WaitForKeyDown(KeyCode.Space));
        letter.DOMoveY(-2000, 2f, true);
        yield return new WaitForSeconds(1f);
        i.DOFade(0, 3);
    }

    IEnumerator WaitForKeyDown(KeyCode keyCode)
    {
        while (!Input.GetKeyDown(keyCode) || !InputManager.ActiveDevice.Action1)
            yield return null;
        yield return new WaitForFixedUpdate();
    }
}
