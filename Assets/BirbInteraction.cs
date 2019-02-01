using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbInteraction : BaseInteraction 
{

	public AudioSource birbPeckAudio;
//public GameObject highlightedObject;
    //public bool interactable;
    //public GameObject blinds; - stretch goal 
    public GameObject window_1;
    public bool windowIsOpen = false;
    public bool shouldPlayBeachLoop;
    public bool thirdInt;
    public AudioSource beachSource;
    public AudioSource birdSource;
    public Animator anim;
    public Animator interactionAnim;
    public GameObject[] toEnable;
    public GameObject[] toDisable;

    public List<DialogueRunner.ConversationWindow> otherWindows;
    //public SoundInteraction  
    //public AudioSource windowSound;

        

 
    public override void Interact()
    {
        //check if window is enabled, if it isn't enable it, if it is disable it.
        //{
        // blind strech goal (if (blindIsOpen == true)){} 

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
            //anim.SetBool("WindowStayUpBool", true);
            //anim.SetBool("WindowStayDownBool", false);
            interactionAnim.SetBool("WindowStayUpBool", true);
            interactionAnim.SetBool("WindowStayDownBool", false);
            interactEnabled = true;
            if (shouldPlayBeachLoop) {
                beachSource.Play();
            }
            //windowSound.Play(0);

            conversationWindows = otherWindows;
            DialogueStart();

        }
        else
        {
            if (thirdInt)
            {

                foreach (GameObject obj in toDisable)
                {
                    obj.SetActive(false);
                }

                interactEnabled = false;
            }
            ScreenDarkener.Instance.Darken(this);
            thirdInt = true;
        }
            //else{blindIsOpen = true);}
        //}
    }


        // Use this for initialization
        void Start()
        {
           
            //windowSound = GetComponent<AudioSource>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

    }
