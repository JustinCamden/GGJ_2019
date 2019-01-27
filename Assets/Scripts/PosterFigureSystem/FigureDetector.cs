using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureDetector : MonoBehaviour {
    //This script makes sure that players can't have figures overlapping on the shelves.


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
