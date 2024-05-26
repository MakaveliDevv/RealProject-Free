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
        if (moveableObjects.Count == 0 || controlPoints.Count < 4)
            return;

        for (int i = 0; i < moveableObjects.Count; i++)
        {
            Moveable _Moveable = moveableObjects[i]; // Reference to the Moveable in the list
            // Transform obj = moveableObj.objectTransform; // Reference to the Transform from the Moveable

            // Fetch the Moveable script
            // if (obj.gameObject.TryGetComponent<Moveable>(out var _Moveable)) 
            // {
                // Set the starting position to the first control point when an object is added
                if (!_Moveable.hasStartingPosition)
                {
                    _Moveable.startingPosition = controlPoints[0].position;
                    _Moveable.hasStartingPosition = true;
                    _Moveable.isMoving = true;
                    // moveable.InterpolateAmount = 0; // Ensure InterpolateAmount starts at 0
                    Debug.Log("Starting Position: " + _Moveable.startingPosition);
                }

                // Ensure the object is moving
                if (!_Moveable.isMoving)
                    continue;

                _Moveable.interpolateAmount = (_Moveable.interpolateAmount + Time.deltaTime / controlPoints.Count) % 1f;
                int segmentCount = controlPoints.Count;
                float t = _Moveable.interpolateAmount * segmentCount;
                int currentPoint = Mathf.FloorToInt(t);
                t -= currentPoint;

                Vector3 position = CatmullRom(
                    controlPoints[(currentPoint - 1 + segmentCount) % segmentCount].position,
                    controlPoints[currentPoint % segmentCount].position,
                    controlPoints[(currentPoint + 1) % segmentCount].position,
                    controlPoints[(currentPoint + 2) % segmentCount].position,
                    t);

                _Moveable.objectTransform.position = position;

                // if(_Moveable.objectTransform.position.magnitude > 0.01f) 
                //     _Moveable.inGravitationalCircle = true;

                // Check the distance to other objects and stop if too close
                for (int j = 0; j < moveableObjects.Count; j++)
                {
                    if (i != j)
                    {
                        Moveable otherMoveable = moveableObjects[j];
                        if (Vector3.Distance(_Moveable.objectTransform.position, otherMoveable.objectTransform.position) < minimumDistance)
                        {
                            this._Moveable.isMoving = false;
                            Debug.Log("Object stopped to maintain minimum distance.");
                        }
                    }
                }

                // if(_Moveable.inGravitationalCircle) 
                //     return;

                if(!_Moveable.inGravitationalCircle)
                {
                    // Check if the object has returned to the starting position
                    if (_Moveable.interpolateAmount >= .9f && Vector3.Distance(_Moveable.objectTransform.position, _Moveable.startingPosition) < threshold)
                    {
                        _Moveable.objectTransform.position = _Moveable.startingPosition;
                        _Moveable.isMoving = false;
                        _Moveable.inGravitationalCircle = true;

                        // moveable.InterpolateAmount = 0; // Reset InterpolateAmount to start from beginning next time
                        // objState.HasStartingPosition = false; // Reset for next object addition
                        Debug.Log("Object has returned to the starting position.");
                    }
                }
            // }
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
