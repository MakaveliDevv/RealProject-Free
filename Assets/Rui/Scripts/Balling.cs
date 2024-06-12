using UnityEngine;

public class Balling : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float floatStrength = 1f;
    public float floatFrequency = 1f;
    public float smoothTime = 0.1f; // Smoothing time for movement

    private Rigidbody rb;
    private Vector3 startPos;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Ensure gravity is disabled
        startPos = transform.position;
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyFloatingEffect();
    }

    void HandleMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 targetVelocity = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, smoothTime);
    }

    void ApplyFloatingEffect()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * floatFrequency) * floatStrength;
        Vector3 targetPosition = new Vector3(transform.position.x, newY, transform.position.z);
        rb.MovePosition(Vector3.Slerp(transform.position, targetPosition, Time.fixedDeltaTime * floatFrequency));
    }
}
