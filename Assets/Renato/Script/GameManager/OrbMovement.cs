using System.Collections.Generic;
using UnityEngine;

public class OrbMovement : MonoBehaviour
{
    [Header("List")]
    public List<Transform> controlPoints;
    [SerializeField] private List<Star> stars = new();

    [Header("GameObject")]
    [SerializeField] private GameObject moveableGameObject;

    [Header("Float")]
    public float threshold = 0.1f;
    public float minimumDistance = 15.0f; // Minimum distance between objects

    [Header("Bool")]
    public float offsetDistance;

    #region Singleton
    public static OrbMovement instance;
    void Awake() 
    {
        instance = this;
    }
    
    #endregion

    private void Update()
    {
        if (stars.Count == 0 || (controlPoints.Count < 4))
            return;

        MoveObject();  
    }

    private void MoveObject() 
    {
        for (int i = 0; i < stars.Count; i++)
        {
            int stopIndex = controlPoints.Count - 1 - i;
            Star star = stars[i];

            if (!star.hasStartingPosition)
            {
                // Calculate the index of the control point to stop at
                stopIndex = Mathf.Clamp(stopIndex, 0, controlPoints.Count - 1); // Ensure within bounds
                Debug.Log(stopIndex);

                star.startingPosition = controlPoints[stopIndex].position;
                Debug.Log(star.startingPosition);
                star.hasStartingPosition = true;
                star.isMoving = true;
                star.interpolateAmount = 0;
                star.isInitializing = true;
                star.objectTransform.position = star.startingPosition;
            }

            if (!star.isMoving)
                continue;

            star.interpolateAmount = (star.interpolateAmount + Time.deltaTime / controlPoints.Count) % 1f;
            int segmentCount = controlPoints.Count;
            float t = star.interpolateAmount * segmentCount;
            int currentPoint = Mathf.FloorToInt(t);
            t -= currentPoint;

            Vector3 position = CatmullRom(
                controlPoints[(currentPoint - 1 + segmentCount) % segmentCount].position,
                controlPoints[currentPoint % segmentCount].position,
                controlPoints[(currentPoint + 1) % segmentCount].position,
                controlPoints[(currentPoint + 2) % segmentCount].position,
                t);

            star.objectTransform.position = position;

            // Maintain minimum distance between objects
            // if (!star.isInitializing)
            // {
            //     for (int j = 0; j < stars.Count; j++)
            //     {
            //         if (i != j)
            //         {
            //             Star otherStar = stars[j];
            //             if (Vector3.Distance(star.objectTransform.position, otherStar.objectTransform.position) <= minimumDistance)
            //             {
            //                 star.isMoving = false;
            //                 break;
            //             }
            //         }
            //     }
            // }

            // Switch off the initializing phase after the object has moved away from the starting position
            if (star.isInitializing && Vector3.Distance(star.objectTransform.position, star.startingPosition) > minimumDistance)
            {
                star.isInitializing = false;
            }
        }

            // // Check if the object has returned to the starting position
            // if (!star.inGravitationalCircle && star.interpolateAmount >= 0.9f && Vector3.Distance(star.objectTransform.position, star.startingPosition) <= threshold)
            // {
            //     star.objectTransform.position = star.startingPosition;
            //     star.isMoving = false;
            //     star.inGravitationalCircle = true;
            //     star.interpolateAmount = 0;
            //     // Debug.Log($"Object {i} has returned to the starting position.");
            // }

            // // Maintain minimum distance between objects
            // if (!star.isInitializing)
            // {
            //     for (int j = 0; j < stars.Count; j++)
            //     {
            //         if (i != j)
            //         {
            //             Star otherMoveable = stars[j];
            //             if (Vector3.Distance(star.objectTransform.position, otherMoveable.objectTransform.position) <= minimumDistance)
            //             {
            //                 star.isMoving = false;
            //                 // Debug.Log($"Object {i} stopped to maintain minimum distance from Object {j}.");
            //                 break; // Exit the loop as soon as we stop the object
            //             }
            //         }
            //     }
            // }

            // Switch off the initializing phase after the object has moved away from the starting position
            // if (star.isInitializing && Vector3.Distance(star.objectTransform.position, star.startingPosition) > minimumDistance)
            // {
            //     star.isInitializing = false;
            //     // Debug.Log($"Object {i} initialization phase ended.");
            // }
        // }
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
        if(collider.CompareTag("Star")) 
        {
            Debug.Log("Found the star");
            if(collider.TryGetComponent<Star>(out var star))
            {
                stars.Add(star);
            }
        } 
    }
}
