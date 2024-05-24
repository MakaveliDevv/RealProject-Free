using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    // Components
    [Header("Components & GameObjects")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;
    public GameObject objectPos;

    // Movement
    [Header("Movement Stuff")]
    public Vector3 inputVector;
    public Vector3 inputDirection;
    public float walkSpeed = 3f; 
    public float sprintSpeed = 6f;
    public float veticalVelocity;
    public float gravityForce;
    [SerializeField] private float stepDistance = 1f;
    private float accumulated_distance;
    private int stepAmount; 
    [SerializeField] private float bobbingAmount = 0.05f; 
    [SerializeField] private float bobbingSpeed = 10f; 
    public bool moving, idle = true, hasStepped;
    public bool allowedToMove, ableToShake, ableToLookAround;

    // Camera
    [Header("Camera Stuff")]
    [HideInInspector] public Vector3 initialCamPos;
    [SerializeField] private float cameraOffset = -.5f;
    [SerializeField] private float cameraSwayAmount = 0.1f;
    [SerializeField] private float cameraSwaySpeed = 2f; 
    [SerializeField] private bool shake;

    // Mouse look
    [Header("Mouse Look")]
    private Vector2 inputLookVector; 
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    
    [SerializeField] private bool cursorLocked;
    public float sphereRadius = 1.5f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        initialCamPos = new(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z);
        Cursor.lockState = CursorLockMode.Locked;
    
        allowedToMove = true;
        ableToShake = true;
        ableToLookAround = true;
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) && Cursor.lockState == CursorLockMode.Locked) 
            Cursor.lockState = CursorLockMode.None;
        
        else if(Input.GetKey(KeyCode.LeftShift) && Input.GetMouseButton(0) && Cursor.lockState == CursorLockMode.None) 
            Cursor.lockState = CursorLockMode.Locked;


        if(Cursor.lockState == CursorLockMode.Locked)
            cursorLocked = true;

        else if(Cursor.lockState == CursorLockMode.None)
            cursorLocked = false;

            
        // CameraShake();
        // MouseLook();

        if(Input.GetKey(KeyCode.Space)) 
            SceneManager.LoadScene("Scene_Renato");

        ApplyGravity();
            
    }

    void FixedUpdate() 
    {
        ApplyGravity();
        CameraShake();
        MouseLook();
        Moving();
    }
    
    public void SetInputVectorMovement(Vector3 direction)
    {
        inputVector = direction;
    }

    public void SetInputLookVector(Vector2 rotation) 
    {
        inputLookVector = rotation;
    }

    void MouseLook()
    {
        // if(!ableToLookAround)
        //     return;
        cam.gameObject.TryGetComponent<InspectObject>(out var inspectObject);
        if(inspectObject.inspectMode)
            return;

        float mouseX = inputLookVector.x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputLookVector.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        if(Cursor.lockState == CursorLockMode.Locked) 
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
        }
        
    }

    void Moving() 
    {
        // if(!allowedToMove) 
        //     return;
        cam.gameObject.TryGetComponent<InspectObject>(out var inspectObject);
        if(inspectObject.inspectMode)
            return;

        inputDirection = new(inputVector.x, 0, inputVector.z);
        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.z).normalized;

        // Apply weighted movement
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed; 

        // Apply movement in world space
        Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);

        controller.Move(speed * Time.deltaTime * worldMoveDirection);

        // Store the current camera local position
        // Vector3 currentCamPos = cam.transform.localPosition;

        if (controller.velocity.sqrMagnitude > 0.01f) // Check if the controller moved
        {
            // Set the camera position to the initial local position
            // cam.transform.localPosition = initialCamPos; 

            moving = true;
            idle = false;
            ableToShake = false;
            shake = false;

            accumulated_distance += controller.velocity.magnitude * Time.deltaTime; // The amount of increased velocity
            if (accumulated_distance > stepDistance) 
            {
                float previousStepAmount = stepAmount; // Store the previous step for reference
                stepAmount += 1; // Increase the step amount

                if (stepAmount >= previousStepAmount) // If true, the player made a step
                {
                    hasStepped = true;

                    // Head bob
                    StartCoroutine(HeadBobbing(initialCamPos, cam)); 
                }

                // Reset accumulated distance
                accumulated_distance = 0f;

                // Reset the camera position
                // cam.transform.localPosition = currentCamPos;           
            }
        } 
        else
        {
            moving = false;
            ableToShake = true;
            idle = true;
        }
    }

    private IEnumerator HeadBobbing(Vector3 currentCamPos, Camera cam)
    {
        if (moving && hasStepped)
        {
            // Define target positions
            Vector3 targetUpPosition = currentCamPos + new Vector3(0f, bobbingAmount, 0f);
            Vector3 targetDownPosition = currentCamPos + new Vector3(0f, -bobbingAmount, 0f);

            // Calculate duration based on bobbingSpeed
            float duration = 1f / bobbingSpeed;

            float startTime = Time.time;

            while (Time.time < startTime + duration)
            {
                float t = (Time.time - startTime) / duration;

                // Interpolate between target positions
                Vector3 newPosition = Vector3.Lerp(targetUpPosition, targetDownPosition, t);

                // Apply movement directly to camera position
                cam.transform.localPosition = newPosition;

                yield return null;
            }

            // Ensure the camera is at the correct final position
            cam.transform.localPosition = targetDownPosition;
        }
    }

    private void CameraShake() 
    {
        // if(!ableToShake)
        //     return;

        cam.gameObject.TryGetComponent<InspectObject>(out var inspectObject);
        if(inspectObject.inspectMode)
            return;

        // Shake camera
        if (idle) 
        {
            ableToShake = true;
            
            float swayOffsetX = Mathf.Sin(Time.time * cameraSwaySpeed) * cameraSwayAmount;
            float swayOffsetY = Mathf.Cos(Time.time * cameraSwaySpeed * 0.5f) * cameraSwayAmount + (transform.position.y + cameraOffset);

            Vector3 cameraSway = new(swayOffsetX, swayOffsetY, 0f);
            cam.transform.localPosition = cameraSway;
            shake = true;
        }
    }

    private void ApplyGravity() 
    {
        veticalVelocity -= gravityForce * Time.deltaTime;

        inputDirection.y = veticalVelocity * Time.deltaTime;

    }
}


