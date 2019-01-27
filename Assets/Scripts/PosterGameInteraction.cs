using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PosterGameInteraction : BaseInteraction
{
    public PosterGame pG;
    
    public override void Interact()
    {
        pG.PosterMinigame();
    }
}
