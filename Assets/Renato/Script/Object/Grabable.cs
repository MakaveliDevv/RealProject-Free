using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    public Interactable _Interactable;
    [SerializeField] private PlayerController _PlayerC;
    [SerializeField] private SphereCollider sphereCol;
    
    public bool inGrabRange;
    [SerializeField] private bool objectPickedup;
    public float grabRadius;


    void Awake() 
    {
        _Interactable = GetComponentInParent<Interactable>();
        if (TryGetComponent<SphereCollider>(out var col))
        {
            sphereCol = col;
            sphereCol.radius = grabRadius;
        }
        else
        {
            Debug.LogWarning("SphereCollider is missing.");
        }
    }
    // void Update() 
    // {
    //     if (_Interactable != null && _Interactable.inRange && _PlayerC != null) 
    //     {
    //         _PlayerC = _Interactable._PlayerContr; // Fetch the PlayerController script from the Interactable script
    //         float distance = Vector3.Distance(transform.position, _PlayerC.transform.position);
    //         Debug.Log(distance);

    //         if (distance <= grabRadius) 
    //         {
    //             inGrabRange = true;
    //             sphereCol.enabled = true;
    //             Debug.Log("Object is within grab range.");
    //         }
    //         else
    //         {
    //             sphereCol.enabled = false;
    //             inGrabRange = false;
    //             // Debug.Log("Object is out of grab range.");
    //         }
            
    //         // else
    //         // {
    //         //     Debug.LogWarning("PlayerController is null in Grabable.");
    //         // }
    //     }
    // }
    
    void OnTriggerEnter(Collider collider)
    {
        inGrabRange = true;
        Debug.Log("Object is within grab range.");
        
        if(collider.TryGetComponent<PlayerController>(out var controller))
        {
            _PlayerC = controller;
        }         
    }

    void OnTriggerExit(Collider collider) 
    {
        inGrabRange = false;
    }

    public void ToggleGrab()
    {
        if(_PlayerC != null) 
        {
            GameObject parent = _Interactable.gameObject; // Initialize main object
            if (!objectPickedup) 
            {
                parent.transform.SetParent(_PlayerC.objectPos.transform);
                parent.transform.position = _PlayerC.objectPos.transform.position;

                // Disable colliders
                sphereCol.enabled = false;
                _Interactable.gameObject.GetComponent<SphereCollider>().enabled = false;

                objectPickedup = true;
                Debug.Log("Object picked up");
            } 
            else 
            {
                sphereCol.enabled = true;
                _Interactable.gameObject.GetComponent<SphereCollider>().enabled = true;
                parent.transform.SetParent(null);
                
                objectPickedup = false;
                Debug.Log("Object released");
            }
        }
    }

    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, grabRadius);
    }
}
