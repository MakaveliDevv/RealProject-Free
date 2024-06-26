//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Pepijn/MovementController.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @MovementController: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @MovementController()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""MovementController"",
    ""maps"": [
        {
            ""name"": ""Feet"",
            ""id"": ""2fb79b31-c3fe-455c-8483-373420706922"",
            ""actions"": [
                {
                    ""name"": ""MoveLeft"",
                    ""type"": ""Button"",
                    ""id"": ""c4fcc285-f7a8-4fd3-b0a6-1ad923fdf2c7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""MoveRigt"",
                    ""type"": ""Button"",
                    ""id"": ""7f40de44-3f84-4883-b1c0-27d8f45f2957"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""664576d7-bc65-474b-bcb9-e7e3a0595b04"",
                    ""path"": ""<Gamepad>/leftTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e33665e7-af0a-4b9b-9d89-a6aed8e75111"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveRigt"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Canvas"",
            ""id"": ""13dcd254-2f77-4af4-a678-90b9b9341b9d"",
            ""actions"": [
                {
                    ""name"": ""MoveCursor"",
                    ""type"": ""Value"",
                    ""id"": ""700b9f14-b62f-4f43-af34-319bfe2b3d5f"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Paint"",
                    ""type"": ""Button"",
                    ""id"": ""8a00a3c3-ffc4-483f-bdab-e5dd1d2f4b2b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""8a780c5d-b193-49b7-8979-6d0eba4233f0"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9023f658-3ed7-41ac-8d7f-7a01ab180342"",
                    ""path"": ""<Gamepad>/rightStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2477b339-6fe7-460b-8dc5-f8bb33870944"",
                    ""path"": ""<Gamepad>/rightStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""89fe8c68-0569-4116-9d2a-6c84fda289a5"",
                    ""path"": ""<Gamepad>/rightStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cde595f7-ed8e-447d-ba41-7610aeacfd83"",
                    ""path"": ""<Gamepad>/rightStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCursor"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""c2cbdc9a-5471-42ff-8673-2ae96868a084"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Paint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Feet
        m_Feet = asset.FindActionMap("Feet", throwIfNotFound: true);
        m_Feet_MoveLeft = m_Feet.FindAction("MoveLeft", throwIfNotFound: true);
        m_Feet_MoveRigt = m_Feet.FindAction("MoveRigt", throwIfNotFound: true);
        // Canvas
        m_Canvas = asset.FindActionMap("Canvas", throwIfNotFound: true);
        m_Canvas_MoveCursor = m_Canvas.FindAction("MoveCursor", throwIfNotFound: true);
        m_Canvas_Paint = m_Canvas.FindAction("Paint", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Feet
    private readonly InputActionMap m_Feet;
    private List<IFeetActions> m_FeetActionsCallbackInterfaces = new List<IFeetActions>();
    private readonly InputAction m_Feet_MoveLeft;
    private readonly InputAction m_Feet_MoveRigt;
    public struct FeetActions
    {
        private @MovementController m_Wrapper;
        public FeetActions(@MovementController wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveLeft => m_Wrapper.m_Feet_MoveLeft;
        public InputAction @MoveRigt => m_Wrapper.m_Feet_MoveRigt;
        public InputActionMap Get() { return m_Wrapper.m_Feet; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FeetActions set) { return set.Get(); }
        public void AddCallbacks(IFeetActions instance)
        {
            if (instance == null || m_Wrapper.m_FeetActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_FeetActionsCallbackInterfaces.Add(instance);
            @MoveLeft.started += instance.OnMoveLeft;
            @MoveLeft.performed += instance.OnMoveLeft;
            @MoveLeft.canceled += instance.OnMoveLeft;
            @MoveRigt.started += instance.OnMoveRigt;
            @MoveRigt.performed += instance.OnMoveRigt;
            @MoveRigt.canceled += instance.OnMoveRigt;
        }

        private void UnregisterCallbacks(IFeetActions instance)
        {
            @MoveLeft.started -= instance.OnMoveLeft;
            @MoveLeft.performed -= instance.OnMoveLeft;
            @MoveLeft.canceled -= instance.OnMoveLeft;
            @MoveRigt.started -= instance.OnMoveRigt;
            @MoveRigt.performed -= instance.OnMoveRigt;
            @MoveRigt.canceled -= instance.OnMoveRigt;
        }

        public void RemoveCallbacks(IFeetActions instance)
        {
            if (m_Wrapper.m_FeetActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IFeetActions instance)
        {
            foreach (var item in m_Wrapper.m_FeetActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_FeetActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public FeetActions @Feet => new FeetActions(this);

    // Canvas
    private readonly InputActionMap m_Canvas;
    private List<ICanvasActions> m_CanvasActionsCallbackInterfaces = new List<ICanvasActions>();
    private readonly InputAction m_Canvas_MoveCursor;
    private readonly InputAction m_Canvas_Paint;
    public struct CanvasActions
    {
        private @MovementController m_Wrapper;
        public CanvasActions(@MovementController wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveCursor => m_Wrapper.m_Canvas_MoveCursor;
        public InputAction @Paint => m_Wrapper.m_Canvas_Paint;
        public InputActionMap Get() { return m_Wrapper.m_Canvas; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(CanvasActions set) { return set.Get(); }
        public void AddCallbacks(ICanvasActions instance)
        {
            if (instance == null || m_Wrapper.m_CanvasActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_CanvasActionsCallbackInterfaces.Add(instance);
            @MoveCursor.started += instance.OnMoveCursor;
            @MoveCursor.performed += instance.OnMoveCursor;
            @MoveCursor.canceled += instance.OnMoveCursor;
            @Paint.started += instance.OnPaint;
            @Paint.performed += instance.OnPaint;
            @Paint.canceled += instance.OnPaint;
        }

        private void UnregisterCallbacks(ICanvasActions instance)
        {
            @MoveCursor.started -= instance.OnMoveCursor;
            @MoveCursor.performed -= instance.OnMoveCursor;
            @MoveCursor.canceled -= instance.OnMoveCursor;
            @Paint.started -= instance.OnPaint;
            @Paint.performed -= instance.OnPaint;
            @Paint.canceled -= instance.OnPaint;
        }

        public void RemoveCallbacks(ICanvasActions instance)
        {
            if (m_Wrapper.m_CanvasActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ICanvasActions instance)
        {
            foreach (var item in m_Wrapper.m_CanvasActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_CanvasActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public CanvasActions @Canvas => new CanvasActions(this);
    public interface IFeetActions
    {
        void OnMoveLeft(InputAction.CallbackContext context);
        void OnMoveRigt(InputAction.CallbackContext context);
    }
    public interface ICanvasActions
    {
        void OnMoveCursor(InputAction.CallbackContext context);
        void OnPaint(InputAction.CallbackContext context);
    }
}
