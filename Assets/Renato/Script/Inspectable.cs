using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : Interactable
{
    private Transform inspectObjectTransform;
    private new Camera camera;

    private float deltaRotationX;
    private float deltaRotationY;
    [SerializeField] private float rotateSpeed = 2f;
    [SerializeField] private bool interacting;


    void Start() 
    {
        camera = Camera.main;
        inspectObjectTransform = transform;
    }

    public override void Interact() 
    {
        base.Interact();
        InputInspect();
    }

    private void InputInspect() 
    {
        if(Input.GetKey(KeyCode.E) && !interacting) // Change into unity's input system
        {
            Debug.Log("Start inspecting");
            interacting = true;
            
            target.TryGetComponent<PlayerController>(out var playerController);
            
            // Stop movement
            playerController.allowedToMove = false;

            // Stop camera shaking
            playerController.ableToShake = false;

            // Fix the camera towards the object
        } 
        
        if(Input.GetKey(KeyCode.R) && interacting) 
        {
            Debug.Log("Stop inspecting");
            interacting = false;

            target.TryGetComponent<PlayerController>(out var playerController);

            // Reset movement
            playerController.allowedToMove = true;

            // Reset shaking
            playerController.ableToShake = true;

            // Release the camera
        }
    }

    // void Update() 
    // {
    //     if(Input.GetMouseButtonDown(0)) 
    //     {
    //         if (CameraToMouseRay(Input.mousePosition, out RaycastHit RayHit))
    //         {
    //             RayHit.transform.gameObject.TryGetComponent<Interactable>(out var interactable);
    //             if(interactable._InteractableType == InteractableType.INSPECTABLE)
    //                 inspectObjectTransform = RayHit.transform;
    //         }
    //     }

    //     deltaRotationX = -Input.GetAxis("Mouse X");
    //     deltaRotationY = Input.GetAxis("Mouse Y");

    //     if(Input.GetMouseButton(1)) 
    //     {
    //         if(inspectObjectTransform == null)
    //             return; 
                
    //         inspectObjectTransform.rotation = 
    //             Quaternion.AngleAxis(deltaRotationX * rotateSpeed, transform.up) * 
    //             Quaternion.AngleAxis(deltaRotationY * rotateSpeed, transform.right) * 
    //             inspectObjectTransform.rotation;
    //     }
    // }

    // private bool CameraToMouseRay(Vector3 mousePosition, out RaycastHit rayHit)
    // {
    //     Ray ray = camera.ScreenPointToRay(mousePosition);

    //     return Physics.Raycast(ray, out rayHit);
    // }
    
}
