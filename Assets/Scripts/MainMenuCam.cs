using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCam : MonoBehaviour {

    public Transform viewPoint;

    public float turnSpeed;

    void Update()
    {
        transform.LookAt(viewPoint.position);
        transform.RotateAround(viewPoint.position, Vector3.up, turnSpeed * Time.deltaTime);
    }
}