using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : Interactable
{
    [SerializeField] private new Camera camera;
    private InspectObject _InspectObject;
    public PlayerController _PlayerContr;
    [SerializeField] private Vector3 yOffset = new(0, 0, 0);

    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private bool interacting;
    [SerializeField] private bool inInspectMode;
    [SerializeField] private bool objectGrabbed;
    [SerializeField] private float forcePower = 2.5f;

    void Start() 
    {   
        camera = Camera.main;
        _InspectObject = camera.GetComponent<InspectObject>();
    }

    public override void Interact() 
    {
        base.Interact();

        _PlayerContr = camera.GetComponentInParent<PlayerController>();
        InteractWithObject();
        InputInspect();
    }

    private void InputInspect() 
    {
        if(Input.GetKey(KeyCode.F) && !interacting) // Change into unity's input system
        {
            if(inInspectMode)
                return;

            inInspectMode = true;

            Debug.Log("Start inspecting");
            interacting = true;
            
            target.TryGetComponent<PlayerController>(out var playerController);
            
            // Stop movement
            playerController.allowedToMove = false;

            // Stop camera shaking
            playerController.ableToShake = false;

            // Fix the camera towards the object
            // Set the camera position to the initalposition
            camera.transform.localPosition = _PlayerContr.initialCamPos + yOffset;

            // Stop looking arounnd
            playerController.ableToLookAround = false;

            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
            
            // Inspect object
            _InspectObject.ableToInspect = true;
        } 
        
        if(Input.GetKey(KeyCode.R) && interacting) 
        {
            if(!inInspectMode)
                return;

            inInspectMode = false;

            Debug.Log("Stop inspecting");
            interacting = false;

            // Reset movement
            _PlayerContr.allowedToMove = true;

            // Reset shaking
            _PlayerContr.ableToShake = true;

            // Release the camera
            _PlayerContr.ableToLookAround = true;

            // Lock cursor
            Cursor.lockState = CursorLockMode.Locked;

            _InspectObject.ableToInspect = false;
        }
    }


    void InteractWithObject() 
    {
        if(Input.GetKey(KeyCode.E)) 
        {
            if(objectGrabbed)
                return;

            objectGrabbed = true;

            transform.position = _PlayerContr.objectPos.transform.position;
            transform.SetParent(_PlayerContr.objectPos.transform);
        }
    }

    void Update() 
    {
        if(objectGrabbed && Input.GetKey(KeyCode.L)) // Release object
        {
            Rigidbody rb = transform.gameObject.AddComponent<Rigidbody>();
            rb.useGravity = true;
            rb.AddForce(transform.forward * forcePower, ForceMode.Impulse);
        }   
    }
}
