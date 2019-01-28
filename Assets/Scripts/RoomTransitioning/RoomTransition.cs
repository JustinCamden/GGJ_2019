using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour {

	public Color normalColor;
	public Color transparentColor;
	public MeshRenderer[] stuffToMakeTransparent;
	public Color[] originalColors;

	public Vector3 cameraPos;
	public Vector3 balconyPos;

	// Use this for initialization
	void Start () {
		normalColor = new Color(1f,1f,1f,1f);
		transparentColor = new Color(1f,1f,1f,0f);

		for(int i=0;i<stuffToMakeTransparent.Length;i++){
			originalColors[i] = stuffToMakeTransparent[i].material.color;
			stuffToMakeTransparent[i].material.color = new Color(originalColors[i].r,originalColors[i].g,originalColors[i].b,0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Room"){
			RoomInfo rinfo = other.gameObject.GetComponent<RoomInfo>();
			StartCoroutine(lightChange(normalColor,transparentColor));
			StartCoroutine(cameraChange(Camera.main.transform.position,cameraPos));
			//for(int i=0;i<rinfo.stuffToDeactivate.Length;i++){
			//	rinfo.stuffToDeactivate[i].SetActive(false);
			//}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Room"){
			RoomInfo rinfo = other.gameObject.GetComponent<RoomInfo>();
			StartCoroutine(lightChange(transparentColor,normalColor));
			StartCoroutine(cameraChange(cameraPos,balconyPos));
			//for(int i=0;i<rinfo.stuffToDeactivate.Length;i++){
			//	rinfo.stuffToDeactivate[i].SetActive(false);
			//}
		}
	}

	IEnumerator lightChange(Color startColor, Color targetColor){
		float elapsedTime = 0f;
		float targTime = 1.2f;

		//float startIntensity = lightSource.intensity;
		while(elapsedTime < targTime){
			elapsedTime += Time.deltaTime;
			for(int x=0;x<stuffToMakeTransparent.Length;x++){
				startColor = new Color(originalColors[x].r,originalColors[x].g,originalColors[x].b,startColor.a);
				targetColor = new Color(originalColors[x].r,originalColors[x].g,originalColors[x].b,targetColor.a);
				stuffToMakeTransparent[x].material.color = Color.Lerp(startColor, targetColor, (elapsedTime/targTime));
			}

			yield return null;
		}
	}

	IEnumerator cameraChange(Vector3 startPos, Vector3 endPos){
		float elapsedTime = 0f;
		float targTime = 2f;
		//float startIntensity = lightSource.intensity;
		while(elapsedTime < targTime){
			elapsedTime += Time.deltaTime;
			//for(int x=0;x<stuffToMakeTransparent.Length;x++){
			//stuffToMakeTransparent[x].material.color = Color.Lerp(startColor, targetColor, (elapsedTime/targTime));
			//}

			Camera.main.transform.position = Vector3.Lerp(startPos,endPos,(elapsedTime/targTime));

			yield return null;
		}
	}

}
