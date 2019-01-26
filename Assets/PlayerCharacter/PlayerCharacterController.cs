using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InControl;

[RequireComponent(typeof(CharacterController))]
public class PlayerCharacterController : MonoBehaviour {

    [Tooltip("The maximum movement speed of the character.")]
    public float maxMovementSpeed = 5.0f;

    [Tooltip("The movement speed acceleration curve of the character.")]
    public AnimationCurve accelerationCurve;

    [Tooltip("How long it takes to accelerate.")]
    [Range(0.0f, 5.0f)]
    public float accelerationTime = 0.1f;

    [Tooltip("How the gravity scale for the character.")]
    public float gravity = 9.8f;

    [Tooltip("The initial acceleration force of our jump.")]
    public float jumpAcceleration = 5.0f;

    [Tooltip("The character controller to send input to.")]
    public CharacterController ownerCharacterController;

    [Tooltip("The camera to base our movement on.")]
    public Camera ownerCamera;

    [Tooltip("How fast the character should rotate in degrees per second.")]
    public float movementTurningSpeed = 720.0f;

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
        // Cache local variables
        Vector3 movementAcceleration = Vector3.zero;
        float deltaTime = Time.deltaTime;
        bool moveAxisPressed = owningCharacterActions.moveAxis.IsPressed;

       // Update lateral movement if appropriate
        if (moveAxisPressed || ownerCharacterController.velocity.sqrMagnitude > 0.0f)
        {

            // Transform camera space input into world space
            // If movement is pressed use the pressed direction
            // Otherwise, use the current velocity
            Vector3 horizontalDirection;
            float horizontalAccelerationScalar = 1.0f;
            if (moveAxisPressed)
            {
                Vector2 moveInput = new Vector2(owningCharacterActions.moveAxis.X, owningCharacterActions.moveAxis.Y);
                horizontalDirection = ownerCamera.transform.TransformDirection(new Vector3(moveInput.x, 0.0f, moveInput.y));
                horizontalDirection.y = 0.0f;
                horizontalAccelerationScalar = Mathf.Clamp(moveInput.magnitude, -1.0f, 1.0f);
            }
            else
            {
                horizontalDirection = new Vector3(ownerCharacterController.velocity.x, 0.0f, ownerCharacterController.velocity.z);
            }
            horizontalDirection.Normalize();

            if (horizontalDirection != Vector3.zero)
            {
                // Normalize to get the final movement direction
                horizontalDirection.Normalize();

                // Get the acceleration based on the movement direction
                movementAcceleration = GetHorizontalAcceleration(horizontalDirection, horizontalAccelerationScalar, deltaTime);

                // Rotate towards the movement direction
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(horizontalDirection), movementTurningSpeed * deltaTime);
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

    Vector3 GetHorizontalAcceleration(Vector3 movementDirection, float maxAccelerationScalar, float deltaTime)
    {
        // Accelerate or decelerate depending on whether movement is pressed
        accelerationProgress = Mathf.Lerp(0.0f, 1.0f, accelerationProgress + deltaTime * (1.0f / accelerationTime) * (owningCharacterActions.moveAxis.IsPressed ? 1.0f : -1.0f));

        // Final movement vector is the acceleration curve at acceleration progress * maxSpeed * direction
        return (movementDirection * Mathf.Min(accelerationCurve.Evaluate(accelerationProgress), maxAccelerationScalar) * maxMovementSpeed) * deltaTime;
    }
}
