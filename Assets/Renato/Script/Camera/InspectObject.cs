using UnityEngine;

public class InspectObject : MonoBehaviour
{
    public new Camera camera;
    public float rotateSpeed = 200f;
    public float smoothingFactor = 8f; 

    public Transform inspectObjectTransform;
    [SerializeField] private PlayerInteraction _PlayerInteraction;

    public bool inspectMode;
    private Vector2 inputRotateVector, targetRotateVector;
    

    void Awake() 
    {
        camera = GetComponent<Camera>();
        _PlayerInteraction = GetComponentInParent<PlayerInteraction>();
    }

    private void Update() 
    {
        if(inspectMode) 
        {
            if (_PlayerInteraction != null && _PlayerInteraction._Inspectable != null)
            {
                inspectObjectTransform = _PlayerInteraction._Inspectable.transform;
                RotateObject();
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

        float deltaX = inputRotateVector.x * rotateSpeed * Time.deltaTime;
        float deltaY = inputRotateVector.y * rotateSpeed * Time.deltaTime;

        inspectObjectTransform.rotation =
                Quaternion.AngleAxis(-deltaX, transform.up) *
                Quaternion.AngleAxis(deltaY, transform.right) *
                inspectObjectTransform.rotation;
    }
}


