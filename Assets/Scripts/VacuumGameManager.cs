using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumGameManager : MonoBehaviour {

	[Range(0,8)] public int dirtPopulation;
	public GameObject dirtPrefab;
	public bool active;
	public GameObject vacuum;
	public Camera myCam;
	private PlayerCharacterController pCC;
	[Header("RealWorld Items")]
	public GameObject[] itemsToRemove;

	// Use this for initialization
	void Start () {
		vacuum.SetActive(active);
		if(active){
			ActivateVacuumMinigame();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ActivateVacuumMinigame(){
		active = true;
		vacuum.SetActive(active);
		if(dirtPopulation <= 0){
			dirtPopulation = Random.Range(1,8);
		}

		Vector3 topLeftScreenAnchor;
			topLeftScreenAnchor = myCam.ScreenToWorldPoint(new Vector3(0f,0f,7f));
		Vector3 bottomRightScreenAnchor;
			bottomRightScreenAnchor = myCam.ScreenToWorldPoint(new Vector3(Screen.width,Screen.height,7f));
			print(bottomRightScreenAnchor);

		for(int i=0;i<dirtPopulation;i++){
			Vector3 dirtPos = new Vector3(Random.Range(topLeftScreenAnchor.x,bottomRightScreenAnchor.x),Random.Range(topLeftScreenAnchor.y,bottomRightScreenAnchor.y),0f);

			//Make sure dirt is on the right z-value
			dirtPos = new Vector3(dirtPos.x,dirtPos.y,0f);
			Instantiate(dirtPrefab,dirtPos,Quaternion.identity);
		}
	}

	public void EndMinigame(){
		pCC = GameObject.FindObjectOfType<PlayerCharacterController>();
		pCC.movementInputEnabled = true;
		Destroy(gameObject);
	}
}
