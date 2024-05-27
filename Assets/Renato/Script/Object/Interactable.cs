using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    // Enum
    public enum InteractableType { INSPECTABLE, READABLE }
    public enum ObjectType { GRABABLE, NON_GRABABLE }
    public enum GravitationalType { FLOATING, NON_FLOATING }
    public InteractableType _InteractableType;
    public ObjectType _ObjectType;
    public GravitationalType _GravitationalType;

    public PlayerController _PlayerContr;
    public InspectObject _InspectObject;
    public bool playerInRange;

    [SerializeField] protected float interactRadius = 1.25f;
    public bool objectPickedup;
    public bool objectReleased;
    public bool ableToInspect;




    [SerializeField] private new Camera camera;
    public Grabable _Grabable;
    public bool releaseAfterInspect;
    public bool grabAfterInspect;

    // Additional variables to freeze the camera
    private Vector3 frozenCameraPosition;
    private Quaternion frozenCameraRotation;
    [SerializeField] private float distanceFromCamera = 2f;
    public PlayerInteraction _PlayerInteraction;


    void Awake()
    {    
        camera = Camera.main;
        _InspectObject = camera.gameObject.GetComponent<InspectObject>();
        _Grabable = GetComponentInChildren<Grabable>();

        if (TryGetComponent<SphereCollider>(out var col))
        {
            col.radius = interactRadius;
            col.isTrigger = true;
        }
    }

    virtual public void Update()
    {
        if(!objectPickedup) 
        {
            if (_InspectObject.CameraToMouseRay())
            {
                if (_InspectObject.rayExists)
                {
                    if (_InspectObject.hitInfo.transform.CompareTag("Interactable"))
                    {
                        _InspectObject.inspectObject = _InspectObject.hitInfo.transform;
                        _InspectObject.objectHit = true;
                    }
                    else
                        _InspectObject.objectHit = false;
                }
            }
            else
            {
                _InspectObject.objectHit = false; // Set objectHit to false if no object is detected
                playerInRange = false;
                ableToInspect = false;
            }

            if(_InspectObject.objectHit)
            {
                _PlayerContr = _InspectObject.GetComponentInParent<PlayerController>();
                Debug.Log(_InspectObject.hitInfo.transform.gameObject.name);
                playerInRange = true;
            }   
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, interactRadius);
    }

    public void Inspect() 
    {
        _PlayerInteraction = _InspectObject.GetComponentInParent<PlayerInteraction>();
        
        // _PlayerInteraction.ableToInspect
        if(_InteractableType == InteractableType.INSPECTABLE) 
        {   
            releaseAfterInspect = false;
            grabAfterInspect = false;
            _Grabable.grabable = true;

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


            // Remove object from the Grabables list
            Inventory.instance._Grabables.Clear();

            objectPickedup = false;

            // if(_Grabable != null)
            // {
            //     if(_Grabable.sphereCol != null)
            //     {
            //         _Grabable.sphereCol.enabled = true;
            //         _Grabable.sphereCol.radius = 3f;
            //     }


            //     // Interactable
            //     SphereCollider sphereColliderInteractable = GetComponent<SphereCollider>();
            //     sphereColliderInteractable.enabled = true;
            //     sphereColliderInteractable.radius = 5f;
            // }
        
            // Unlock cursor
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void AfterInspectInput() 
    {
        if(releaseAfterInspect)     
            _Grabable.Drop();
    
        else if(grabAfterInspect)     
            _Grabable.Grab();
    

        Cursor.lockState = CursorLockMode.Locked;
        _InspectObject.inspectMode = false;
    }
}
