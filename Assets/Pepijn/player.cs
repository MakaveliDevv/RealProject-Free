using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    public GameObject objectToPickup, objectHeld;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if ((objectToPickup != objectHeld) && (objectToPickup != null))
            {
                objectToPickup.transform.SetParent(gameObject.transform);
                objectHeld = objectToPickup;
                objectToPickup = null;
            }
            else if (objectHeld != null)
            {
                objectHeld.transform.SetParent(null);
                objectHeld = null;
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "object")
        {
            objectToPickup = collider.gameObject;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "object")
        {
            objectToPickup = null;
        }
    }
}
