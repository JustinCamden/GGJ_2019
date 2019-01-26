using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureDetector : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Detector")
            FigureHandler.fh.isBlocked = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Detector")
            FigureHandler.fh.isBlocked = false;
    }
}
