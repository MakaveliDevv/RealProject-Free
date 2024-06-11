using UnityEngine;
using System.Collections;


public class Moveable : Interactable
{
    private Rigidbody rb;
    public Transform objectTransform;
    public Vector3 startingPosition = Vector3.zero;

    [Header("Bool")]
    [HideInInspector] public float interpolateAmount;
    public bool isMoving;
    public bool hasStartingPosition;
    public bool inGravitationalCircle;
    public bool isInitializing = true; // New flag for initialization phase

    [SerializeField] private float duration = 3f;
    [SerializeField] private bool insideOrbit;
    
    void Start() 
    {
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
        if(transform.CompareTag("Star") && collider.CompareTag("Player")) 
        {
            Debug.Log("Made contact with the player");

            // Fetch the first control point
            Transform firstControlPoint = OrbMovement.instance.controlPoints[0].transform;

            // Move towards the first control point
            StartCoroutine(MoveTowardsControlPoint(firstControlPoint));

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
