using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : BaseInteraction {

	public List<Material> paints;
	public List<GameObject> objectsToPaint;
	private int currentMat = 0;

	public override void Interact()
	{
		if (currentMat >= paints.Count)
		{
			currentMat = 0;
		}

		foreach (GameObject obj in objectsToPaint)
		{
			obj.GetComponent<MeshRenderer>().material = paints[currentMat];
		}

		currentMat++;
	}
}
