using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;



public class VRMenuRaycaster : MonoBehaviour
{
    public Transform controller;

    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        // Create a pointer event data object
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = new Vector2(controller.transform.position.x, controller.transform.position.y); // Use controller position

        if (Physics.Raycast(controller.position, controller.forward, out RaycastHit hit))
        {
            if (hit.collider.gameObject.GetComponent<Button>() )
            {
                // Set the pointer event data position to the hit object's position
                //pointerEventData.position = hit.textureCoord;
                Debug.Log("Hit a button");
            }
        }

    }
}