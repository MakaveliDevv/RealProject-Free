using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;

public class FootController : NetworkBehaviour
{
    public GameObject leftFoot, rightFoot, centerPoint;
    public GameObject leftStepPrefab, rightStepPrefab;
    public bool leftToMove;
    [SerializeField] bool isFirstStep, isStepOnCooldown;
    float horizontalInput, currentTime, currentTime2;
    public float rotationSpeed;
    public float stepSize, stepCooldown;
    public FootstepPlayer footstepPlayer;
    bool walking;


    // Start is called before the first frame update
    void Start()
    {
        isFirstStep = true;
        isStepOnCooldown = false;
    }

    // Update is called once per frame
    void Update()
    {
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
        if (leftToMove && left)
        {
            Vector3 step = leftFoot.transform.forward * stepSize * -1.5f; // Adjust the multiplier as needed
            Vector3 newPosition = leftFoot.transform.position + step;
            //if ((leftFoot.transform.position + new Vector3(stepSize * -1.5f, 0, 0)).x >= -21 && (leftFoot.transform.position + new Vector3(stepSize * 1.5f, 0, 0)).x <= 21)
            if (newPosition.x >= -21 && newPosition.x <= 21)
            {
                walking = true;
                if (ClientScript.instance.clientName == "Wall")
                {
                    footstepPlayer.PlayFootsteps();
                }

                StartWalkTimer();
                StartWalkingTimer();

                leftToMove = false;
                //leftFoot.transform.Translate(0, stepSize, 0);
                CreateFadingStepServerRpc(true);
                WalkForwardServerRpc(true, stepSize);
            }
        }
        else if (!left && !leftToMove)
        {
            walking = true;
            if (ClientScript.instance.clientName == "Wall")
            {
                footstepPlayer.PlayFootsteps();
            }

            StartWalkTimer();
            StartWalkingTimer();

            leftToMove = true;
            if (isFirstStep)
            {
                isFirstStep = false;
                //rightFoot.transform.Translate(0, stepSize / 2, 0);
                CreateFadingStepServerRpc(false);
                WalkForwardServerRpc(false, stepSize / 2);
            }
            else
            {
                //rightFoot.transform.Translate(0, stepSize, 0);
                CreateFadingStepServerRpc(false);
                WalkForwardServerRpc(false, stepSize);
            }
        }
    }

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
            leftFoot.transform.Translate(0, stepSize, 0);
        }
        else
        {
            rightFoot.transform.Translate(0, stepSize, 0);
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
            Vector3 newDirection = new Vector3(
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
                targetPosition = leftFoot.transform.position;
                targetRotation = leftFoot.transform.rotation;
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
        if (left)
        {
            Instantiate(leftStepPrefab, position, rotation);
        }
        else
        {
            Instantiate(rightStepPrefab, position, rotation);
        }
    }
}
