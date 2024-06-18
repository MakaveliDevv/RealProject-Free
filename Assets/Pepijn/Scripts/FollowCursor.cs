using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    public Camera mainCamera;
    public float distanceFromCamera = 10f; // Distance from the camera to place the GameObject

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    void Update()
    {
        // Get the mouse position in screen coordinates
        Vector3 mousePosition = Input.mousePosition;

        // Set the z-coordinate to the desired distance from the camera
        mousePosition.z = distanceFromCamera;

        // Convert the screen position to world position
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);

        // Update the GameObject's position
        transform.position = worldPosition;
    }
}
