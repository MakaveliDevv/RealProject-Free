using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveable : Interactable
{
    private GameObject moveableGameObject;
    private OrbMovement _OrbMovement;

    void Start() 
    {
        _OrbMovement = GetComponent<OrbMovement>();
    }

    public override void InteractOnCollision()
    {
        base.InteractOnCollision();

        AddObjectToTheMovement();
    }

    private void AddObjectToTheMovement() 
    {
        // Reference to the PlayerInteraction
        if(_PlayerContr.TryGetComponent<PlayerInteraction>(out var playerInteraction))
        {
            // Then fetch the interactable object
            moveableGameObject = playerInteraction._Interactable.gameObject;
            SphereCollider sphereCollider = moveableGameObject.GetComponent<SphereCollider>();
            Grabable _Grabable = moveableGameObject.GetComponentInChildren<Grabable>();
            
            if(sphereCollider != null) 
                sphereCollider.enabled = true;
            
            if(_Grabable != null)
            {
                _Grabable.sphereCol.enabled = true;
                _Grabable.objectPickedup = false;

                moveableGameObject.transform.SetParent(null);
                
                // Add it to the moveableObjects list
                _OrbMovement.moveableObjects.Add(moveableGameObject.transform);
            }


        }

        // Check if the fetched object is the right object
    }
    
}









