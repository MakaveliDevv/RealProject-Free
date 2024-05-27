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
    [SerializeField] private List<Transform> controlPoints;
    private List<Moveable> moveableObjects = new();

    [Header("GameObject")]
    [SerializeField] private GameObject moveableGameObject;

    [Header("Float")]
    public float threshold = 0.1f;
    public float minimumDistance = 1.0f; // Minimum distance between objects

    [Header("Bool")]
    [SerializeField] private bool playerInRange;

    private void Update()
    {
        if (moveableObjects.Count == 0 || controlPoints.Count < 4 && !playerInRange)
            return;

        for (int i = 0; i < moveableObjects.Count; i++)
        {
            Moveable moveable = moveableObjects[i];
    
            // Set the starting position to the first control point when an object is added
            if (!moveable.hasStartingPosition)
            {
                moveable.startingPosition = controlPoints[0].position;
                moveable.hasStartingPosition = true;
                moveable.isMoving = true;
                moveable.interpolateAmount = i / (float)moveableObjects.Count % 1f; // Stagger start positions
                Debug.Log("Starting Position: " + moveable.startingPosition);
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
            if (!moveable.inGravitationalCircle && moveable.interpolateAmount >= 0.9f && Vector3.Distance(moveable.objectTransform.position, moveable.startingPosition) < threshold)
            {
                moveable.objectTransform.position = moveable.startingPosition;
                moveable.isMoving = false;
                moveable.inGravitationalCircle = true;
                moveable.interpolateAmount = 0;
                Debug.Log("Object has returned to the starting position.");
            }


            for (int j = 0; j < moveableObjects.Count; j++)
            {
                if (i != j)
                {
                    Moveable otherMoveable = moveableObjects[j];
                    if (Vector3.Distance(moveable.objectTransform.position, otherMoveable.objectTransform.position) < minimumDistance)
                    {
                        moveable.isMoving = false;
                        Debug.Log("Object stopped to maintain minimum distance.");
                        break; // Exit the loop as soon as we stop the object
                    }
                }
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
                        }
                    }
                }
            }
        }
    }
}
