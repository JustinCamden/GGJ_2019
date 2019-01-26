using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

public class PlayerInputManager : MonoBehaviour {

    MainPlayerActionSet localPlayerActions;
    private static PlayerInputManager instance = null;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public static MainPlayerActionSet PlayerActions
    {
        get
        {
            return instance.localPlayerActions;
        }
    }

    // Use this for initialization
    void Start () {
        // Create player actions
        localPlayerActions = new MainPlayerActionSet();

        // Bind default key bindings
        // Move up
        localPlayerActions.moveUp.AddDefaultBinding(Key.W);
        localPlayerActions.moveUp.AddDefaultBinding(Key.UpArrow);
        localPlayerActions.moveUp.AddDefaultBinding(InputControlType.LeftStickUp);
        localPlayerActions.moveUp.AddDefaultBinding(InputControlType.DPadUp);

        // Move down
        localPlayerActions.moveDown.AddDefaultBinding(Key.S);
        localPlayerActions.moveDown.AddDefaultBinding(Key.DownArrow);
        localPlayerActions.moveDown.AddDefaultBinding(InputControlType.LeftStickDown);
        localPlayerActions.moveDown.AddDefaultBinding(InputControlType.DPadDown);

        // Move right
        localPlayerActions.moveRight.AddDefaultBinding(Key.D);
        localPlayerActions.moveRight.AddDefaultBinding(Key.RightArrow);
        localPlayerActions.moveRight.AddDefaultBinding(InputControlType.LeftStickRight);
        localPlayerActions.moveRight.AddDefaultBinding(InputControlType.DPadRight);

        // Move left
        localPlayerActions.moveLeft.AddDefaultBinding(Key.A);
        localPlayerActions.moveLeft.AddDefaultBinding(Key.LeftArrow);
        localPlayerActions.moveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        localPlayerActions.moveLeft.AddDefaultBinding(InputControlType.DPadLeft);

        // Jump
        localPlayerActions.jump.AddDefaultBinding(Key.E);
        localPlayerActions.jump.AddDefaultBinding(InputControlType.Action3);

        // Interact
        localPlayerActions.interact.AddDefaultBinding(Key.Space);
        localPlayerActions.interact.AddDefaultBinding(InputControlType.Action1);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
