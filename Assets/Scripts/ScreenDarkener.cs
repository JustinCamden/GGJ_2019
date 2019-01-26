using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDarkener : MonoBehaviour
{
	#region SINGLETON
	private static ScreenDarkener _instance;
	public static ScreenDarkener Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<ScreenDarkener>();

				if (_instance == null)
				{
					GameObject container = new GameObject("ScreenDarkener");
					_instance = container.AddComponent<ScreenDarkener>();
				}
			}

			return _instance;
		}
	}
	#endregion
	
	private SpriteRenderer sprite;
	public float DarkenSpeed;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Darken(BaseInteraction interaction)
	{
		StartCoroutine(darkenRoutine(interaction));
	}

	public IEnumerator darkenRoutine(BaseInteraction interaction)
	{
		sprite = GetComponent<SpriteRenderer>();		
		
		while (sprite.color.a < 1)
		{
			yield return new WaitForSeconds(DarkenSpeed);
//			if (GetComponent<SpriteRenderer>().color.a <= darkVal) {
//				GetComponent<SpriteRenderer>().color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, (GetComponent<SpriteRenderer>().color.a + fadeSpeed));
//			}
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + 0.02f);
		}
		
		interaction.Interact();
		Debug.Log("interacting");

		while (sprite.color.a > 0)
		{
			yield return new WaitForSeconds(DarkenSpeed);
			sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - 0.02f);
		}
	}
}
