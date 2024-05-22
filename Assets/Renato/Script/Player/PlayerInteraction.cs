using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Grabable _Grabable; // Reference to the grabable object
    public Inspectable _Inspectable;


    private void OnTriggerEnter(Collider collider) 
    {
        collider.TryGetComponent<Grabable>(out var grabable);
        if (grabable != null) 
        {
            _Grabable = grabable;
            _Inspectable = _Grabable._Interactable.gameObject.GetComponentInParent<Inspectable>();
            Debug.Log(_Grabable + " " + _Inspectable);
        }
        // else
        // {
        //     Debug.LogWarning("Grabable component not found on collider.");
        // }
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, radius);
    // }
}
