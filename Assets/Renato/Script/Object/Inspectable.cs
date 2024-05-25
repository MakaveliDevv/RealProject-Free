using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspectable : Interactable
{
    [SerializeField] private new Camera camera;
    public InspectObject _InspectObject;
    public bool releaseAfterInspect;
    public bool grabAfterInspect;

    // Additional variables to freeze the camera
    private Vector3 frozenCameraPosition;
    private Quaternion frozenCameraRotation;
    [SerializeField] private float distanceFromCamera = 2f;
    public PlayerInteraction _PlayerInteraction;

    void Start() 
    {   
        camera = Camera.main;
        _InspectObject = camera.GetComponent<InspectObject>();
    }

    public void Inspect() 
    {
        _PlayerInteraction = _InspectObject.GetComponentInParent<PlayerInteraction>();
        
        if(_PlayerInteraction.ableToInspect) 
        {   
            releaseAfterInspect = false;
            grabAfterInspect = false;

            _PlayerInteraction.GetComponent<PlayerController>().shake = false;
            _PlayerInteraction.GetComponent<PlayerController>().ableToLookAround = false;
            
            camera.transform.localPosition = _PlayerContr.initialCamPos;

            // Freeze the camera on the current position
            frozenCameraPosition = camera.transform.position;
            frozenCameraRotation = camera.transform.rotation;

            // Disable camera movement by setting initial position and rotation
            camera.transform.SetPositionAndRotation(frozenCameraPosition, frozenCameraRotation);

            Vector3 targetPosition = camera.transform.position + camera.transform.forward * distanceFromCamera;

            // Set the object in the middle of the camera            
            transform.SetParent(camera.transform); // Detach from any parent to be centered independently

            transform.position = targetPosition;            

            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void AfterInspectInput() 
    {
        Grabable _Grabable = GetComponentInChildren<Grabable>();
        _InspectObject.inspectMode = false;

        Cursor.lockState = CursorLockMode.Locked;
        
        if(releaseAfterInspect) 
        {
            _Grabable.Release();
        }
        else if(grabAfterInspect) 
        {
            _Grabable.Grab();
        }
    }
}
