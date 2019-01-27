using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class FigureGameInteraction : BaseInteraction
{
    public FigureHandler fH;
    public GameObject boxToDisable;
    
    public override void Interact()
    {
        fH.FigureStart();
        boxToDisable.SetActive(false);
        CameraRailer.cr.MoveCamera(CameraRailer.cr.figureMiniTrans, CameraRailer.cr.figureMiniSize);
    }
}
