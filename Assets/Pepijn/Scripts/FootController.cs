using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Networking;

public class FootController : NetworkBehaviour
{
    public GameObject leftFoot, rightFoot, centerPoint;
    public GameObject leftStepPrefab, rightStepPrefab;
    public GameObject empty;
    public bool leftToMove;
    [SerializeField] bool isFirstStep, isStepOnCooldown;
    float horizontalInput, currentTime, currentTime2;
    public float rotationSpeed;
    public float stepSize, stepCooldown;
    public FootstepPlayer footstepPlayer;
    bool walking;

    // Vector2 inputVector = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        isFirstStep = true;
        isStepOnCooldown = false;
    }

    // void SetInputVector(Vector2 direction) 
    // {
        // inputVector = direction;
    // }

    public void LeftStep(InputAction.CallbackContext ctx) 
    {
        if(IsOwner && !isStepOnCooldown && ctx.performed)
        {
            Debug.Log(ctx.performed + ": Left step");
            // SetInputVector(ctx.ReadValue<Vector2>());
            WalkForward(true);
        }
    }

    public void RightStep(InputAction.CallbackContext ctx)
    {
        if(IsOwner && !isStepOnCooldown && ctx.performed)
        {
            Debug.Log(ctx.performed + ": Right step");
            // SetInputVector(ctx.ReadValue<Vector2>());
            WalkForward(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // inputDirection = new(inputVector.x, 0f);
        if (IsOwner)
        {
            //Get horizontal input
            horizontalInput = Input.GetAxis("Horizontal");
            if (!isStepOnCooldown)
            {
                //Walk Forward
                if (Input.GetMouseButtonDown(0))
                {
                    WalkForward(true);
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    WalkForward(false);
                }
            }

            //Step cooldown timer
            if (isStepOnCooldown)
            {
                if (currentTime > 0)
                {
                    currentTime -= Time.deltaTime;
                }
                else
                {
                    currentTime = 0;
                    isStepOnCooldown = false;
                }
            }

            if (walking)
            {
                if (currentTime2 > 0)
                {
                    currentTime2 -= Time.deltaTime;
                }
                else
                {
                    currentTime2 = 0;
                    if (ClientScript.instance.clientName == "Wall")
                    {
                        footstepPlayer.StopFootsteps();
                    }
                    walking = false;
                }
            }

            RotateServerRpc(true, centerPoint.transform.position, rotationSpeed * -horizontalInput * Time.deltaTime);
            RotateServerRpc(false, centerPoint.transform.position, rotationSpeed * -horizontalInput * Time.deltaTime);

            // Rotate object1 around the center point
            //RotateAroundCenter(leftFoot, centerPoint.transform.position, rotationSpeed * -horizontalInput * Time.deltaTime);

            // Rotate object2 around the center point
            //RotateAroundCenter(rightFoot, centerPoint.transform.position, rotationSpeed * -horizontalInput * Time.deltaTime);
        }
    }

    void FixedUpdate()
    {

    }

    void StartWalkTimer()
    {
        isStepOnCooldown = true;
        currentTime = stepCooldown;
    }

    void StartWalkingTimer()
    {
        currentTime2 = 0.4f;
    }

    void WalkForward(bool left)
    {
        // Debug.Log("WalkForward called with left = " + left);
        GameObject newEmpty = Instantiate(empty, centerPoint.transform.position, leftFoot.transform.rotation);
        // Debug.Log("New empty GameObject created at position: " + newEmpty.transform.position + " with rotation: " + newEmpty.transform.rotation);

        newEmpty.transform.Translate(0, -stepSize, 0);
        // Debug.Log("New empty GameObject translated to position: " + newEmpty.transform.position);

        // Screen borders
        if (newEmpty.transform.position.x >= -21 && newEmpty.transform.position.x <= 21 && newEmpty.transform.position.y >= -38 && newEmpty.transform.position.y <= 12)
        {
            // Debug.Log("Game Object is within the screen bounds");

            // Debug.Log("leftToMove = " + leftToMove + ", left = " + left);
            if (leftToMove && left)
            {
                walking = true;
                // Debug.Log("leftToMove and left are true, starting walking sequence");

                if (ClientScript.instance.clientName == "Wall")
                {
                    footstepPlayer.PlayFootsteps();
                    // Debug.Log("Footsteps sound played...");
                }

                // Debug.Log("Starting walk and walking timers");
                StartWalkTimer();
                StartWalkingTimer();

                leftToMove = false;
                // Debug.Log("Setting leftToMove to false");
                //leftFoot.transform.Translate(0, stepSize, 0);
                // Debug.Log("Calling CreateFadingStepServerRpc(true)");
                CreateFadingStepServerRpc(true);
                // Debug.Log("Calling WalkForwardServerRpc(true, " + stepSize + ")");
                WalkForwardServerRpc(true, stepSize);
            }
            else if (!left && !leftToMove)
            {
                walking = true;
                // Debug.Log("leftToMove is false and left is false, starting walking sequence");

                if (ClientScript.instance.clientName == "Wall")
                {
                    footstepPlayer.PlayFootsteps();
                    // Debug.Log("Footsteps sound played...");
                }

                // Debug.Log("Starting walk and walking timers");
                StartWalkTimer();
                StartWalkingTimer();

                leftToMove = true;
                // Debug.Log("Setting leftToMove to true");

                if (isFirstStep)
                {
                    isFirstStep = false;
                    // Debug.Log("First step detected, setting isFirstStep to false");
                    //rightFoot.transform.Translate(0, stepSize / 2, 0);
                    // Debug.Log("Calling WalkForwardServerRpc(false, " + (stepSize / 2) + ")");
                    WalkForwardServerRpc(false, stepSize / 2);
                }
                else
                {
                    //rightFoot.transform.Translate(0, stepSize, 0);
                    // Debug.Log("Calling CreateFadingStepServerRpc(false)");
                    CreateFadingStepServerRpc(false);
                    // Debug.Log("Calling WalkForwardServerRpc(false, " + stepSize + ")");
                    WalkForwardServerRpc(false, stepSize);
                }
            }
        }
        else
        {
            Debug.Log("Game Object is outside the screen bounds");
        }

        Destroy(newEmpty);
        // Debug.Log("Destroyed the temporary new empty GameObject");
    }

    // void WalkForward(bool left)
    // {
    //     GameObject newEmpty = Instantiate(empty, centerPoint.transform.position, leftFoot.transform.rotation);
    //     newEmpty.transform.Translate(0, stepSize, 0);

    //     // Screen borders
    //     if (newEmpty.transform.position.x >= -21 && newEmpty.transform.position.x <= 21 && newEmpty.transform.position.y >= -38 && newEmpty.transform.position.y <= 12)
    //     {
    //         Debug.Log("Game Object is within the screen bounds");

    //         if (leftToMove && left)
    //         {
    //             walking = true;
    //             Debug.Log("left to move and left are true");

    //             if (ClientScript.instance.clientName == "Wall")
    //             {
    //                 footstepPlayer.PlayFootsteps();
    //                 Debug.Log("Footsteps sound played...");
    //             }

    //             Debug.Log("Start walk timer and walking timer");
    //             StartWalkTimer();
    //             StartWalkingTimer();

    //             leftToMove = false;
    //             //leftFoot.transform.Translate(0, stepSize, 0);
    //             CreateFadingStepServerRpc(true);
    //             WalkForwardServerRpc(true, stepSize);
    //         }
    //         else if (!left && !leftToMove)
    //         {
    //             walking = true;
    //             if (ClientScript.instance.clientName == "Wall")
    //             {
    //                 footstepPlayer.PlayFootsteps();
    //             }

    //             StartWalkTimer();
    //             StartWalkingTimer();

    //             leftToMove = true;
    //             if (isFirstStep)
    //             {
    //                 isFirstStep = false;
    //                 //rightFoot.transform.Translate(0, stepSize / 2, 0);
    //                 //CreateFadingStepServerRpc(false);
    //                 WalkForwardServerRpc(false, stepSize / 2);
    //             }
    //             else
    //             {
    //                 //rightFoot.transform.Translate(0, stepSize, 0);
    //                 CreateFadingStepServerRpc(false);
    //                 WalkForwardServerRpc(false, stepSize);
    //             }
    //         }
    //     }

    //     Destroy(newEmpty);
    // }

    [ServerRpc]
    private void WalkForwardServerRpc(bool left, float stepSize)
    {
        if (IsServer)
        {
            WalkForwardClientRpc(left, stepSize);
        }
    }

    [ClientRpc]
    private void WalkForwardClientRpc(bool left, float stepSize)
    {
        if (left)
        {
            leftFoot.transform.Translate(0, -stepSize, 0);
            // Debug.Log("LeftFoot step"); 
        }
        else
        {
            rightFoot.transform.Translate(0, -stepSize, 0);
            // Debug.Log("RigtFoot step");
        }
    }

    [ServerRpc]
    private void RotateServerRpc(bool left, Vector3 center, float angle)
    {
        if (IsServer)
        {
            GameObject obj;
            if (left)
            {
                obj = leftFoot;
            }
            else
            {
                obj = rightFoot;
            }
            // Calculate the direction vector from the center point to the object
            Vector3 direction = obj.transform.position - center;
            // Calculate the new direction vector after rotation
            float cosAngle = Mathf.Cos(angle * Mathf.Deg2Rad);
            float sinAngle = Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 newDirection = new(
            direction.x * cosAngle - direction.y * sinAngle,
            direction.x * sinAngle + direction.y * cosAngle,
            direction.z // Z remains unchanged
            );
            Vector3 newPosition = center + newDirection;

            RotateClientRpc(left, center, angle, newPosition);
        }
    }

    [ClientRpc]
    private void RotateClientRpc(bool left, Vector3 center, float angle, Vector3 newPosition)
    {
        GameObject obj;
        if (left)
        {
            obj = leftFoot;
        }
        else
        {
            obj = rightFoot;
        }   

        // Set the new position
        obj.transform.position = newPosition;

        // Calculate the new rotation
        obj.transform.Rotate(0, 0, angle);
    }

    [ServerRpc]
    private void CreateFadingStepServerRpc(bool left)
    {
        if (IsServer)
        {
            Vector3 targetPosition;
            Quaternion targetRotation;
            if (left)
            {
                leftFoot.transform.GetPositionAndRotation(out targetPosition, out targetRotation);
            }
            else
            {
                targetPosition = rightFoot.transform.position;
                targetRotation = rightFoot.transform.rotation;
            }

            CreateFadingStepClientRpc(targetPosition, targetRotation, left);
        }
    }
    [ClientRpc]
    private void CreateFadingStepClientRpc(Vector3 position, Quaternion rotation, bool left)
    {
        GameObject newPrint;
        if (left)
        {
            newPrint = Instantiate(leftStepPrefab, position, rotation);
        }
        else
        {
            newPrint = Instantiate(rightStepPrefab, position, rotation);
        }
    }
}
