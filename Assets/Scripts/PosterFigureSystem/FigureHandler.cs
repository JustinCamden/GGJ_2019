using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class FigureHandler : MonoBehaviour {

    public static FigureHandler fh;
    Rigidbody rb;

    public bool hasFigure = false; //Serves to handle if the game is being played
    public bool isBlocked = false; //Checks that there is no figure overlaps, assigned by FigureDetector.cs

    public GameObject[] figures; //Prefabs for each figure that is to be placed
    public GameObject[] detectors; //Corresponding semi-transparent prefabs for previewing placement

    GameObject curFigure; //Current detector prefab in hand

    int figTracker = 0; //Keeps track of current numbers of placed figures

    private PlayerCharacterController pCC;
    
    private void Awake()
    {
        fh = this;
        rb = GetComponent<Rigidbody>();
        transform.position = new Vector3(transform.position.x, -1.92f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F)) //Debug Start 
        {
            FigureStart();
        }

        if (hasFigure)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            rb.MovePosition(transform.position + move * 1.75f * Time.deltaTime); //Change hardcoded float for mini-game movement speed adjustment


            if (Input.GetAxis("Vertical") > 0) // Moves between shelves with the figure placement
                transform.position = new Vector3(transform.position.x, -0.72f, transform.position.z);
            if (Input.GetAxis("Vertical") < 0)
                transform.position = new Vector3(transform.position.x, -1.92f, transform.position.z);

            if (Input.GetKeyUp(KeyCode.Space) && !isBlocked || InputManager.ActiveDevice.Action1.WasPressed && !isBlocked)
                PlaceFigure();
        }
    }

    void SwitchFigure() // Switches the detector in the placer
    {
        curFigure = Instantiate(detectors[figTracker], fh.gameObject.transform.position, Quaternion.identity) as GameObject; //Instantiates next poster for the poster minigame
        curFigure.transform.SetParent(fh.gameObject.transform); // Assigns it to move along with the controller
        hasFigure = true;
    }

    void PlaceFigure() // Spawns the actual figure in the current position of the detector
    {
        Instantiate(figures[figTracker], fh.transform.position, Quaternion.identity);
        figTracker += 1;
        curFigure.SetActive(false);

        if (figTracker != figures.Length)
        {
            SwitchFigure();
        }
        else
        {
            CameraRailer.cr.MoveCamera(CameraRailer.cr.overWorldTrans, CameraRailer.cr.overWorldSize);
            pCC = GameObject.FindObjectOfType<PlayerCharacterController>();
            pCC.movementInputEnabled = true;
        }

    }

    public void FigureStart() //Call this from interactable to start the figure mini-game.
    {
        StartCoroutine(StartFigures());
    }

    IEnumerator StartFigures()
    {
        yield return new WaitForSeconds(1.5f);
        SwitchFigure();
    }
}
