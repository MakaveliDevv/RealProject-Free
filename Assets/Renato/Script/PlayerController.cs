using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Components
    [Header("Components")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private Camera cam;

    // Movement
    [Header("Movement Stuff")]
    [SerializeField] private float walkSpeed = 3f; 
    [SerializeField] private float sprintSpeed = 6f;
    [SerializeField] private float stepDistance = 1f;
    private float accumulated_distance;
    private int stepAmount; 
    [SerializeField] private float bobbingAmount = 0.05f; 
    [SerializeField] private float bobbingSpeed = 10f; 
    private bool moving, idle = true, hasStepped;

    // Camera
    [Header("Camera Stuff")]
    [SerializeField] private Vector3 initialCamPos;
    [SerializeField] private float cameraOffset = -.5f;
    [SerializeField] private float cameraSwayAmount = 0.1f;
    [SerializeField] private float cameraSwaySpeed = 2f; 
    private bool shake;

    // Mouse look
    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivity = 100f;
    private float xRotation = 0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cam = Camera.main;
        initialCamPos = new(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z);
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        CameraShake();
        MouseLook();
    }

    void FixedUpdate() 
    {
        Moving();
    }

    void MouseLook()
    {
        // Rotate the camera based on mouse movement
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Moving() 
    {
        // Calculate movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0f, verticalInput).normalized;

        // Apply weighted movement
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        // Apply movement in world space
        Vector3 worldMoveDirection = transform.TransformDirection(moveDirection);
        controller.Move(speed * Time.deltaTime * worldMoveDirection);

        // Store the current camera position
        Vector3 currentCamPos = cam.transform.localPosition;

        if (controller.velocity.sqrMagnitude > 0.01f) // Check if the controller moved
        {
            cam.transform.localPosition = initialCamPos; // Set the camera position to the initial position

            moving = true;
            idle = false;
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
                    StartCoroutine(HeadBobbing(currentCamPos, cam)); 
                }

                // Reset accumulated distance
                accumulated_distance = 0f;

                // Reset the camera position
                cam.transform.localPosition = currentCamPos;           
            }
        } 
        else
        {
            moving = false;
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

    void CameraShake() 
    {
        // Shake camera
        if (idle) 
        {
            float swayOffsetX = Mathf.Sin(Time.time * cameraSwaySpeed) * cameraSwayAmount;
            float swayOffsetY = Mathf.Cos(Time.time * cameraSwaySpeed * 0.5f) * cameraSwayAmount + (transform.position.y + cameraOffset);

            Vector3 cameraSway = new Vector3(swayOffsetX, swayOffsetY, 0f);
            cam.transform.localPosition = cameraSway;
            shake = true;
        }
    }
}
