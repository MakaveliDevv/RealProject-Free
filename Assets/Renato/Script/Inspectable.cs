using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : Interactable
{
    [SerializeField] private new Camera camera;
    private InspectObject _InspectObject;
    // public PlayerController _PlayerContr;
    private Rigidbody rb;

    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private bool inInspectMode;
    // [SerializeField] private bool objectGrabbed;
    [SerializeField] private float forcePower = 2.5f;


    // Additional variables to freeze the camera
    private Vector3 frozenCameraPosition;
    private Quaternion frozenCameraRotation;
    [SerializeField] private float distanceFromCamera = 2f;

    void Start() 
    {   
        camera = Camera.main;
        _InspectObject = camera.GetComponent<InspectObject>();
        rb = GetComponent<Rigidbody>();
    }

    public override void Interact() 
    {
        base.Interact();

        // _PlayerContr = camera.GetComponentInParent<PlayerController>();
        Inspect();
    }



    void Inspect() 
    {
        // Check if player pressed the inspect button while grabbing
        if(_InteractableType == InteractableType.INSPECTABLE 
        && Input.GetKey(KeyCode.I)) 
        {   
            // Inspect
            if(_InspectObject.inspectMode)
                return;

            camera.transform.localPosition = _PlayerContr.initialCamPos;

            // Freeze the camera on the current position
            frozenCameraPosition = camera.transform.position;
            frozenCameraRotation = camera.transform.rotation;

            // Disable camera movement by setting initial position and rotation
            camera.transform.SetPositionAndRotation(frozenCameraPosition, frozenCameraRotation);

            // Set the object in the middle of the camera
            transform.position = camera.transform.position + camera.transform.forward * distanceFromCamera;
            transform.SetParent(null); // Detach from any parent to be centered independently

            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            
            // Inspect object
            _InspectObject.inspectMode = true;
        }

        // Check if player pressed to grab again
        if(_InspectObject.inspectMode && Input.GetKey(KeyCode.R)) 
        {
            _InspectObject.inspectMode = false;

            // Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
            // rb.useGravity = true;
            // rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);


        } 
        
        // Check if player pressed to let it go
        else if(_InspectObject.inspectMode && Input.GetKey(KeyCode.L))
        {
            // Check what type of object
            // If floating object, then float

            // If non float object, then drop object by gravity
        }

        // Or if the player choose to grab it again
    }
    // private void InputInspect() 
    // {
    //     if(Input.GetKey(KeyCode.F)) // Change into unity's input system
    //     {
            // if(inInspectMode)
            //     return;

            // inInspectMode = true;

            // Debug.Log("Start inspecting");
            // interacting = true;
            
            // target.TryGetComponent<PlayerController>(out var playerController);
            
            // // Stop movement
            // playerController.allowedToMove = false;

            // // Stop camera shaking
            // playerController.ableToShake = false;

            // // Fix the camera towards the object
            // // Set the camera position to the initalposition
            // camera.transform.localPosition = _PlayerContr.initialCamPos + yOffset;

            // // Stop looking arounnd
            // playerController.ableToLookAround = false;

            // // Unlock cursor
            // Cursor.lockState = CursorLockMode.None;
            
            // // Inspect object
            // _InspectObject.ableToInspect = true;
        // } 
        
    //     if(Input.GetKey(KeyCode.R)) 
    //     {
    //         if(!inInspectMode)
    //             return;

    //         inInspectMode = false;

    //         // Reset movement
    //         _PlayerContr.allowedToMove = true;

    //         // Reset shaking
    //         _PlayerContr.ableToShake = true;

    //         // Release the camera
    //         _PlayerContr.ableToLookAround = true;

    //         // Lock cursor
    //         Cursor.lockState = CursorLockMode.Locked;

    //         _InspectObject.inspectMode = false;
    //     }
    // }

    // void Update() 
    // {
    //     if(objectGrabbed && Input.GetKey(KeyCode.L)) // Release object
    //     {
    //         Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
    //         rb.useGravity = true;
    //         rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);
    //     }   
    // }
}
