using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Grabable _Grabable; // Reference to the grabable object
    public Interactable _Interactable; // Reference to the inspect object
    public bool ableToInspect;


    void OnTriggerStay(Collider collider) 
    {
        if(collider.TryGetComponent<Interactable>(out var interactable) && interactable._InteractableType == Interactable.InteractableType.INSPECTABLE) 
        {
            _Interactable = interactable;
            ableToInspect = true;
        }

        collider.TryGetComponent<Grabable>(out var grabable);
        if (grabable != null) 
        {
            _Grabable = grabable;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        ableToInspect = false;
    }
}
