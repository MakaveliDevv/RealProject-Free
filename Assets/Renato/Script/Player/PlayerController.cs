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
    public InspectObject _InspectObject;

    // Movement
    [Header("Movement Stuff")]
    public float walkSpeed = 3f; 
    public float sprintSpeed = 6f;
    public float jumpForce = 3.5f;
    public float gravityForce = 9.81f;
    
    public Vector3 inputVector;
    public Vector3 inputDirection;
    public float verticalVelocity;
    public bool jumping;

    [SerializeField] private float stepDistance = 1f;
    private float accumulated_distance;
    private int stepAmount; 
    [SerializeField] private float bobbingAmount = 0.05f; 
    [SerializeField] private float bobbingSpeed = 10f; 
    public bool moving, idle = true, hasStepped;
    // public bool allowedToMove, ableToShake; 
    public bool ableToLookAround;

    // Camera
    [Header("Camera Stuff")]
    public Vector3 initialCamPos;
    [SerializeField] private float cameraOffset = -.5f;
    [SerializeField] private float cameraSwayAmount = 0.1f;
    [SerializeField] private float cameraSwaySpeed = 2f; 
    public bool shake;
    private bool lockstate;

    // Mouse look
    [Header("Mouse Look")]
    private Vector2 inputLookVector; 
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0f;
    
    [SerializeField] private bool cursorLocked;
    public float sphereRadius = 1.5f;
  

    void Awake()
    {
        cam = Camera.main;

        controller = GetComponent<CharacterController>();
        _InspectObject = cam.gameObject.GetComponent<InspectObject>();

        //initialCamPos = new(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z);
        Cursor.lockState = CursorLockMode.Locked;

        ableToLookAround = true;
    }

    void FixedUpdate() 
    {
        CameraShake();
        if(!_InspectObject.inspectMode) 
        {
            LookAround();
            Moving();
        }
    }

    public void ReloadScene() 
    {
        SceneManager.LoadScene("Scene_Renato");
    }

    public void LockUnlockState() 
    {
        if(Cursor.lockState == CursorLockMode.Locked) 
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else if(Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    
    public void SetInputVectorMovement(Vector3 direction)
    {
        inputVector = direction;
    }

    public void SetInputLookVector(Vector2 rotation) 
    {
        inputLookVector = rotation;
    }

    void LookAround()
    {
        float mouseX = inputLookVector.x * mouseSensitivity * Time.deltaTime;
        float mouseY = inputLookVector.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        if(Cursor.lockState == CursorLockMode.Locked) 
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            transform.Rotate(Vector3.up * mouseX);
            ableToLookAround = true;
        }
        
    }

    void Moving()
    {
        cam.gameObject.TryGetComponent<InspectObject>(out var inspectObject);
        if(inspectObject.inspectMode)
            return;        
            
        ApplyGravity();

        inputDirection = new Vector3(inputVector.x, 0f, inputVector.z);
        Vector3 moveDirection = new Vector3(inputDirection.x, 0f, inputDirection.z).normalized;

        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;
        Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
        Vector3 finalMoveDirection = worldMoveDirection + new Vector3(0, verticalVelocity, 0);

        controller.Move(speed * Time.deltaTime * finalMoveDirection);

        Vector3 currentCamPos = cam.transform.localPosition;

        if (controller.velocity.sqrMagnitude > 0.01f)
        {
            //cam.transform.localPosition = initialCamPos;

            accumulated_distance += controller.velocity.magnitude * Time.deltaTime;
            if (accumulated_distance > stepDistance)
            {
                float previousStepAmount = stepAmount;
                stepAmount += 1;

                if (stepAmount >= previousStepAmount)
                {
                    idle = false;
                    //shake = false;
                    moving = true;

                    StartCoroutine(HeadBobbing(initialCamPos, cam));
                }

                accumulated_distance = 0f;
                cam.transform.localPosition = currentCamPos;
            }
        }
        else
        {
            moving = false;
            //shake = true;
            idle = true;
        }
    }

    private IEnumerator HeadBobbing(Vector3 currentCamPos, Camera cam)
    {
        if (moving)
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
        cam.gameObject.TryGetComponent<InspectObject>(out var inspectObject);
        if(inspectObject.inspectMode)
            return;

        // Shake camera
        if (idle) 
        {            
            float swayOffsetX = Mathf.Sin(Time.time * cameraSwaySpeed) * cameraSwayAmount;
            float swayOffsetY = Mathf.Cos(Time.time * cameraSwaySpeed * 0.5f) * cameraSwayAmount + (transform.position.y + cameraOffset);

            Vector3 cameraSway = new(swayOffsetX, swayOffsetY, 0f);
            cam.transform.localPosition = cameraSway;
            shake = true;
        }
    }

    public void Jump() 
    {
        if(controller.isGrounded) 
        {
            verticalVelocity = jumpForce;
            jumping = true;
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && verticalVelocity <= 0)
        {
            verticalVelocity = -gravityForce * Time.deltaTime;
            jumping = false;
        }
        else
        {
            verticalVelocity -= gravityForce * Time.deltaTime;
        }
    }
}


