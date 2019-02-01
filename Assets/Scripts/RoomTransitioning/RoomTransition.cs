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
	public Vector3 activePos;
	Vector3 startPos;

	public bool movingCamera = false;
	float eTime = 0f;
	float maxTime = 1f;

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
		if(Vector3.Distance(Camera.main.transform.position,activePos) > 0.01f){
			if (eTime < maxTime){
				eTime += Time.deltaTime;
				Camera.main.transform.position = Vector3.Lerp(startPos,activePos,(eTime/maxTime));
			}
		}
	}

	void OnTriggerEnter(Collider other){
		if(other.tag == "Room"){
			RoomInfo rinfo = other.gameObject.GetComponent<RoomInfo>();
			StartCoroutine(lightChange(normalColor,transparentColor));
			cameraChange(cameraPos);
			//for(int i=0;i<rinfo.stuffToDeactivate.Length;i++){
			//	rinfo.stuffToDeactivate[i].SetActive(false);
			//}
		}
	}

	void OnTriggerExit(Collider other){
		if(other.tag == "Room"){
			RoomInfo rinfo = other.gameObject.GetComponent<RoomInfo>();
			StartCoroutine(lightChange(transparentColor,normalColor));
			cameraChange(balconyPos);
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

	void cameraChange(Vector3 endPos){
		eTime = 0f;
		maxTime = 1f;
		startPos = Camera.main.transform.position;
		activePos = endPos;
		//float startIntensity = lightSource.intensity;
		
	}

}
