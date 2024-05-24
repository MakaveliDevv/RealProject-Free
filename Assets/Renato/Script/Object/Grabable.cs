using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    public Interactable _Interactable;
    [SerializeField] private PlayerController _PlayerC;
    [SerializeField] private SphereCollider sphereCol;
    [SerializeField] private Rigidbody rb;
    
    public bool inGrabRange;
    public bool objectPickedup;
    public float grabRadius;

    public LayerMask groundLayer;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;


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
    
    void OnTriggerEnter(Collider collider)
    {
        if(collider.TryGetComponent<PlayerController>(out var controller))
        {
            inGrabRange = true;
            Debug.Log("Player is within grab range.");
            _PlayerC = controller;
        }         
    }

    void OnTriggerExit(Collider collider) 
    {
        inGrabRange = false;
    }

    public void ToggleGrab()
    {
        Inspectable _Inspectable = _Interactable.GetComponent<Inspectable>();
        // GameObject parent = _Interactable.gameObject; // Initialize main object
        
        if(_PlayerC != null && !_Inspectable._InspectObject.inspectMode) 
        {
            if (!objectPickedup) 
            {
                Grab();
            } 
            else 
            {
                Release();
            }
        }
    }
    
    public void Grab() 
    {
        _Interactable.gameObject.transform.SetParent(_PlayerC.objectPos.transform);
        _Interactable.gameObject.transform.position = _PlayerC.objectPos.transform.position;

        sphereCol.enabled = false;
        _Interactable.gameObject.GetComponent<SphereCollider>().enabled = false;

        if(rb != null) 
            Destroy(rb);
        
        objectPickedup = true;
        Debug.Log("Object picked up");
    }

    public void Release() 
    {
        sphereCol.enabled = true;
        _Interactable.gameObject.GetComponent<SphereCollider>().enabled = true;
        _Interactable.gameObject.transform.SetParent(null);
        rb = _Interactable.gameObject.AddComponent<Rigidbody>();

        // Check if its a floating object or not
        if(_Interactable._GravitationalType != Interactable.GravitationalType.NON_FLOATING) 
        {
            // If floating then float
            rb.useGravity = false;
        }

        // Start the coroutine to drop the object smoothly
        StartCoroutine(SmoothDrop());

        
        objectPickedup = false;
        Debug.Log("Object released");
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
        RaycastHit hit;
        if (Physics.Raycast(_Interactable.transform.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
        {
            return hit.point;
        }

        return _Interactable.transform.position;
    }
    
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, grabRadius);
    }
}
