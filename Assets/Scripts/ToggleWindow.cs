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
    public Animator anim;

 
    public override void Interact()
    {
        //check if window is enabled, if it isn't enable it, if it is disable it.
        {
            // blind strech goal (if (blindIsOpen == true)){} 
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (windowIsOpen == false)
                {
                    windowIsOpen = true;
                    //for simple animations, like for the window (this is an
                    //untrue example), but if we ONLY wanted to open the window
                    //this one below will work fine for doing one thing
                    //as long as the animation plays and goes to an end state
                    // - ex/
                    //gameObject.GetComponent<Animator>().enabled = true;

                    //if we want to do multiple animations - like closing the 
                    //window after we open it, just add bools to trigger appropriate
                    //connections, in the animation tree - ex/
                    //anim.SetBool("isOpeningWindowFromScript", true);

                }
                else
                {
                    windowIsOpen = false;
                    //so again, we want to close the window, so set the 
                    //connection from the animation tree to trigger appropriate
                    //animation - ex/
                    //anim.SetBool("isClosingWindowFromScript", true);
                }
            }
            //else{blindIsOpen = true);}
        }
    }


        // Use this for initialization
        void Start()
        {
            window_1 = GetComponent<GameObject>();
        anim = gameObject.GetComponent<Animator>();
    }

        // Update is called once per frame
        void Update()
        {
            
        }

    }
