using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour {
	public float intensity = 0.0015f;
	float startIntensity;
	float startRange;
	public float val; 
	public float speed = 0.5f;
	public Light affectedLight;
	public bool flickerRun = true;
	public float flickerInterval = 0.1f;
	public float flickerIntervalMin = 0.1f;
	public float flickerIntervalMax = 0.5f;
	public float flickerTime = 0.1f;
	public float flickerSpeed = 0.1f;
	public bool sin = true;
	public bool flick = false;

	public IEnumerator flicker ()
	{
		while (flickerRun) {
			yield return new WaitForSeconds (flickerSpeed);

			if (flick) {
				affectedLight.intensity += Random.Range (-.4f, .4f);
				affectedLight.range += Random.Range (-.4f, .4f);
			}
		}
	}

	public IEnumerator setRun ()
	{
		while (flickerRun) {
			flick = false;
			flickerInterval = Random.Range(flickerIntervalMin, flickerIntervalMax);
			yield return new WaitForSeconds (flickerInterval);

			flick = true;

			yield return new WaitForSeconds (flickerTime);
		}
	}

	// Use this for initialization
	void Start ()
	{
		if (affectedLight == null) {
			affectedLight = GetComponent<Light>();
		}
		startIntensity = affectedLight.intensity;
		startRange = affectedLight.range;
		StartCoroutine(flicker());
		StartCoroutine(setRun());
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (sin) {
			val = Mathf.Sin (Time.time * speed);
			affectedLight.intensity = (affectedLight.intensity + (val * intensity));
			affectedLight.range = (affectedLight.range + (val * intensity / 3));
		}

		if (affectedLight.intensity >= (startIntensity + startIntensity/3) || affectedLight.range >= (startRange + startRange/2) 
			|| affectedLight.intensity <= (startIntensity - startIntensity/3)  || affectedLight.range <= (startRange - startRange/2) ) {
			affectedLight.intensity = startIntensity;
			affectedLight.range = startRange;
		}
	}
}
