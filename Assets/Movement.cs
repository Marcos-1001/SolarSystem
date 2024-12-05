using Oculus.Interaction;
using UnityEngine;

public class FreeMovement : MonoBehaviour
{

    public OVRPlayerController playerController;
    public Transform cameraTransform; 
    public float movementSpeed = 100f; // Movement speed
    public float rotationSpeed = 90f; // Rotation speed

    public Transform leftHandAnchor; // Left hand anchor

    void Start()
    {
        if (playerController == null)
        {
            // Try to automatically find the OVRPlayerController if not assigned
            playerController = GetComponent<OVRPlayerController>();
        }
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        playerController.EnableRotation = false; 
        
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();

        CharacterController characterController = playerController.GetComponent<CharacterController>();
        
        
        characterController.enabled = false;

        // Get input from the left and right hand triggers
        float leftTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger); // Left hand trigger
        float rightTrigger = OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger); // Right hand trigger

        // Combine the triggers for vertical movement (positive for up, negative for down)
        float triggerInput = leftTrigger - rightTrigger; // Positive for up (left trigger), negative for down (right trigger)

        // Move the player up or down based on the trigger input
        if (Mathf.Abs(triggerInput) > 0.1f)  // Only move if there's a significant trigger press
        {
            
            Vector3 verticalMovement = new Vector3(0, triggerInput, 0);
            playerController.transform.position  += verticalMovement * movementSpeed * 100f * Time.deltaTime;           
            Debug.Log(playerController.transform.position);
        }
    }

    private void HandleMovement()
    {

        Vector2 movementInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);


        // Get the forward and right directions of the camera
        Vector3 forwardDirection = cameraTransform.forward ;  // Camera's forward direction (z-axis)
        Vector3 rightDirection = cameraTransform.right;      // Camera's right direction (x-axis)

        // Normalize the vectors to prevent moving faster diagonally
        forwardDirection.y = 0;  // Keep movement on the horizontal plane (y = 0)
        rightDirection.y = 0;    // Same for the right direction

        forwardDirection.Normalize();
        rightDirection.Normalize();


        
        // Calculate movement vector based on input
        Vector3 movement = forwardDirection * movementInput.y + rightDirection * movementInput.x;
        
        // Move the player in the direction of the camera view (forward/backward + left/right)
        playerController.transform.position += movement * movementSpeed * 100f * Time.deltaTime;
    }

    private void HandleRotation()
    {
        float spinInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick).x; // Horizontal axis of right thumbstick

        // Calculate rotation based on input (spin around the Y-axis)
        float spinAmount = spinInput * 20f * Time.deltaTime;

        // Apply the spin to the player's rotation
        playerController.transform.Rotate(Vector3.up, spinAmount);

    }

}
