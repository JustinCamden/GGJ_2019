using System.Collections;
using System.Collections.Generic;
using InControl;
using UnityEngine;

public class DialogueRunner : MonoBehaviour {
	#region SINGLETON
	private static DialogueRunner _instance;
	public static DialogueRunner Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = GameObject.FindObjectOfType<DialogueRunner>();

				if (_instance == null)
				{
					GameObject container = new GameObject("DialogueRunner");
					_instance = container.AddComponent<DialogueRunner>();
				}
			}

			return _instance;
		}
	}
	#endregion

	public GameObject UIWindow;
	public SuperTextMesh textMesh;
	public int currentWindow = 0;
	public InputDevice controller;
	public List<ConversationWindow> currentWindows;
	
	void Update()
	{
		controller = InputManager.ActiveDevice;
	}

	private void Awake()
	{
		UIWindow.SetActive(false);
	}

	[System.Serializable] public struct ConversationWindow
	{
		public string text;
		public GameObject windowTarget;
		public BaseInteraction interaction; 
	}

	public void RunConversation()
	{
		StartCoroutine(RunLine(currentWindows[currentWindow]));
	}

	private IEnumerator RunLine(ConversationWindow window)
	{
		if (window.windowTarget != null)
		{
			UIWindow.transform.position = window.windowTarget.transform.position;
		}
		else
		{
			UIWindow.transform.position = transform.position;
		}

		Debug.Log("crasho loopo");
		
		textMesh.text = window.text;
		textMesh.Rebuild();
		
		while (!Input.GetKeyDown(KeyCode.Space) || textMesh.reading)
		{
			if (!textMesh.reading)
			{
				if (Input.GetKeyDown(KeyCode.Space) || controller.Action1)
				{
					Debug.Log("crasho post loopo");
					break;
				}
			}

			yield return null;
		}

		Debug.Log("crasho supo past loopo");
		


		if (window.interaction != null)
		{
			textMesh.text = "";
			textMesh.Rebuild();
			UIWindow.SetActive(false);
			window.interaction.DialogueInteract();
			yield break;
		}
		else
		{
			textMesh.text = "";
			textMesh.Rebuild();
			currentWindow++;
			RunConversation();
			yield break;
		}
	}
}
