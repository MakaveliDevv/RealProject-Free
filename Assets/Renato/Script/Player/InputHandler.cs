using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerController _Player;
    private PlayerInteraction _PlayerInteraction;
    [SerializeField] private InspectObject _InspectObject;
    [SerializeField] private float smoothingFactor = 5f;



    void Awake() 
    {
        _Player = GetComponent<PlayerController>();
        _PlayerInteraction = GetComponent<PlayerInteraction>();
        _InspectObject = GetComponentInChildren<InspectObject>();
    }
    
    public void MoveInput(InputAction.CallbackContext ctx) 
    {
        if(_Player != null) 
        {
            if(ctx.performed) 
                _Player.SetInputVectorMovement(ctx.ReadValue<Vector3>());
            
            else
                _Player.SetInputLookVector(Vector3.zero);
        }
    }

    public void LookAroundInput(InputAction.CallbackContext ctx) 
    {
        if(_Player != null) 
        {
            if(ctx.performed)     
                _Player.SetInputLookVector(ctx.ReadValue<Vector2>());
        
            else if(ctx.canceled)
                _Player.SetInputLookVector(Vector2.zero); 
        }
    }

    public void GrabInput(InputAction.CallbackContext ctx) 
    {
        if (_PlayerInteraction != null && _PlayerInteraction._Grabable != null)
        {
            if (_PlayerInteraction._Grabable.inGrabRange && ctx.performed)
            {
                _PlayerInteraction._Grabable.ToggleGrab();
            }
        }
    }

    public void InspectInput(InputAction.CallbackContext ctx) 
    {
        if(ctx.performed) 
        {
            // Inspect
            _PlayerInteraction._Inspectable.Inspect();
        }
    }

    public void RotateObjectInput(InputAction.CallbackContext ctx)
    {
        if (_InspectObject != null)
        {
            if(ctx.performed) 
                _InspectObject.SetInputRotateVector(ctx.ReadValue<Vector2>());
            
            else if (ctx.canceled)
                _InspectObject.SetInputRotateVector(Vector2.zero); 
        }

    }

    public void ReleaseAfterInspect(InputAction.CallbackContext ctx) 
    {
        // This is meant for when in inspect mode
        if(_InspectObject.inspectMode && ctx.performed) 
        {
            if(_InspectObject.inspectObjectTransform.TryGetComponent<Inspectable>(out var _Inspectable)) 
            {
                _Inspectable.releaseAfterInspect = true;
                _Inspectable.AfterInspectInput();
                Debug.Log("Object released from inspect mode");
            } 
        }
    }

    public void GrabAfterInspect(InputAction.CallbackContext ctx) 
    {
        // This is meant for when in inspect mode
        if(_InspectObject.inspectMode && ctx.performed) 
        {
            if(_InspectObject.inspectObjectTransform.TryGetComponent<Inspectable>(out var _Inspectable)) 
            {
                _Inspectable.grabAfterInspect = true;
                _Inspectable.AfterInspectInput();
                Debug.Log("Object grabbed from inspect mode");
            } 
        }
    }
}