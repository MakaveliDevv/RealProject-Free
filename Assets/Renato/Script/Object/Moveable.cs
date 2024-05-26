using UnityEngine;

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
    
    void Start() 
    {
        objectTransform = gameObject.transform;
        rb = GetComponent<Rigidbody>();
    }

    void Update() 
    {
        if(rb != null && isMoving)
            rb.useGravity = false;
    }
}
