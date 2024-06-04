// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class Star : Interactable
// {
//     [SerializeField] float moveableSpeed = 10f;
//     [SerializeField] float duration = 3f;
    
//     void OnTriggerEnter(Collider collider) 
//     {
//         if(collider.CompareTag("Player")) 
//         {
//             Debug.Log("Made contact with the player");

//             // Fetch the first control point
//             Transform firstControlPoint = OrbMovement.instance.controlPoints[0].transform;

//             // Move towards the first control point
//             float t = moveableSpeed / duration;
//             Vector3 direction = Vector3.Lerp(transform.position, firstControlPoint.position, t);

//             // Indicate that the object in the gravitational orbit
//             transform.Translate(direction);
//         }
//     }
// }


using System.Collections;
using UnityEngine;

public class Star : Interactable
{
    [SerializeField] float moveableSpeed = 10f;
    [SerializeField] float duration = 3f;
    
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.CompareTag("Player")) 
        {
            Debug.Log("Made contact with the player");

            // Fetch the first control point
            Transform firstControlPoint = OrbMovement.instance.controlPoints[0].transform;

            // Start moving towards the first control point
            StartCoroutine(MoveTowardsControlPoint(firstControlPoint));
        }
    }

    IEnumerator MoveTowardsControlPoint(Transform target)
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            transform.position = Vector3.Lerp(startPosition, target.position, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the object ends exactly at the target position
        transform.position = target.position;
    }
}
