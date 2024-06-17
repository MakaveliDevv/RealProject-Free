using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : Interactable
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private bool insideOrbit;
    private Rigidbody rb;
    public Transform objectTransform;
    public Vector3 startingPosition = Vector3.zero;

    public bool interact;
    public GameObject point;
    
    [Header("Bool")]
    [HideInInspector] public float interpolateAmount;
    public bool isMoving;
    public bool hasStartingPosition;
    public bool inGravitationalCircle;
    public bool isInitializing = true; // New flag for initialization phase
    public bool ableToMove;


    void Start() 
    {
        point = GameObject.FindGameObjectWithTag("Sun");
        objectTransform = gameObject.transform;
        rb = GetComponent<Rigidbody>();
    }

    public override void Update() 
    {
        base.Update();

        if(rb != null && isMoving)
            rb.useGravity = false;
    }
    
    
    void OnTriggerEnter(Collider collider) 
    {
        if(collider.CompareTag("Footstep")) 
        {
            interact = true;

            if(ableToMove)
            {
                StartCoroutine(MoveTowardsControlPoint(point.transform));
                
                // Indicate that the object is in the gravitational orbit
                insideOrbit = true;
            }

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
        if(transform.position == target.position)
            ableToMove = false;
    }
}