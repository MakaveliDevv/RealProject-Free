using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerController _Player;
    private PlayerInteraction _PlayerInteraction;
    [SerializeField] private InspectObject _InspectObject;

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
        // else
        // {
        //     Debug.LogWarning("PlayerInteraction or Grabable is null.");
        // }
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
        // if (_InspectObject != null)
        // {
        //     if (ctx.performed)
        //     {
        //         _InspectObject.RotateObject(ctx.ReadValue<Vector2>());
        //     }
        //     else if (ctx.canceled)
        //     {
        //         // Handle cancellation if needed
        //         _InspectObject.RotateObject(Vector2.zero);
        //     }
        // }

        if (_InspectObject != null && ctx.performed)
        {
            Vector2 input = ctx.ReadValue<Vector2>();
            _InspectObject.RotateObject(input);
        }
    }
}
