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
    public bool isInitializing = true; // New flag for initialization phase
    
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
}
