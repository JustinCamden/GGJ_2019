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

    [Tooltip("How long continuing to press the jump key will continue lengthen the jump.")]
    public float fatJumpTime = 0.1f;

    [Tooltip("The collider we use for detecting interactables.")]
    public ChildCollider interactionCollider;

    [Tooltip("The animator for the character.")]
    public Animator characterAnimator;

    // The name of the jump variable for the animator
    const string jumpAnimKey = "Jumping";

    // The name of the walk variable for the animator
    const string walkAnimKey = "Walking";

    // Our current position along the curve
    float accelerationProgress = 0.0f;

    // Our current vertical speed
    float verticalSpeed = 0.0f;

    // The start time of the last jump
    float jumpStartTimeStamp;

    // Whether we are currently fat jumping
    bool fatJumping = false;

    // The currently overlapping interactables
   public List<Collider> overlappingInteractables;

    // The currently selected interactable
    BaseInteraction selectedInteractable;

    // Whether movement input is currently enabled
    public bool movementInputEnabled = true;

    // Whether walking is currently enabled
    bool walkingEnabled = true;

    // Whether jumping is currently enabled
    bool jumpingEnabled = true;

    // Whether interaction is currently enabled
    bool interactionEnabled = true;

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
        // Find child interaction collider if we have none
        if (!interactionCollider)
        {
            interactionCollider = GetComponentInChildren<ChildCollider>();
            if (interactionCollider)
            {
                Rigidbody interactionColliderRigidBody = interactionCollider.gameObject.GetComponent<Rigidbody>();
                if (!interactionColliderRigidBody)
                {
                    interactionColliderRigidBody = interactionCollider.gameObject.AddComponent<Rigidbody>();
                }
                if (interactionColliderRigidBody)
                {
                    interactionColliderRigidBody.isKinematic = true;
                    interactionColliderRigidBody.useGravity = false;
                }
            }
        }
        if (interactionCollider)
        {
            interactionCollider.onChildTriggerEnter += OnInteractionTriggerEnter;
            interactionCollider.onChildTriggerExit += OnInteractionTriggerExit;
        }
        if (!characterAnimator)
        {
            characterAnimator = GetComponentInChildren<Animator>();
        }

        // Initialize lists
        overlappingInteractables = new List<Collider>();
    }

    // Update is called once per frame
    void Update ()
    {
        // Cache local variables
        Vector3 movementAcceleration = Vector3.zero;
        float deltaTime = Time.deltaTime;
        bool moveAxisPressed = PlayerInputManager.PlayerActions.moveAxis.IsPressed && CanWalk();

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
                Vector2 moveInput = new Vector2(PlayerInputManager.PlayerActions.moveAxis.X, PlayerInputManager.PlayerActions.moveAxis.Y);
                horizontalDirection = ownerCamera.transform.TransformDirection(new Vector3(moveInput.x, 0.0f, moveInput.y));
                horizontalDirection.y = 0.0f;
                horizontalAccelerationScalar = Mathf.Clamp(moveInput.magnitude, -1.0f, 1.0f);
            }
            else
            {
                horizontalDirection = new Vector3(ownerCharacterController.velocity.x, 0.0f, ownerCharacterController.velocity.z);
            }

            if (horizontalDirection != Vector3.zero)
            {
                // Normalize to get the final movement direction
                horizontalDirection.Normalize();

                // Get the acceleration based on the movement direction
                movementAcceleration = GetHorizontalAcceleration(horizontalDirection, horizontalAccelerationScalar, deltaTime);

                // Rotate towards the movement direction
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(horizontalDirection), movementTurningSpeed * deltaTime);
            }

            characterAnimator.SetBool(walkAnimKey, true);
        }
        else
        {
            characterAnimator.SetBool(walkAnimKey, false);
        }

        // Update jump state
        if (ownerCharacterController.isGrounded)
        {
            // Start when pressed
            if (PlayerInputManager.PlayerActions.jump.WasPressed && CanJump())
            {
                verticalSpeed = jumpAcceleration;
                fatJumping = true;
                jumpStartTimeStamp = Time.time;
                characterAnimator.SetBool(jumpAnimKey, true);
            }
            // Stop when on the ground
            else
            {
                verticalSpeed = -gravity;
                fatJumping = false;
                characterAnimator.SetBool(jumpAnimKey, false);
            }
        }

        // Update falling state
        else
        {
            // Update fat jumping
            if (fatJumping)
            {
                // End fat jump when jump released or we go beyond the maximum time
                if (!PlayerInputManager.PlayerActions.jump.IsPressed || Time.time - jumpStartTimeStamp > fatJumpTime)
                {
                    fatJumping = false;
                    verticalSpeed -= gravity * Time.deltaTime;
                }

                // Otherwise, continue fat jumping
                // Do nothing in that case
            }

            // Handle gradual deceleration
            else
            {
                // Double deceleration at jump maximum
                verticalSpeed -= gravity * deltaTime * (verticalSpeed < 0.0f ? 2.0f : 1.0f);
            }
        }

        // Combine and apply horizontal and vertical movement
        movementAcceleration.y = verticalSpeed * deltaTime;
        if (movementAcceleration != Vector3.zero)
        {
            ownerCharacterController.Move(movementAcceleration);
        }

        // Update the best interactable if appropriate
        if (overlappingInteractables.Count > 1)
        {
            // Find the interactable we are closest to
            BaseInteraction bestInteractable = null;
            float smallestDistance = 9999.0f;
            foreach (Collider interactable in overlappingInteractables)
            {
                float distanceToInteractable = Vector3.Distance(ownerCharacterController.transform.position, interactable.transform.position);
                if (distanceToInteractable < smallestDistance)
                {
                    bestInteractable = interactable.gameObject.GetComponent<BaseInteraction>();
                    smallestDistance = distanceToInteractable;
                }
            }

            // Update our best interactable
            if (bestInteractable != selectedInteractable)
            {
                // Deselect old best
                if (selectedInteractable)
                {
                    selectedInteractable.OnDeselected();
                }

                // Select new best
                selectedInteractable = bestInteractable;
                if (selectedInteractable)
                {
                    selectedInteractable.OnSelected();
                }
            }
        }

        // Interact with selected interactable if appropriate
        if (PlayerInputManager.PlayerActions.interact.WasPressed && selectedInteractable && CanInteract())
        {
            selectedInteractable.TryInteract();
            for(int i=0;i<overlappingInteractables.Count;i++){
            if(selectedInteractable.gameObject.name == overlappingInteractables[i].gameObject.name){
                EnableDisable overlap = overlappingInteractables[i].GetComponent<EnableDisable>();
                for(int x=0;x<overlap.toDisable.Length;x++){
                    if(overlap.toDisable[x].gameObject.name == selectedInteractable.gameObject.name){
                        overlappingInteractables.RemoveAt(i);
                    }
                }
            }
            }
        }
    }

    Vector3 GetHorizontalAcceleration(Vector3 movementDirection, float maxAccelerationScalar, float deltaTime)
    {
        // Accelerate or decelerate depending on whether movement is pressed
        accelerationProgress = Mathf.Lerp(0.0f, 1.0f, accelerationProgress + deltaTime * (1.0f / accelerationTime) * (PlayerInputManager.PlayerActions.moveAxis.IsPressed ? 1.0f : -1.0f));

        // Final movement vector is the acceleration curve at acceleration progress * maxSpeed * direction
        return (movementDirection * Mathf.Min(accelerationCurve.Evaluate(accelerationProgress), maxAccelerationScalar) * maxMovementSpeed) * deltaTime;
    }

    void OnInteractionTriggerEnter(ChildCollider callingChildCollider, Collider other)
    {
        BaseInteraction interactable = other.gameObject.GetComponent<BaseInteraction>();
        if (interactable)
        {
            overlappingInteractables.Add(other);
            if (overlappingInteractables.Count < 2)
            {
                selectedInteractable = interactable;
                selectedInteractable.OnSelected();
            }
        }

        return;
    }

    void OnInteractionTriggerExit(ChildCollider callingChildCollider, Collider other)
    {
        BaseInteraction interactable = other.gameObject.GetComponent<BaseInteraction>();
        if (interactable)
        {
            overlappingInteractables.Remove(other);
            if (overlappingInteractables.Count <= 0 && selectedInteractable)
            {
                selectedInteractable.OnDeselected();
                selectedInteractable = null;
            }
        }
    }

    bool CanJump() { return movementInputEnabled && walkingEnabled; }
    bool CanWalk() { return movementInputEnabled && jumpingEnabled; }
    bool CanInteract() { return interactionEnabled; }
}
