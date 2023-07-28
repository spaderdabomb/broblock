//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Input/GameInput.inputactions
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

public partial class @GameInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @GameInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""GameInput"",
    ""maps"": [
        {
            ""name"": ""GameSceneInput"",
            ""id"": ""1b35c06c-c809-4853-8dd4-ac8ac89c01e7"",
            ""actions"": [
                {
                    ""name"": ""TogglePauseMenu"",
                    ""type"": ""Button"",
                    ""id"": ""ba7a7ae3-6206-448f-85e6-66fbcdeef0cd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""ea93171d-434b-4471-9760-0fe9c14b47ea"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TogglePauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c3be19b5-5826-4dc6-a7a9-b53e5d5acd77"",
                    ""path"": ""<Keyboard>/p"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""TogglePauseMenu"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // GameSceneInput
        m_GameSceneInput = asset.FindActionMap("GameSceneInput", throwIfNotFound: true);
        m_GameSceneInput_TogglePauseMenu = m_GameSceneInput.FindAction("TogglePauseMenu", throwIfNotFound: true);
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

    // GameSceneInput
    private readonly InputActionMap m_GameSceneInput;
    private List<IGameSceneInputActions> m_GameSceneInputActionsCallbackInterfaces = new List<IGameSceneInputActions>();
    private readonly InputAction m_GameSceneInput_TogglePauseMenu;
    public struct GameSceneInputActions
    {
        private @GameInput m_Wrapper;
        public GameSceneInputActions(@GameInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @TogglePauseMenu => m_Wrapper.m_GameSceneInput_TogglePauseMenu;
        public InputActionMap Get() { return m_Wrapper.m_GameSceneInput; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GameSceneInputActions set) { return set.Get(); }
        public void AddCallbacks(IGameSceneInputActions instance)
        {
            if (instance == null || m_Wrapper.m_GameSceneInputActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_GameSceneInputActionsCallbackInterfaces.Add(instance);
            @TogglePauseMenu.started += instance.OnTogglePauseMenu;
            @TogglePauseMenu.performed += instance.OnTogglePauseMenu;
            @TogglePauseMenu.canceled += instance.OnTogglePauseMenu;
        }

        private void UnregisterCallbacks(IGameSceneInputActions instance)
        {
            @TogglePauseMenu.started -= instance.OnTogglePauseMenu;
            @TogglePauseMenu.performed -= instance.OnTogglePauseMenu;
            @TogglePauseMenu.canceled -= instance.OnTogglePauseMenu;
        }

        public void RemoveCallbacks(IGameSceneInputActions instance)
        {
            if (m_Wrapper.m_GameSceneInputActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IGameSceneInputActions instance)
        {
            foreach (var item in m_Wrapper.m_GameSceneInputActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_GameSceneInputActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public GameSceneInputActions @GameSceneInput => new GameSceneInputActions(this);
    public interface IGameSceneInputActions
    {
        void OnTogglePauseMenu(InputAction.CallbackContext context);
    }
}
