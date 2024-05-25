using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public new Camera camera;
    public float rotateSpeed = 200f;
    public float smoothingFactor = 8f;
    public float detectionRadius = 1f; 

    public Transform inspectObjectTransform;
    [SerializeField] private PlayerInteraction _PlayerInteraction;

    public bool inspectMode;
    private Vector2 inputRotateVector, targetRotateVector;

    private struct CustomRaycastHit
    {
        public Transform transform;
        public Vector3 point;
        public Vector3 normal;
        public float distance;
    }
    

    void Awake() 
    {
        camera = GetComponent<Camera>();
        _PlayerInteraction = GetComponentInParent<PlayerInteraction>();
    }

    private void Update() 
    {
        // To rotate an inspectable object
        if(_PlayerInteraction != null && _PlayerInteraction.ableToInspect) 
        {
            if(_PlayerInteraction._Interactable != null)
            {
                inspectObjectTransform = _PlayerInteraction._Interactable.transform;
                RotateObject();
            }
        }

        if (CameraToMouseRay(Input.mousePosition, out CustomRaycastHit customRayHit))
        {
            if (customRayHit.transform.CompareTag("Interactable2"))
            {
                inspectObjectTransform = customRayHit.transform;
                Debug.Log(customRayHit.transform.gameObject.name);
            }
        }
    }

    public void SetInputRotateVector(Vector2 rotation)
    {
        targetRotateVector = rotation;
    }

    public void RotateObject()
    {
        if (!inspectMode || inspectObjectTransform == null)
            return;

        inputRotateVector = Vector2.Lerp(inputRotateVector, targetRotateVector, Time.deltaTime * smoothingFactor);

        float deltaX = inputRotateVector.x * (rotateSpeed * 10f) * Time.deltaTime;
        float deltaY = inputRotateVector.y * (rotateSpeed * 10f) * Time.deltaTime;

        inspectObjectTransform.rotation =
                Quaternion.AngleAxis(-deltaX, transform.up) *
                Quaternion.AngleAxis(deltaY, transform.right) *
                inspectObjectTransform.rotation;
        
    }

    private bool CameraToMouseRay(Vector3 mousePosition, out CustomRaycastHit customRayHit)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);
        Vector3 worldPoint = ray.GetPoint(2f);  // Get a point along the ray a short distance away from the camera

        // Use OverlapSphere to detect colliders within a certain radius from the world point around the mouse position
        Collider[] colliders = Physics.OverlapSphere(worldPoint, detectionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Interactable2"))
            {
                customRayHit = new CustomRaycastHit
                {
                    transform = collider.transform,
                    point = collider.transform.position,
                    normal = Vector3.zero, // Default normal
                    distance = Vector3.Distance(worldPoint, collider.transform.position)
                };
                return true;
            }
        }

        customRayHit = default;
        return false;
    }
}


