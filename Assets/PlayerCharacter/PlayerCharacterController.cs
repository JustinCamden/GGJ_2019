using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerCharacterActions))]
public class PlayerCharacterController : MonoBehaviour {

    [Tooltip("The maximum movement speed of the character.")]
    public float maxMovementSpeed = 250.0f;

    [Tooltip("The movement speed acceleration curve of the character.")]
    public AnimationCurve accelerationCurve;

    [Tooltip("How long it takes to accelerate.")]
    [Range(0.0f, 5.0f)]
    public float accelerationTime = 1.0f;

    [Tooltip("How the gravity scale for the character.")]
    public float gravity = 9.8f;

    [Tooltip("The initial acceleration force of our jump.")]
    public float jumpAcceleration = 5.0f;

    [Tooltip("The character controller to send input to.")]
    public CharacterController ownerCharacterController;

    [Tooltip("The camera to base our movement on.")]
    public Camera ownerCamera;

    [Tooltip("How fast the character should rotate in degrees per second.")]
    public float movementRotationSpeed = 360.0f;

    // Our current position along the curve
    float accelerationProgress = 0.0f;

    // Our current vertical speed
    float verticalSpeed = 0.0f;

    // Player character actions instance
    PlayerCharacterActions owningCharacterActions;

    void Start () {
        // Grab refs
        if (!ownerCharacterController)
        {
            ownerCharacterController = GetComponent<CharacterController>();
        }
        if (!ownerCamera)
        {
            ownerCamera = Camera.main;
        }

        // Create player actions
        owningCharacterActions = new PlayerCharacterActions();

        // Bind default key bindings
        // Move up
        owningCharacterActions.moveUp.AddDefaultBinding(Key.W);
        owningCharacterActions.moveUp.AddDefaultBinding(Key.UpArrow);
        owningCharacterActions.moveUp.AddDefaultBinding(InputControlType.LeftStickUp);
        owningCharacterActions.moveUp.AddDefaultBinding(InputControlType.DPadUp);

        // Move down
        owningCharacterActions.moveDown.AddDefaultBinding(Key.S);
        owningCharacterActions.moveDown.AddDefaultBinding(Key.DownArrow);
        owningCharacterActions.moveDown.AddDefaultBinding(InputControlType.LeftStickDown);
        owningCharacterActions.moveDown.AddDefaultBinding(InputControlType.DPadDown);

        // Move right
        owningCharacterActions.moveRight.AddDefaultBinding(Key.D);
        owningCharacterActions.moveRight.AddDefaultBinding(Key.RightArrow);
        owningCharacterActions.moveRight.AddDefaultBinding(InputControlType.LeftStickRight);
        owningCharacterActions.moveRight.AddDefaultBinding(InputControlType.DPadRight);

        // Move left
        owningCharacterActions.moveLeft.AddDefaultBinding(Key.A);
        owningCharacterActions.moveLeft.AddDefaultBinding(Key.LeftArrow);
        owningCharacterActions.moveLeft.AddDefaultBinding(InputControlType.LeftStickLeft);
        owningCharacterActions.moveLeft.AddDefaultBinding(InputControlType.DPadLeft);

        // Jump
        owningCharacterActions.jump.AddDefaultBinding(Key.Space);
        owningCharacterActions.jump.AddDefaultBinding(InputControlType.Action1);
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Update acceleration if appropriate
        Vector3 movementAcceleration = Vector3.zero;
        float deltaTime = Time.deltaTime;
        if (owningCharacterActions.moveAxis.IsPressed || ownerCharacterController.velocity.sqrMagnitude > 0.0f)
        {

            // Transform camera space input into world space
            // If movement is pressed use the pressed direction
            // Otherwise, use the current velocity
            Vector3 movementDirection =
                (owningCharacterActions.moveAxis.IsPressed ?
                ownerCamera.transform.TransformDirection(new Vector3
                (owningCharacterActions.moveAxis.X, 0.0f,
                owningCharacterActions.moveAxis.Y))
                : ownerCharacterController.velocity);

            // Make sure we flatten the Y since we don't want to move the character up or down based on 
            // the movement axis
            movementDirection.y = 0.0f;

            if (movementDirection != Vector3.zero)
            {
                // Normalize to get the final movement direction
                movementDirection.Normalize();

                // Get the acceleration based on the movement direction
                movementAcceleration = GetAcceleration(movementDirection, deltaTime);

                // Rotate towards the movement direction
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(movementDirection), movementRotationSpeed * deltaTime);
            }
        }

        // Add gravity if appropriate
        if (!ownerCharacterController.isGrounded)
        {
            verticalSpeed += -gravity * deltaTime;
        }
        else
        {
            // If we are grounded, then jump
            if (owningCharacterActions.jump.WasPressed)
            {
                verticalSpeed = jumpAcceleration;
            }
            else
            {
                // Otherwise flatten our y velocity
                verticalSpeed = 0.0f;
            }


        }
        movementAcceleration.y = verticalSpeed * deltaTime;
        if (movementAcceleration != Vector3.zero)
        {
            ownerCharacterController.Move(movementAcceleration);
        }
    }

    Vector3 GetAcceleration(Vector3 movementDirection, float deltaTime)
    {
        // Accelerate or decelerate depending on whether movement is pressed
        accelerationProgress = Mathf.Lerp(0.0f, 1.0f, deltaTime * (1.0f / accelerationTime) * (owningCharacterActions.moveAxis.IsPressed ? 1.0f : -1.0f));

        // Final movement vector is the acceleration curve at acceleration progress * maxSpeed * direction
        return (movementDirection * accelerationCurve.Evaluate(accelerationProgress) * maxMovementSpeed) * deltaTime;
    }
}
