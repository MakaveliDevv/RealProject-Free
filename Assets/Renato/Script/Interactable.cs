using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public enum InteractableType { INSPECTABLE }
    public InteractableType _InteractableType;
    private bool inRange;
    [SerializeField] private float radius = 3f;

    protected GameObject target; 

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
        // Do something
    }

    // void OnCollisionEnter(Collision collision) 
    // {
    //     if(collision.gameObject.CompareTag("Player") && !inRange) 
    //     {
    //         inRange = true;
    //         Interact();
    //     }
    // }

    // void OnCollisionExit(Collision collision) 
    // {
    //     if(collision.gameObject.CompareTag("Player") && inRange) 
    //     {
    //         inRange = false;
    //     }
    // }

    void OnTriggerEnter(Collider collider) 
    {
        collider.TryGetComponent<PlayerController>(out var player);
        if(player != null && !inRange) 
        {
            target = player.gameObject;
            inRange = true;
            InteractOnCollision();
        }
        // if(collider.CompareTag("Player") && !inRange) 
        // {
        //     inRange = true;
        //     Interact();
        // }
    }

    void OnTriggerExit(Collider collider) 
    {
        collider.TryGetComponent<PlayerController>(out var player);
        if(player != null && inRange)  
        {
            inRange = false;
        }
        // if(collider.CompareTag("Player") && inRange) 
        // {
        //     inRange = false;
        // }
    }

    void OnTriggerStay(Collider collider) 
    {
        collider.TryGetComponent<PlayerController>(out var player);
        if(player != null && inRange)  
        {
            Interact();
        }
    }

    void OnDrawGizmosSelect() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, radius);
    }

}
