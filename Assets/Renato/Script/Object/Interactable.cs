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
    public bool inRange;

    [SerializeField] private float interactRadius = 3f;


    void Awake() 
    {
        if(TryGetComponent<SphereCollider>(out var col)) 
        {
            col.radius = interactRadius;
            col.isTrigger = true;
        }
    }

    public virtual void InteractOnCollision() 
    {
        Debug.Log("Interact on collision");
    }

    public virtual void Interact() 
    {        
        Debug.Log("You can now interact with me");
    }

    void OnTriggerEnter(Collider collider) 
    {
        var playerController = collider.GetComponent<PlayerController>();
        if (playerController != null && !inRange) 
        {
            _PlayerContr = playerController;
            inRange = true;
            InteractOnCollision();
        }
    }

    void OnTriggerExit(Collider collider) 
    {
        if(_PlayerContr != null && inRange)
        {
            inRange = false;
        }  
    }

    void OnTriggerStay(Collider collider) 
    {
        if(_PlayerContr != null)  
            Interact();
        
    }
}
