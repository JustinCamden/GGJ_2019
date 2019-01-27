using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosetHandler : BaseInteraction {

    public GameObject closet;
    bool isOpen = false;
    //Should probably add something about the ghost spawning in here?


    public override void Interact()
    {
        if (!isOpen)
        {
            closet.SetActive(true);
            isOpen = true;
        }
    }
}
