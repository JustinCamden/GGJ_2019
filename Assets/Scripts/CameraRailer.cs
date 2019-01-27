using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRailer : MonoBehaviour
{
    public static CameraRailer cr;
    private Transform camTrans;
    private Camera c;
    public float duration;
    public float elapsed;
    private bool transition;
    private Transform targetTrans;
    private float targetSize;

    public Transform overWorldTrans;
    public float overWorldSize;

    public Transform posterMiniTrans;
    public float posterMiniSize;

    public Transform figureMiniTrans;
    public float figureMiniSize;

	// Use this for initialization
	void Start () 
    {
        camTrans = GetComponent<Transform>();
        c = Camera.main;
        cr = this;
    }
	
	// Update is called once per frame
	void Update () 
    {
	    if(Input.GetKeyUp(KeyCode.C))
        {
            MoveCamera(posterMiniTrans, posterMiniSize);
            PosterGame.pg.PosterMinigame();
        }

        if(Input.GetKeyUp(KeyCode.R))
        {
            MoveCamera(overWorldTrans, overWorldSize);
        }

        if(Input.GetKeyUp(KeyCode.F))
        {
            MoveCamera(figureMiniTrans, figureMiniSize);
        }

        if (transition)
        {
            elapsed += Time.deltaTime / duration;

            camTrans.position = Vector3.Lerp(camTrans.position, targetTrans.position, elapsed);
            camTrans.rotation = Quaternion.Lerp(camTrans.rotation, targetTrans.rotation, elapsed);
            c.orthographicSize = Mathf.SmoothStep(c.orthographicSize, targetSize, elapsed);

            if (elapsed > 0.3f)
            {
                transition = false;
                elapsed = 0f;
            }
        }
    }


    public void MoveCamera(Transform t, float s)
    {
        targetTrans = t;
        targetSize = s;

        transition = true;
    }
}
