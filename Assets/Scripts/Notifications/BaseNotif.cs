using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BaseNotif : MonoBehaviour {

	Vector3 notifPos;

	Color fullColor;
	Color fullTransparent;

	float disappearTimer = 0f;
	float disappearLimit = 3f;

	// Use this for initialization
	void Start () {

		//attach to the notificationUI
		if(GameObject.Find("NotificationUI")){
			transform.SetParent(GameObject.Find("NotificationUI").transform);
			GetComponent<RectTransform>().anchoredPosition = new Vector3(420f,-150f,0f);
			GetComponent<RectTransform>().localScale = new Vector3(1f,1f,1f);
		}

		fullColor = new Color(1f,1f,1f,1f);
		fullTransparent = new Color(1f,1f,1f,0f);

		NotificationEnter();
	}
	
	// Update is called once per frame
	void Update () {
		if(disappearTimer < disappearLimit){
			disappearTimer += Time.deltaTime;
		} else{
			NotificationExit();
		}
	}

	public void NotificationEnter(){
		StartCoroutine(colorChanger(0.7f,fullTransparent,fullColor));
		disappearTimer = 0f;
	}

	public void NotificationExit(){
		StartCoroutine(colorChanger(0.7f,fullColor,fullTransparent));
	}

	IEnumerator colorChanger(float timer,Color startCol, Color endCol){
		float eTime = 0f;
		while(eTime < timer){
			eTime += Time.deltaTime;
			GetComponent<Image>().color = Color.Lerp(startCol,endCol,(eTime/timer));
			Text[] textChildren = GetComponentsInChildren<Text>();
			for(int x=0;x<textChildren.Length;x++){
			textChildren[x].color = new Color(0f,0f,0f,GetComponent<Image>().color.a);
			}
			yield return null;
		}
		if(eTime >= timer){
			Destroy(gameObject);
		}
	}
}
