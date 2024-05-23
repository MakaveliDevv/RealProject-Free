using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : Interactable
{
    [SerializeField] private new Camera camera;
    private InspectObject _InspectObject;
    private Rigidbody rb;

    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private bool inInspectMode;
    [SerializeField] private float forcePower = 2.5f;


    // Additional variables to freeze the camera
    private Vector3 frozenCameraPosition;
    private Quaternion frozenCameraRotation;
    [SerializeField] private float distanceFromCamera = 2f;

    void Start() 
    {   
        camera = Camera.main;
        _InspectObject = camera.GetComponent<InspectObject>();
    }

    // public override void Interact() 
    // {
    //     base.Interact();
    // }



    public void Inspect() 
    {
        // Inspect
        if(_InspectObject.inspectMode)
            return;

        // Check if player pressed the inspect button while grabbing
        if(_InteractableType == InteractableType.INSPECTABLE) 
        {   
            _InspectObject.inspectMode = true;         
            
            // sphereCol.enabled = false;
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
        }

        // // Check if player pressed to grab again
        // if(_InspectObject.inspectMode && Input.GetKey(KeyCode.R)) 
        // {
        //     _InspectObject.inspectMode = false;
        //     // sphereCol.enabled = true;


        //     // rb.useGravity = true;
        //     // rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);


        // } 
        
        // // Check if player pressed to let it go
        // else if(_InspectObject.inspectMode && Input.GetKey(KeyCode.L))
        // {
        //     // Check what type of object
        //     // If floating object, then float

        //     // If non float object, then drop object by gravity
        // }

        // Or if the player choose to grab it again
    }
}
