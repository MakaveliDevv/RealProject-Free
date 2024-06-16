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

    public void LookAroundWithMouseInput(InputAction.CallbackContext ctx)
    {
        // if(_Player != null)
        // {
        //     if(ctx.performed)
        //     {
        //         _Player
        //     }
        // }
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
        if (_PlayerInteraction != null && ctx.performed)
            if(_PlayerInteraction._Grabable != null) 
                _PlayerInteraction._Grabable.ToggleGrab();
    }

    public void InspectInput(InputAction.CallbackContext ctx) 
    {
        if(ctx.performed) 
        {
            if(_PlayerInteraction._Interactable._InteractableType == Interactable.InteractableType.INSPECTABLE) 
            {
                _InspectObject.inspectMode = true;
                _PlayerInteraction._Interactable.Inspect();
            }
        }
    }

    public void RotateObjectInput(InputAction.CallbackContext ctx)
    {
        if (_InspectObject != null && _InspectObject.inspectMode)
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
            if(_InspectObject.inspectObject.TryGetComponent<Interactable>(out var _Interactable)) 
            {
                _Interactable.releaseAfterInspect = true;
                _Interactable.AfterInspectInput();
                Debug.Log("Object released from inspect mode");
            } 
        }
    }

    public void GrabAfterInspect(InputAction.CallbackContext ctx) 
    {
        // This is meant for when in inspect mode
        if(_InspectObject.inspectMode && ctx.performed) 
        {
            if(_InspectObject.inspectObject.TryGetComponent<Interactable>(out var _Interactable)) 
            {
                _Interactable.grabAfterInspect = true;
                _Interactable.AfterInspectInput();
                Debug.Log("Object grabbed from inspect mode");
            } 
        }
    }

    public void JumpInput(InputAction.CallbackContext ctx) 
    {
        if(ctx.performed)
        {
            if(_Player != null && !_Player.jumping) 
            {   
                Debug.Log(ctx.performed);
                _Player.Jump();
            }
        }
    }

    public void UnlockInput(InputAction.CallbackContext ctx) 
    {
        if(ctx.performed)
            if(_Player != null)
                _Player.LockUnlockState();     
    }

    public void ReloadSceneInput(InputAction.CallbackContext ctx) 
    {
        if(ctx.performed)
            if(_Player != null)
                _Player.ReloadScene();     
    }
}