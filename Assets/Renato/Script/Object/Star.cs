using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Interactable
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private bool insideOrbit;
    // public bool destroyTimerRunning;

    public bool interact;
    public GameObject point;

    void Start() 
    {
        point = GameObject.FindGameObjectWithTag("AssemblePoint");
    }
    
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.CompareTag("Footstep")) 
        {
            interact = true;
            // destroyTimerRunning = false;
            // Debug.Log("Made contact with the footsteps");

            // Fetch the first control point
            // Transform firstControlPoint = OrbMovement.instance.controlPoints[0].transform;
            
            // Move towards the first control point
            StartCoroutine(MoveTowardsControlPoint(point.transform));

            // Indicate that the object is in the gravitational orbit
            insideOrbit = true;
        }
    }

    private IEnumerator MoveTowardsControlPoint(Transform target) 
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;
        while(elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, target.position, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = target.position;
    }
}