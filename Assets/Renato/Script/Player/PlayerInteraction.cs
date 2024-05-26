using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Grabable _Grabable; // Reference to the grabable object
    public Interactable _Interactable; // Reference to the inspect object
    public InspectObject _InspectObject;

    void Awake()
    {
        _InspectObject = GetComponentInChildren<InspectObject>();
    } 

    void Update() 
    {
        if(Inventory.instance._Grabables.Count <= 0) 
        {
            // If object hit
            if(_InspectObject.objectHit)
            {
                GameObject hitObj = _InspectObject.hitInfo.transform.gameObject;
                if(hitObj.TryGetComponent<Interactable>(out var interactable))
                {
                    if(!interactable.objectPickedup) 
                    {
                        _Interactable = interactable;
                        _Interactable.ableToInspect = true;
                    }
                }

                // If grabable fetch the script
                if (_Interactable._ObjectType == Interactable.ObjectType.GRABABLE)
                    _Grabable = _Interactable.gameObject.GetComponentInChildren<Grabable>();
            }
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if(_Interactable != null)
            _Interactable.ableToInspect = false;
    }
}
