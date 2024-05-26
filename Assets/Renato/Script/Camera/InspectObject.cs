using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public Interactable _Interactable;
    public new Camera camera;
    public RaycastHit hitInfo;
    public float rotateSpeed = 200f;
    public float smoothingFactor = 8f;
    public float detectionRadius = 1f;
    public bool objectHit;
    public Transform inspectObject;
    [SerializeField] private PlayerInteraction _PlayerInteraction;

    public bool inspectMode;
    private Vector2 inputRotateVector, targetRotateVector;

    private Vector3 rayOrigin;
    private Vector3 rayDirection;
    public bool rayExists = false;
    public float distance = 2f;

    #region Singleton
    public static InspectObject instance;

    void Awake()
    {
        instance = this;

        camera = GetComponent<Camera>();
        _PlayerInteraction = GetComponentInParent<PlayerInteraction>();
    }
    #endregion

    private void Update()
    {
        if (_PlayerInteraction != null)
        {
            if (_PlayerInteraction._Interactable != null)
            {
                if (_PlayerInteraction._Interactable.TryGetComponent<Interactable>(out var interactable))
                {
                    _Interactable = interactable;
                    inspectObject = interactable.transform;

                    if (_Interactable._InteractableType == Interactable.InteractableType.INSPECTABLE)
                    {
                        RotateObject();
                    }
                }
            }
        }
    }

    public void SetInputRotateVector(Vector2 rotation)
    {
        targetRotateVector = rotation;
    }

    public void RotateObject()
    {
        if (!inspectMode || inspectObject == null)
            return;

        inputRotateVector = Vector2.Lerp(inputRotateVector, targetRotateVector, Time.deltaTime * smoothingFactor);

        float deltaX = inputRotateVector.x * (rotateSpeed * 10f) * Time.deltaTime;
        float deltaY = inputRotateVector.y * (rotateSpeed * 10f) * Time.deltaTime;

        inspectObject.rotation =
                Quaternion.AngleAxis(-deltaX, transform.up) *
                Quaternion.AngleAxis(deltaY, transform.right) *
                inspectObject.rotation;
    }

    public bool CameraToMouseRay()
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        rayOrigin = ray.origin;
        rayDirection = ray.direction;
        rayExists = true;  // Indicate that a ray exists

        return Physics.Raycast(ray, out hitInfo, distance);
    }

    private void OnDrawGizmos()
    {
        if (rayExists)
        {
            Gizmos.color = objectHit ? Color.green : Color.red;
            Gizmos.DrawRay(rayOrigin, rayDirection * distance); // Draw the ray extending 'distance' units into the scene
        }
    }
}
