using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FigureHandler : MonoBehaviour {

    public static FigureHandler fh;
    Rigidbody rb;

    public bool hasFigure = false;
    public bool isBlocked = false;

    public GameObject[] figures;
    public GameObject[] detectors;

    GameObject curFigure;

    int figTracker = 0;

    private void Awake()
    {
        fh = this;
        rb = GetComponent<Rigidbody>();
    }

    // Use this for initialization
    void Start () 
    {
		
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            FigureStart();
        }

        if (hasFigure)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
            rb.MovePosition(transform.position + move * 1.75f * Time.deltaTime);


            if (Input.GetAxis("Vertical") > 0)
                transform.position = new Vector3(transform.position.x, 0.02f, transform.position.z);
            if (Input.GetAxis("Vertical") < 0)
                transform.position = new Vector3(transform.position.x, -1.24f, transform.position.z);

            if (Input.GetKeyUp(KeyCode.Space) && !isBlocked)
                PlaceFigure();
        }
    }

    void SwitchFigure()
    {
        curFigure = Instantiate(detectors[figTracker], fh.gameObject.transform.position, fh.gameObject.transform.rotation) as GameObject; //Instantiates next poster for the poster minigame
        curFigure.transform.SetParent(fh.gameObject.transform); // Assigns it to move along with the controller
        hasFigure = true;
    }

    void PlaceFigure()
    {
        Instantiate(figures[figTracker], fh.transform.position, fh.transform.rotation);
        figTracker += 1;
        curFigure.SetActive(false);

        if (figTracker != figures.Length)
            SwitchFigure();

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
