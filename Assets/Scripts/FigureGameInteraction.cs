using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class FigureGameInteraction : BaseInteraction
{
    public FigureHandler fH;
    public GameObject boxToDisable;
    private PlayerCharacterController pCC;
    
    public override void Interact()
    {
        fH.FigureStart();
        boxToDisable.SetActive(false);
        CameraRailer.cr.MoveCamera(CameraRailer.cr.figureMiniTrans, CameraRailer.cr.figureMiniSize);
        pCC = GameObject.FindObjectOfType<PlayerCharacterController>();
        pCC.movementInputEnabled = false;
    }
}
