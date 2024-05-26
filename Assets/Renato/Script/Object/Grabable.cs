using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    public Interactable _Interactable;
    public PlayerController _PlayerC;

    public SphereCollider sphereCol;
    private Rigidbody rb;
    
    public bool grabable;
    public float grabRadius;

    public LayerMask groundLayer;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;


    void Awake() 
    {
        _Interactable = GetComponentInParent<Interactable>();
        rb = GetComponentInParent<Rigidbody>();

        if (TryGetComponent<SphereCollider>(out var col))
        {
            if(_Interactable._ObjectType == Interactable.ObjectType.GRABABLE)
            {
                sphereCol = col;
                sphereCol.radius = grabRadius;
            }
            else 
                Destroy(sphereCol);            
        }
    }

    void Update() 
    {
        if(_Interactable.objectPickedup)
        {
            grabable = false;
            _Interactable.playerInRange = false;
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if(!grabable)
            return;
            
        if(collider.TryGetComponent<PlayerController>(out var controller) && _Interactable.playerInRange)
        {
            // Check if the object got hit
            if(_Interactable._InspectObject.objectHit) 
            {
                Debug.Log("Grab distance");
                _PlayerC = controller;
                grabable = true;
            }
        }         
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.TryGetComponent<PlayerController>(out var controller) && _Interactable.playerInRange) 
        {
            grabable = true;   
            _PlayerC = controller;
        }
    }

    void OnTriggerExit(Collider collider) 
    {
        grabable = false;
    }

    public void ToggleGrab()
    {        
        if(_PlayerC != null && !InspectObject.instance.inspectMode) 
        {
            if (!_Interactable.objectPickedup) 
            {
                Grab();
            } 
            else 
            {
                Drop();
            }
        }
    }
    
    public void Grab() 
    {
        if(Inventory.instance.ReturnInventorySpace() && grabable) 
        {
            _Interactable.gameObject.transform.SetParent(_PlayerC.objectPos.transform);
            _Interactable.gameObject.transform.SetPositionAndRotation(_PlayerC.objectPos.transform.position, _PlayerC.objectPos.transform.rotation);
            sphereCol.enabled = false;
            _Interactable.gameObject.GetComponent<SphereCollider>().enabled = false;

            if(rb != null) 
                Destroy(rb);
            
            _Interactable.objectPickedup = true;        
            _Interactable.objectReleased = false;
            Inventory.instance._Grabables.Add(this);
        }
    }

    public void Drop() 
    {
        if(_Interactable.objectPickedup || InspectObject.instance.inspectMode)
        {
            _Interactable.gameObject.GetComponent<SphereCollider>().enabled = true;
            rb = _Interactable.gameObject.AddComponent<Rigidbody>();
            _Interactable.gameObject.transform.SetParent(null);

            // Check if its a floating object or not
            if(_Interactable._GravitationalType != Interactable.GravitationalType.NON_FLOATING) 
                rb.useGravity = false;
            
            else 
                rb.useGravity = true;

            // Start the coroutine to drop the object smoothly
            StartCoroutine(SmoothDrop());

            _Interactable.objectPickedup = false;

            if(Inventory.instance._Grabables.Contains(this))
                Inventory.instance._Grabables.Remove(this);
                

            if(sphereCol != null)
            {
                sphereCol.enabled = true;

                if(sphereCol.radius != grabRadius)
                    sphereCol.radius = grabRadius;
            }
        } 
    }

    private IEnumerator SmoothDrop()
    {
        Vector3 targetPosition = CalculateGroundPosition();
        float distanceToGround = Vector3.Distance(_Interactable.transform.position, targetPosition);

        // While the object is not close to the ground
        while (distanceToGround > 0.1f)
        {
            // Smoothly move the object towards the ground
            _Interactable.transform.position = Vector3.SmoothDamp(
                _Interactable.transform.position, 
                targetPosition, 
                ref velocity, 
                smoothTime
            );

            // Update the distance to the ground
            distanceToGround = Vector3.Distance(_Interactable.transform.position, targetPosition);

            yield return null;
        }

        // Once close to the ground, enable gravity if needed
        // if (_Interactable._GravitationalType != Interactable.GravitationalType.NON_FLOATING)
        // {
        //     rb.useGravity = true;
        // }
    }

    
    private Vector3 CalculateGroundPosition()
    {
        if (Physics.Raycast(_Interactable.transform.position, Vector3.down, out RaycastHit hit, Mathf.Infinity, groundLayer))
            return hit.point;
        
        return _Interactable.transform.position;
    }
    
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position, grabRadius);
    }
}
