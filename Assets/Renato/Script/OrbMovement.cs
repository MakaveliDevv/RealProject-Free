using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbMovement : MonoBehaviour
{
    [SerializeField] public List<Transform> controlPoints;
    // [SerializeField] private Transform moveableObject;
    public List<Transform> moveableObjects = new();

    private float interpolateAmount;

    private void Update() {
        interpolateAmount = (interpolateAmount + Time.deltaTime / controlPoints.Count) % 1f;
        int segmentCount = controlPoints.Count;
        float t = interpolateAmount * segmentCount;
        int currentPoint = Mathf.FloorToInt(t);
        t -= currentPoint;

        Vector3 position = CatmullRom(
            controlPoints[(currentPoint - 1 + segmentCount) % segmentCount].position,
            controlPoints[currentPoint % segmentCount].position,
            controlPoints[(currentPoint + 1) % segmentCount].position,
            controlPoints[(currentPoint + 2) % segmentCount].position,
            t);

        // Loop through all the objects in the list
        foreach (Transform moveable in moveableObjects)
        {
            if(moveableObjects.Count > 0) 
                moveable.position = position;
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
}
