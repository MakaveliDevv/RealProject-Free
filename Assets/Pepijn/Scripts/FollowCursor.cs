using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCursor : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromCamera = 10f; // Distance from the camera to place the GameObject
    private Vector2 inputVector;
    public Vector2 inputDirection;
    public float cursorSpeed = 20f;
    public bool collisionDetected;
    public List<GameObject> starList = new();

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        MoveCursor();
        
        if(collisionDetected) 
        {
            for (int i = 0; i < starList.Count; i++)
            {
                GameObject star = starList[0].transform.gameObject;
                star.transform.position = transform.position;
            }
        } 
    }

    public void InputVectorCursor(Vector2 direction) 
    {
        inputVector = direction;
    }

    private void MoveCursor() 
    {
        inputDirection = new(inputVector.x, inputVector.y);
        inputDirection = transform.TransformDirection(inputDirection);
        Vector3 newPosition = transform.position + (cursorSpeed * Time.deltaTime * (Vector3)inputDirection);
        newPosition.z = distanceFromCamera; // Set the Z position to -10f
        transform.position = newPosition;
    }

    public void MoveObjectInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            InputVectorCursor(ctx.ReadValue<Vector2>());
        }
        else if (ctx.canceled)
            InputVectorCursor(Vector2.zero);
    }

    // void OnTriggerEnter(Collider collider)
    // {
    //     if(collider.CompareTag("Star"))
    //     {
    //         Debug.Log("Made collision");
    //         collisionDetected = true;
    
    //         starList.Add(collider.transform.gameObject);
    //     }
    // }
}


        // Get the mouse position in screen coordinates
        // Vector3 mousePosition = Input.mousePosition;

        // // Set the z-coordinate to the desired distance from the camera
        // mousePosition.z = distanceFromCamera;

        // // Convert the screen position to world position
        // Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // // Update the GameObject's position
        // transform.position = worldPosition;