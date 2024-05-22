using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabable : MonoBehaviour
{
    public Interactable _Interactable;
    [SerializeField] private PlayerController _PlayerC;
    public bool inGrabRange;
    public float grabRadius;
    [SerializeField] private bool objectPickedup;


    void Awake() 
    {
        _Interactable = GetComponentInParent<Interactable>();
        if (TryGetComponent<SphereCollider>(out var col))
        {
            col.radius = grabRadius;
        }
        else
        {
            Debug.LogError("SphereCollider is missing.");
        }
    }
    void Update() 
    {
        if (_Interactable != null && _Interactable.inRange) 
        {
            _PlayerC = _Interactable._PlayerContr; // Fetch the PlayerController script from the Interactable script
            if (_PlayerC != null)
            {
                float distance = Vector3.Distance(transform.position, _PlayerC.transform.position);
                if (distance <= grabRadius) 
                {
                    inGrabRange = true;

                    SphereCollider col = _PlayerC.gameObject.GetComponent<SphereCollider>();
                    col.enabled = true;
                    // Debug.Log("Object is within grab range.");
                }
                else
                {
                    SphereCollider col = _PlayerC.gameObject.GetComponent<SphereCollider>();
                    col.enabled = false;
                    inGrabRange = false;
                    // Debug.Log("Object is out of grab range.");
                }
            }
            // else
            // {
            //     Debug.LogWarning("PlayerController is null in Grabable.");
            // }
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

                objectPickedup = true;
                Debug.Log("Object picked up");
            } 
            else 
            {
                parent.transform.SetParent(null);
                objectPickedup = false;
                Debug.Log("Object released");
            }
        }
    }
}
