using System.Collections.Generic;
using UnityEngine;

public class OrbMovement : MonoBehaviour
{
    [Header("Scripts")]
    [SerializeField] private PlayerController _PlayerController;
    [SerializeField] private PlayerInteraction _PlayerInteraction;
    [SerializeField] private Grabable _Grabable;
    [SerializeField] private Moveable _Moveable;

    [Header("List")]
    [SerializeField] public List<Transform> controlPoints;
    [SerializeField] private List<Moveable> moveableObjects = new();

    [Header("GameObject")]
    [SerializeField] private GameObject moveableGameObject;

    [Header("Float")]
    public float threshold = 0.1f;
    public float minimumDistance = 1.0f; // Minimum distance between objects

    [Header("Bool")]
    [SerializeField] private bool playerInRange;
    private Vector3 offsetDirection;
    public float offsetDistance;

    #region Singleton
    public static OrbMovement instance;
    void Awake() 
    {
        instance = this;
    }
    
    #endregion
    // private void Update()
    // {
    //     if (moveableObjects.Count == 0 || controlPoints.Count < 4 && !playerInRange)
    //         return;

    //     for (int i = 0; i < moveableObjects.Count; i++)
    //     {
    //         // Assign the added object
    //         Moveable moveable = moveableObjects[i];

    //         // Set the starting position to the first control point when an object is added
    //         if (!moveable.hasStartingPosition)
    //         {
    //             if(i == 0)
    //             {
    //                 moveable.startingPosition = controlPoints[0].position;  
    //                 Debug.Log($"Object {i}");
    //                 Debug.Log(moveable.startingPosition);
    //             }
    //             else if(i > 0)
    //             {
    //                 Vector3 offsetDirectionNormalized = (controlPoints[1].position - controlPoints[0].position).normalized;
    //                 Vector3 offsetStartingPosition = controlPoints[0].position + offsetDistance * offsetDirectionNormalized;
    //                 moveable.startingPosition = offsetStartingPosition;
    //                 Debug.Log($"Object {i}");
    //                 Debug.Log(moveable.startingPosition);
    //             }

    //             moveable.hasStartingPosition = true;
    //             moveable.isMoving = true;
    //             moveable.interpolateAmount = 0; // Start all objects with 0 interpolate amount
    //             moveable.isInitializing = true; // Mark the object as initializing
    //             // Debug.Log($"Object {i} Starting Position Set: {moveable.startingPosition}, Interpolate Amount: {moveable.interpolateAmount}");
    //         }

    //         // Ensure the object is moving
    //         if (!moveable.isMoving)
    //             continue;

    //         moveable.interpolateAmount = (moveable.interpolateAmount + Time.deltaTime / controlPoints.Count) % 1f;
    //         int segmentCount = controlPoints.Count;
    //         float t = moveable.interpolateAmount * segmentCount;
    //         int currentPoint = Mathf.FloorToInt(t);
    //         t -= currentPoint;

    //         Vector3 position = CatmullRom(
    //             controlPoints[(currentPoint - 1 + segmentCount) % segmentCount].position,
    //             controlPoints[currentPoint % segmentCount].position,
    //             controlPoints[(currentPoint + 1) % segmentCount].position,
    //             controlPoints[(currentPoint + 2) % segmentCount].position,
    //             t);

    //         moveable.objectTransform.position = position;

    //         // Check if the object has returned to the starting position
    //         if (!moveable.inGravitationalCircle && moveable.interpolateAmount >= 0.9f && Vector3.Distance(moveable.objectTransform.position, moveable.startingPosition) <= threshold)
    //         {
    //             moveable.objectTransform.position = moveable.startingPosition;
    //             moveable.isMoving = false;
    //             moveable.inGravitationalCircle = true;
    //             moveable.interpolateAmount = 0;
    //             // Debug.Log($"Object {i} has returned to the starting position.");
    //         }

    //         // Maintain minimum distance between objects
    //         if(!moveable.isInitializing)
    //         {
    //             for (int j = 0; j < moveableObjects.Count; j++)
    //             {
    //                 if (i != j)
    //                 {
    //                     Moveable otherMoveable = moveableObjects[j];
    //                     if (Vector3.Distance(moveable.objectTransform.position, otherMoveable.objectTransform.position) <= minimumDistance)
    //                     {
    //                         moveable.isMoving = false;
    //                         // Debug.Log($"Object {i} stopped to maintain minimum distance from Object {j}.");
    //                         break; // Exit the loop as soon as we stop the object
    //                     }
    //                 }
    //             }
    //         }

    //         // Switch off the initializing phase after the object has moved away from the starting position
    //         if (moveable.isInitializing && Vector3.Distance(moveable.objectTransform.position, moveable.startingPosition) > minimumDistance)
    //         {
    //             moveable.isInitializing = false;
    //             // Debug.Log($"Object {i} initialization phase ended.");
    //         }
    //     }
    // }

    private void Update()
    {
        if (moveableObjects.Count == 0 || (controlPoints.Count < 4 && !playerInRange))
            return;

        for (int i = 0; i < moveableObjects.Count; i++)
        {
            // Assign the added object
            Moveable moveable = moveableObjects[i];

            // Set the starting position to the first control point when an object is added
            if (!moveable.hasStartingPosition)
            {
                if (i == 0)
                {
                    moveable.startingPosition = controlPoints[0].position;  
                    Debug.Log($"Object {i} starting position: {moveable.startingPosition}");
                }
                else
                {
                    Vector3 offsetDirectionNormalized = (controlPoints[1].position - controlPoints[0].position).normalized;
                    Vector3 offsetStartingPosition = controlPoints[0].position + offsetDistance * offsetDirectionNormalized;
                    moveable.startingPosition = offsetStartingPosition;
                    Debug.Log($"Object {i} offset direction normalized: {offsetDirectionNormalized}");
                    Debug.Log($"Object {i} offset starting position: {moveable.startingPosition}");
                }

                moveable.hasStartingPosition = true;
                moveable.isMoving = true;
                moveable.interpolateAmount = 0; // Start all objects with 0 interpolate amount
                moveable.isInitializing = true; // Mark the object as initializing
                moveable.objectTransform.position = moveable.startingPosition; // Set the initial position
            }

            // Ensure the object is moving
            if (!moveable.isMoving)
                continue;

            moveable.interpolateAmount = (moveable.interpolateAmount + Time.deltaTime / controlPoints.Count) % 1f;
            int segmentCount = controlPoints.Count;
            float t = moveable.interpolateAmount * segmentCount;
            int currentPoint = Mathf.FloorToInt(t);
            t -= currentPoint;

            Vector3 position = CatmullRom(
                controlPoints[(currentPoint - 1 + segmentCount) % segmentCount].position,
                controlPoints[currentPoint % segmentCount].position,
                controlPoints[(currentPoint + 1) % segmentCount].position,
                controlPoints[(currentPoint + 2) % segmentCount].position,
                t);

            moveable.objectTransform.position = position;

            // Check if the object has returned to the starting position
            if (!moveable.inGravitationalCircle && moveable.interpolateAmount >= 0.9f && Vector3.Distance(moveable.objectTransform.position, moveable.startingPosition) <= threshold)
            {
                moveable.objectTransform.position = moveable.startingPosition;
                moveable.isMoving = false;
                moveable.inGravitationalCircle = true;
                moveable.interpolateAmount = 0;
                Debug.Log($"Object {i} has returned to the starting position.");
            }

            // Maintain minimum distance between objects
            if (!moveable.isInitializing)
            {
                for (int j = 0; j < moveableObjects.Count; j++)
                {
                    if (i != j)
                    {
                        Moveable otherMoveable = moveableObjects[j];
                        if (Vector3.Distance(moveable.objectTransform.position, otherMoveable.objectTransform.position) <= minimumDistance)
                        {
                            moveable.isMoving = false;
                            Debug.Log($"Object {i} stopped to maintain minimum distance from Object {j}.");
                            break; // Exit the loop as soon as we stop the object
                        }
                    }
                }
            }

            // Switch off the initializing phase after the object has moved away from the starting position
            if (moveable.isInitializing && Vector3.Distance(moveable.objectTransform.position, moveable.startingPosition) > minimumDistance)
            {
                moveable.isInitializing = false;
                Debug.Log($"Object {i} initialization phase ended.");
            }
        }
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<PlayerController>(out var playerController))
        {
            _PlayerController = playerController;
            playerInRange = true;

            Interact();
        }
    }

    private void Interact()
    {
        if (playerInRange)
        {
            if (_PlayerController.TryGetComponent<PlayerInteraction>(out var playerInteraction))
            {
                _PlayerInteraction = playerInteraction;

                // Fetch the player interaction script to initialize the gameobject
                moveableGameObject = playerInteraction._Interactable.gameObject;

                if (moveableGameObject.TryGetComponent<SphereCollider>(out var sphereCollider))
                    sphereCollider.enabled = true;

                Grabable grabable = moveableGameObject.GetComponentInChildren<Grabable>();
                _Grabable = grabable;
                if (_Grabable != null)
                {
                    _Grabable.sphereCol.enabled = true;
                    moveableGameObject.GetComponent<Interactable>().objectPickedup = false;
                    moveableGameObject.transform.SetParent(null);

                    // Add it to the moveableObjects list
                    if (!moveableObjects.Exists(m => m.objectTransform == moveableGameObject.transform))
                    {
                        moveableGameObject.TryGetComponent<Moveable>(out var moveable);
                        if (moveable != null) // Ensure the object has a Moveable component
                        {
                            moveableObjects.Add(moveable);
                            Inventory.instance._Grabables.Clear();
                            Debug.Log($"Added new Moveable Object. Total count: {moveableObjects.Count}");
                        }
                    }
                }
            }
        }
    }

    // private void Interact()
    // {
    //     if (playerInRange)
    //     {
    //         if (_PlayerController.TryGetComponent<PlayerInteraction>(out var playerInteraction))
    //         {
    //             _PlayerInteraction = playerInteraction;

    //             // Fetch the player interaction script to initialize the gameobject
    //             moveableGameObject = playerInteraction._Interactable.gameObject;

    //             if (moveableGameObject.TryGetComponent<SphereCollider>(out var sphereCollider))
    //                 sphereCollider.enabled = true;

    //             Grabable grabable = moveableGameObject.GetComponentInChildren<Grabable>();
    //             _Grabable = grabable;
    //             if (_Grabable != null)
    //             {
    //                 _Grabable.sphereCol.enabled = true;
    //                 moveableGameObject.GetComponent<Interactable>().objectPickedup = false;
    //                 moveableGameObject.transform.SetParent(null);

    //                 // Add it to the moveableObjects list
    //                 if (!moveableObjects.Exists(m => m.objectTransform == moveableGameObject.transform))
    //                 {
    //                     moveableGameObject.TryGetComponent<Moveable>(out var moveable);
    //                     if (moveable != null) // Ensure the object has a Moveable component
    //                     {
    //                         moveableObjects.Add(moveable);
    //                         Inventory.instance._Grabables.Clear();
    //                     }
    //                 }
    //             }
    //         }
    //     }
    // }
}
