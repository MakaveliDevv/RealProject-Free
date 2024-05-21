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


    private bool inRange;
    [SerializeField] private float radius = 3f;

    protected GameObject target; 
    public bool objectGrabbed;
    public PlayerController _PlayerContr;

    private bool canInteract = true;
    private float interactCooldown = 0.2f;


    void Awake() 
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = radius;
        collider.isTrigger = true;
    }

    public virtual void InteractOnCollision() 
    {
        Debug.Log("Interact on collision");
    }

    public virtual void Interact() 
    {
        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("E key pressed");
            if (_ObjectType == ObjectType.GRABABLE) 
            {
                ToggleGrab();
            }
        }
    }

    private void ToggleGrab()
    {
        if (!objectGrabbed) 
        {
            Debug.Log("Grabbing object");
            objectGrabbed = true;
            transform.position = _PlayerContr.objectPos.transform.position;
            transform.SetParent(_PlayerContr.objectPos.transform);
        } 
        else 
        {
            Debug.Log("Releasing object");
            objectGrabbed = false;
            transform.SetParent(null);
        }
    }

    void OnTriggerEnter(Collider collider) 
    {
        collider.TryGetComponent<PlayerController>(out var player);
        _PlayerContr = player;
        if(_PlayerContr != null && !inRange) 
        {
            target = _PlayerContr.gameObject;
            inRange = true;
            InteractOnCollision();
        }
    }

    void OnTriggerExit(Collider collider) 
    {
        // collider.TryGetComponent<PlayerController>(out var player);
        if(_PlayerContr != null && inRange)  
            inRange = false;
        
    }

    void OnTriggerStay(Collider collider) 
    {
        collider.TryGetComponent<PlayerController>(out var player);
        _PlayerContr = player;
        if(_PlayerContr != null && inRange)  
            Interact();
        
    }

    // public virtual void Interact() 
    // {
    //     // Grab the object if possible
    //     if(_ObjectType == ObjectType.GRABABLE && !objectGrabbed && Input.GetKey(KeyCode.E)) 
    //     {
    //         if(objectGrabbed)
    //             return;

    //         objectGrabbed = true;

    //         transform.position = _PlayerContr.objectPos.transform.position;
    //         transform.SetParent(_PlayerContr.objectPos.transform);
    //     }

    //     // Release the object
    //     if(_ObjectType == ObjectType.GRABABLE && objectGrabbed && Input.GetKeyDown(KeyCode.E))
    //     {
    //         if(!objectGrabbed)
    //             return;

    //         objectGrabbed = false;
    //         transform.SetParent(null); // Detach from any parent to be centered independently
    //     }
    // }

    void OnDrawGizmosSelect() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }

}
