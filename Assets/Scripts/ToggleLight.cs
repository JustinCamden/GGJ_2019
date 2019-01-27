using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : BaseInteraction
{
    //public GameObject highlightedObject;
    //public bool interactable;
    public Light light_1;
    public bool candle;
    public GameObject en;
    
    public override void Interact()
    {
        //check if light is enabled, if it isn't enable it, if it is disable it.
//        {
            //if (Input.GetKeyDown(KeyCode.Space))
//            {
                light_1.enabled = !light_1.enabled;
        if (candle)
        {
            if (en.activeSelf)
            {
                en.SetActive(false);
            }
            else
            {
                en.SetActive(true);
            }
        }
//            }
//        }
    }


    // Use this for initialization
    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        interactEnabled = true;
    }

}
