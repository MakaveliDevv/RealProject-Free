using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public new Camera camera;
    public float rotateSpeed = 6f;

    [SerializeField] private Transform inspectObjectTransform;
    [SerializeField] private PlayerInteraction _PlayerInteraction;

    public bool inspectMode;

    void Awake() 
    {
        camera = GetComponent<Camera>();
    }

    private void Update() 
    {
        if(inspectMode) 
        {
            _PlayerInteraction = GetComponentInParent<PlayerInteraction>();
            inspectObjectTransform = _PlayerInteraction._Inspectable.gameObject.transform;
        }
    }


    public void RotateObject(Vector2 input)
    {
        if (!inspectMode || inspectObjectTransform == null)
            return;

        float deltaX = input.x;
        float deltaY = input.y;

        inspectObjectTransform.rotation *= Quaternion.Euler(deltaY * rotateSpeed, -deltaX * rotateSpeed, 0);
    }


    // ROTATE OBJECT WITH OLD INPUT SYSTEM WITH THE MOUSE

    // public float detectionRadius = 0.5f;  // Radius for the overlap sphere
    // private float deltaRotationX;
    // private float deltaRotationY;
    // private void Update()
    // {
    //     if (inspectMode && Mouse.current.leftButton.wasPressedThisFrame)
    //     {
    //         if (CameraToMouseRay(Mouse.current.position.ReadValue(), out CustomRaycastHit customRayHit))
    //         {
    //             if (customRayHit.transform.CompareTag("Inspectable"))
    //             {
    //                 inspectObjectTransform = customRayHit.transform;
    //                 Debug.Log(customRayHit.transform.gameObject.name);
    //             }
    //         }
    //     }
    // }

    // private struct CustomRaycastHit
    // {
    //     public Transform transform;
    //     public Vector3 point;
    //     public Vector3 normal;
    //     public float distance;
    // }

    // void UpdateRotation() 
    // {
    //     if(!inspectMode)
    //         return;
             
    //     if (Input.GetMouseButtonDown(0))
    //     {
    //         if (CameraToMouseRay(Input.mousePosition, out CustomRaycastHit customRayHit))
    //         {
    //             if (customRayHit.transform.CompareTag("Inspectable"))
    //             {
    //                 inspectObjectTransform = customRayHit.transform;
    //                 Debug.Log(customRayHit.transform.gameObject.name);
    //             }
    //         }
    //     }

    //     deltaRotationX = -Input.GetAxis("Mouse X");
    //     deltaRotationY = Input.GetAxis("Mouse Y");

    //     if (Input.GetMouseButton(1))
    //     {
    //         if (inspectObjectTransform == null)
    //             return;

    //         inspectObjectTransform.rotation =
    //             Quaternion.AngleAxis(deltaRotationX * rotateSpeed, transform.up) *
    //             Quaternion.AngleAxis(deltaRotationY * rotateSpeed, transform.right) *
    //             inspectObjectTransform.rotation;
    //     }
    // }


    // private bool CameraToMouseRay(Vector3 mousePosition, out CustomRaycastHit customRayHit)
    // {
    //     Ray ray = camera.ScreenPointToRay(mousePosition);
    //     Vector3 worldPoint = ray.GetPoint(2f);  // Get a point along the ray a short distance away from the camera

    //     // Use OverlapSphere to detect colliders within a certain radius from the world point around the mouse position
    //     Collider[] colliders = Physics.OverlapSphere(worldPoint, detectionRadius);
    //     foreach (Collider collider in colliders)
    //     {
    //         if (collider.CompareTag("Inspectable"))
    //         {
    //             customRayHit = new CustomRaycastHit
    //             {
    //                 transform = collider.transform,
    //                 point = collider.transform.position,
    //                 normal = Vector3.zero, // Default normal
    //                 distance = Vector3.Distance(worldPoint, collider.transform.position)
    //             };
    //             return true;
    //         }
    //     }

    //     customRayHit = default;
    //     return false;
    // }
}


