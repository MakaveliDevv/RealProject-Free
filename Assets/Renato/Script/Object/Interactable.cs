using System.Collections;
using System.Collections.Generic;
//using UnityEditor.EditorTools;
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

    // Script References
    public PlayerInteraction _PlayerInteraction; // Using this to fetch the player controller
    private PlayerController _PlayerController;
    [HideInInspector] public InspectObject _InspectObject;
    private Grabable _Grabable;

    // Components
    private new Camera camera;

    // Booleans
    [HideInInspector] public bool playerInRange;
    [HideInInspector] public bool objectPickedup;
    [HideInInspector] public bool ableToInspect;
    public bool objectReleased;
    [HideInInspector] public bool releaseAfterInspect;
    [HideInInspector] public bool grabAfterInspect;

    // Floats
    [SerializeField] protected float interactRadius = 1.25f;

    // Camera stuff
    public GameObject interactionUI;
    private Vector3 frozenCameraPosition;
    private Quaternion frozenCameraRotation;
    [SerializeField] private float distanceFromCamera = 2f;


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

        if(_GravitationalType == GravitationalType.FLOATING)
        {
            if(TryGetComponent<Rigidbody>(out var rb))
                rb.useGravity = false;
        }
        else if(_GravitationalType == GravitationalType.NON_FLOATING)
        {
            if(TryGetComponent<Rigidbody>(out var rb))
            rb.useGravity = true;
            
        }            
    }

    virtual public void Update()
    {
        if(!objectPickedup) 
        {
            if(_InspectObject != null)
            {
                if (_InspectObject.CameraToMouseRay())
                {
                    if (_InspectObject.rayExists)
                    {
                        if (_InspectObject.hitInfo.transform.CompareTag("Interactable"))
                        {
                            _InspectObject.inspectObject = _InspectObject.hitInfo.transform;
                            _InspectObject.objectHit = true;
                            interactionUI.SetActive(true);
                        }
                        else
                        {
                            _InspectObject.objectHit = false;
                        }
                    }
                }
                else
                {
                    _InspectObject.objectHit = false; // Set objectHit to false if no object is detected
                    playerInRange = false;
                    ableToInspect = false;
                    interactionUI.SetActive(false);
                }

                if(_InspectObject.objectHit)
                {
                    _PlayerController = _InspectObject.GetComponentInParent<PlayerController>();
                    // Debug.Log(_InspectObject.hitInfo.transform.gameObject.name);
                    playerInRange = true;
                }   
            }
        } 
        else if(objectPickedup)
        {
            interactionUI.SetActive(false);
        }
        // else (/* if object is picked up and the player is near the water */)
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

           // _PlayerInteraction.GetComponent<PlayerController>().shake = false;
            _PlayerInteraction.GetComponent<PlayerController>().ableToLookAround = false;
            
           // camera.transform.localPosition = _PlayerController.initialCamPos;

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
