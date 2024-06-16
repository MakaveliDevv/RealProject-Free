using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider collider) 
    {
        if(collider.CompareTag("Player")) 
        {
            if(TryGetComponent<PlayerInteraction>(out var playerInteraction))
            {
                Interactable interactable = playerInteraction._Interactable;

                // UI should pop up to be able to drop the object
                // Object need to be dropped, which already can
                // Animation need to be played

            }
        }
    }
   
}
