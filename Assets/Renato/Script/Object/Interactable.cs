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

    private SphereCollider col;

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
        // else if (_PlayerContr == null)
        // {
        //     Debug.LogWarning("PlayerController not found on collider.");
        // }
        // else
        // {
        //     Debug.LogWarning("Already in range.");
        // }
    }

    void OnTriggerExit(Collider collider) 
    {
        if(_PlayerContr != null && inRange)  
            inRange = false;
        
    }

    void OnTriggerStay(Collider collider) 
    {
        if(_PlayerContr != null)  
            Interact();
        
    }


    // If the object is an inspectable type
    

    // What happends when inspect is done?

    // Either choose to grab the object again
    // Or to release it

    // If grab was the option
    // Then check if the object is an grabable object, then grab
    // If not then show, that this object is ungrabable
    

    

    // void OnDrawGizmosSelect() 
    // {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawSphere(transform.position, interactRadius);
    // }

}
