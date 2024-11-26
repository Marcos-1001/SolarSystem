using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    public float movementSpeed = 3f; // Movement speed
    public float rotationSpeed = 90f; // Rotation speed

    private Transform playerTransform;

    void Start()
    {
        playerTransform = this.transform; // Reference to the player's transform
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        // Get input from the left thumbstick
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Translate the player forward/backward and left/right
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        playerTransform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.Self);
    }

    private void HandleRotation()
    {
        // Get input from the right thumbstick
        Vector2 rotationInput = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        // Rotate the player
        float rotation = rotationInput.x * rotationSpeed * Time.deltaTime;
        playerTransform.Rotate(0, rotation, 0);
    }
}
