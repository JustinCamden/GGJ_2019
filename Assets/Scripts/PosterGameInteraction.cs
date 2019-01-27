using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PosterGameInteraction : BaseInteraction
{
    public PosterGame pG;
    public GameObject boxToDisable;
    
    public override void Interact()
    {
        pG.PosterMinigame();
        boxToDisable.SetActive(false);
        CameraRailer.cr.MoveCamera(CameraRailer.cr.posterMiniTrans, CameraRailer.cr.posterMiniSize);
    }
}
