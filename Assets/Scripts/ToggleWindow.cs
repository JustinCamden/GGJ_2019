using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleWindow : BaseInteraction
{
    //public GameObject highlightedObject;
    //public bool interactable;


    //public GameObject blinds; - stretch goal 
    public GameObject window_1;
    public bool windowIsOpen = false;

    //public bool windowOpenAnimation = false; //check if window is open or closed
    //via which animation played last for the window (probably overcomplicating this)

    public override void Interact()
    {
        //check if window is enabled, if it isn't enable it, if it is disable it.
        {
            // blind strech goal (if (blindIsOpen == true)){} 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (windowIsOpen == true)
                {
                    windowIsOpen = false;
                    //play animation script (if it works like this)
                }
                else
                {
                    windowIsOpen = true;
                    //play animation script 
                }
            }
            //else{blindIsOpen = true);}
        }
    }


        // Use this for initialization
        void Start()
        {
            window_1 = GetComponent<GameObject>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }