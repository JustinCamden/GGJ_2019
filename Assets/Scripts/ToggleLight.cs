using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLight : BaseInteraction
{
    //public GameObject highlightedObject;
    //public bool interactable;
    public Light light_1;

    public override void Interact()
    {
        //check if light is enabled, if it isn't enable it, if it is disable it.
//        {
            //if (Input.GetKeyDown(KeyCode.Space))
//            {
                light_1.enabled = !light_1.enabled;
//            }
//        }
    }


    // Use this for initialization
    void Start()
    {
        light_1 = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {

    }

}
