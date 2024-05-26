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

    [SerializeField] protected float interactRadius = 3f;
    public bool objectPickedup;
    public bool objectReleased;
    public bool ableToInspect;


    void Awake()
    {       
        _InspectObject = InspectObject.instance;
        if (TryGetComponent<SphereCollider>(out var col))
        {
            col.radius = interactRadius;
            col.isTrigger = true;
        }
    }

    void Update()
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
                _PlayerContr = _InspectObject.GetComponent<PlayerController>();
                playerInRange = true;
            }   
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, interactRadius);
    }
}
