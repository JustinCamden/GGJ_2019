using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class VacuumGameInteraction : BaseInteraction
{
    public VacuumGameManager vM;
    public GameObject vacuumToDisable;
    public GameObject vacuumToEnable;
    public GameObject cameraToEnable;
    private PlayerCharacterController pCC;
    
    public override void Interact()
    {
        vM.ActivateVacuumMinigame();
        vacuumToDisable.SetActive(false);
//        vacuumToEnable.SetActive(true);
        cameraToEnable.SetActive(true);
        pCC = GameObject.FindObjectOfType<PlayerCharacterController>();
        pCC.movementInputEnabled = false;
    }
}
