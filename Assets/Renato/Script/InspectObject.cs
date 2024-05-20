using System;
using UnityEngine;

public class InspectObject : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private GameObject player;

    private Transform inspectObjectTransform;
    [SerializeField] private float deltaRotationX;
    [SerializeField] private float deltaRotationY;

    [SerializeField] private float rotateSpeed = 2f;

    void Awake() 
    {
        camera = GetComponent<Camera>();
        // transform.SetParent(player.transform);
        // transform.position = player.transform.position + new Vector3(0f, .5f, 0f);
    }

    void Update() 
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            if (CameraToMouseRay(Input.mousePosition, out RaycastHit RayHit))
            {
                if (RayHit.transform.gameObject.CompareTag("Inspectable"))
                    inspectObjectTransform = RayHit.transform;
            }
        }

        deltaRotationX = -Input.GetAxis("Mouse X");
        deltaRotationY = Input.GetAxis("Mouse Y");

        if(Input.GetMouseButton(1)) 
        {
            if(inspectObjectTransform == null)
                return; 
                
            inspectObjectTransform.rotation = 
                Quaternion.AngleAxis(deltaRotationX * rotateSpeed, transform.up) * 
                Quaternion.AngleAxis(deltaRotationY * rotateSpeed, transform.right) * 
                inspectObjectTransform.rotation;
        }
    }

    private bool CameraToMouseRay(Vector3 mousePosition, out RaycastHit rayHit)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);

        return Physics.Raycast(ray, out rayHit);
    }
}
