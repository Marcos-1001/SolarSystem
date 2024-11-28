using UnityEngine;

public class FreeMovement : MonoBehaviour
{
    public float movementSpeed = 3f; // Movement speed
    public float rotationSpeed = 90f; // Rotation speed

    public Transform leftHandAnchor; // Left hand anchor

    void Start()
    {
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        
        // Get input from the left thumbstick
        Vector2 movementInput = OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick);

        // Move the player
        Vector3 movement = new Vector3(movementInput.x, 0, movementInput.y) * movementSpeed * Time.deltaTime;
        transform.Translate(movement);


    }

    private void HandleRotation()
    {
        
    }
}
