//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.11.2
//     from Assets/Game-Core/Player/Player Actions.inputactions
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

public partial class @PlayerActions: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerActions()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""Player Actions"",
    ""maps"": [
        {
            ""name"": ""StandardMovement"",
            ""id"": ""a0cfb12a-189a-44f3-8803-43a3ecbf33c0"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""b180bb6f-af12-40ad-979a-62ba54691308"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Touch Move"",
                    ""type"": ""Button"",
                    ""id"": ""e54a0829-2288-4779-99e1-cccdd1fb4623"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""Value"",
                    ""id"": ""b399b415-6850-4a3d-83a1-e16045c39f06"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""c4e79ebd-c028-4686-bc1a-8f5d53e01cab"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Crouch"",
                    ""type"": ""Button"",
                    ""id"": ""5ef0c3f7-cd54-48c8-9387-0173285f37a8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Sprint"",
                    ""type"": ""Button"",
                    ""id"": ""269f09f1-3f41-4221-9632-5e8c027d1455"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Rotate Player"",
                    ""type"": ""Value"",
                    ""id"": ""985a97b4-5929-4431-a2ff-59a2a8beea1d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""95a93ffb-7174-49a9-aca8-0270803efd29"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7c88f552-a21f-48d2-bb01-35dd21a97c19"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a022756a-2fa4-463f-a5b5-a0bc14038741"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f358b383-e464-4c8b-814b-6aa4410c9df3"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""10a86d58-7454-424b-b8ef-b36dec10d6d5"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""7462293b-74f8-4f50-90a9-fda325eded00"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Touch Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c40eacdc-fd18-46d9-b9f1-27ac7c7de03a"",
                    ""path"": ""<Touchscreen>/touch0/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Touch Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7e443e45-12e8-4c40-bb7a-e5f20261fe87"",
                    ""path"": ""<Pointer>/{Point}"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d76bb439-835a-451e-a091-f254f21a2456"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15766d15-4b71-411b-90bc-b593c9e9b268"",
                    ""path"": ""<Keyboard>/leftCtrl"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Crouch"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9c27dbee-0a4a-4acb-b350-cdafd96ba386"",
                    ""path"": ""<Keyboard>/shift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Sprint"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""5c622253-5b9c-42ce-83ba-35ce8e797c17"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate Player"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""left"",
                    ""id"": ""d379d216-e1ef-45c4-8630-12260b044477"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Rotate Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""bebb63ea-a83b-45cb-acf7-92e4abfbd693"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Rotate Player"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                }
            ]
        },
        {
            ""name"": ""OverviewMapControls"",
            ""id"": ""276c5945-acb2-4c91-9e82-1f6e4916b603"",
            ""actions"": [
                {
                    ""name"": ""Select"",
                    ""type"": ""Button"",
                    ""id"": ""90b7d3b2-c444-4246-bcd6-922c85935b41"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""0a57eb9e-ce4d-46fd-bf9e-a19bcc79a510"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Game"",
                    ""action"": ""Select"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""LayerControls"",
            ""id"": ""f0b30ec9-c6f0-4b3e-9508-5e2afaf6b0ca"",
            ""actions"": [
                {
                    ""name"": ""PreviousLayer"",
                    ""type"": ""Value"",
                    ""id"": ""297a1c98-bda4-4b82-af47-372d183e8e94"",
                    ""expectedControlType"": """",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""NextLayer"",
                    ""type"": ""Button"",
                    ""id"": ""6b8245ba-c998-4600-8e67-4ef584cfdb5b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""6aa3a7dd-e415-4429-9d44-ae04cd1ca7e8"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""PreviousLayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5945b976-672c-488c-a6ed-d4078ecd976c"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""NextLayer"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Game"",
            ""bindingGroup"": ""Game"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": true,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": true,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // StandardMovement
        m_StandardMovement = asset.FindActionMap("StandardMovement", throwIfNotFound: true);
        m_StandardMovement_Move = m_StandardMovement.FindAction("Move", throwIfNotFound: true);
        m_StandardMovement_TouchMove = m_StandardMovement.FindAction("Touch Move", throwIfNotFound: true);
        m_StandardMovement_Position = m_StandardMovement.FindAction("Position", throwIfNotFound: true);
        m_StandardMovement_Jump = m_StandardMovement.FindAction("Jump", throwIfNotFound: true);
        m_StandardMovement_Crouch = m_StandardMovement.FindAction("Crouch", throwIfNotFound: true);
        m_StandardMovement_Sprint = m_StandardMovement.FindAction("Sprint", throwIfNotFound: true);
        m_StandardMovement_RotatePlayer = m_StandardMovement.FindAction("Rotate Player", throwIfNotFound: true);
        // OverviewMapControls
        m_OverviewMapControls = asset.FindActionMap("OverviewMapControls", throwIfNotFound: true);
        m_OverviewMapControls_Select = m_OverviewMapControls.FindAction("Select", throwIfNotFound: true);
        // LayerControls
        m_LayerControls = asset.FindActionMap("LayerControls", throwIfNotFound: true);
        m_LayerControls_PreviousLayer = m_LayerControls.FindAction("PreviousLayer", throwIfNotFound: true);
        m_LayerControls_NextLayer = m_LayerControls.FindAction("NextLayer", throwIfNotFound: true);
    }

    ~@PlayerActions()
    {
        UnityEngine.Debug.Assert(!m_StandardMovement.enabled, "This will cause a leak and performance issues, PlayerActions.StandardMovement.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_OverviewMapControls.enabled, "This will cause a leak and performance issues, PlayerActions.OverviewMapControls.Disable() has not been called.");
        UnityEngine.Debug.Assert(!m_LayerControls.enabled, "This will cause a leak and performance issues, PlayerActions.LayerControls.Disable() has not been called.");
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

    // StandardMovement
    private readonly InputActionMap m_StandardMovement;
    private List<IStandardMovementActions> m_StandardMovementActionsCallbackInterfaces = new List<IStandardMovementActions>();
    private readonly InputAction m_StandardMovement_Move;
    private readonly InputAction m_StandardMovement_TouchMove;
    private readonly InputAction m_StandardMovement_Position;
    private readonly InputAction m_StandardMovement_Jump;
    private readonly InputAction m_StandardMovement_Crouch;
    private readonly InputAction m_StandardMovement_Sprint;
    private readonly InputAction m_StandardMovement_RotatePlayer;
    public struct StandardMovementActions
    {
        private @PlayerActions m_Wrapper;
        public StandardMovementActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_StandardMovement_Move;
        public InputAction @TouchMove => m_Wrapper.m_StandardMovement_TouchMove;
        public InputAction @Position => m_Wrapper.m_StandardMovement_Position;
        public InputAction @Jump => m_Wrapper.m_StandardMovement_Jump;
        public InputAction @Crouch => m_Wrapper.m_StandardMovement_Crouch;
        public InputAction @Sprint => m_Wrapper.m_StandardMovement_Sprint;
        public InputAction @RotatePlayer => m_Wrapper.m_StandardMovement_RotatePlayer;
        public InputActionMap Get() { return m_Wrapper.m_StandardMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(StandardMovementActions set) { return set.Get(); }
        public void AddCallbacks(IStandardMovementActions instance)
        {
            if (instance == null || m_Wrapper.m_StandardMovementActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_StandardMovementActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @TouchMove.started += instance.OnTouchMove;
            @TouchMove.performed += instance.OnTouchMove;
            @TouchMove.canceled += instance.OnTouchMove;
            @Position.started += instance.OnPosition;
            @Position.performed += instance.OnPosition;
            @Position.canceled += instance.OnPosition;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Crouch.started += instance.OnCrouch;
            @Crouch.performed += instance.OnCrouch;
            @Crouch.canceled += instance.OnCrouch;
            @Sprint.started += instance.OnSprint;
            @Sprint.performed += instance.OnSprint;
            @Sprint.canceled += instance.OnSprint;
            @RotatePlayer.started += instance.OnRotatePlayer;
            @RotatePlayer.performed += instance.OnRotatePlayer;
            @RotatePlayer.canceled += instance.OnRotatePlayer;
        }

        private void UnregisterCallbacks(IStandardMovementActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @TouchMove.started -= instance.OnTouchMove;
            @TouchMove.performed -= instance.OnTouchMove;
            @TouchMove.canceled -= instance.OnTouchMove;
            @Position.started -= instance.OnPosition;
            @Position.performed -= instance.OnPosition;
            @Position.canceled -= instance.OnPosition;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Crouch.started -= instance.OnCrouch;
            @Crouch.performed -= instance.OnCrouch;
            @Crouch.canceled -= instance.OnCrouch;
            @Sprint.started -= instance.OnSprint;
            @Sprint.performed -= instance.OnSprint;
            @Sprint.canceled -= instance.OnSprint;
            @RotatePlayer.started -= instance.OnRotatePlayer;
            @RotatePlayer.performed -= instance.OnRotatePlayer;
            @RotatePlayer.canceled -= instance.OnRotatePlayer;
        }

        public void RemoveCallbacks(IStandardMovementActions instance)
        {
            if (m_Wrapper.m_StandardMovementActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IStandardMovementActions instance)
        {
            foreach (var item in m_Wrapper.m_StandardMovementActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_StandardMovementActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public StandardMovementActions @StandardMovement => new StandardMovementActions(this);

    // OverviewMapControls
    private readonly InputActionMap m_OverviewMapControls;
    private List<IOverviewMapControlsActions> m_OverviewMapControlsActionsCallbackInterfaces = new List<IOverviewMapControlsActions>();
    private readonly InputAction m_OverviewMapControls_Select;
    public struct OverviewMapControlsActions
    {
        private @PlayerActions m_Wrapper;
        public OverviewMapControlsActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @Select => m_Wrapper.m_OverviewMapControls_Select;
        public InputActionMap Get() { return m_Wrapper.m_OverviewMapControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OverviewMapControlsActions set) { return set.Get(); }
        public void AddCallbacks(IOverviewMapControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_OverviewMapControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_OverviewMapControlsActionsCallbackInterfaces.Add(instance);
            @Select.started += instance.OnSelect;
            @Select.performed += instance.OnSelect;
            @Select.canceled += instance.OnSelect;
        }

        private void UnregisterCallbacks(IOverviewMapControlsActions instance)
        {
            @Select.started -= instance.OnSelect;
            @Select.performed -= instance.OnSelect;
            @Select.canceled -= instance.OnSelect;
        }

        public void RemoveCallbacks(IOverviewMapControlsActions instance)
        {
            if (m_Wrapper.m_OverviewMapControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IOverviewMapControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_OverviewMapControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_OverviewMapControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public OverviewMapControlsActions @OverviewMapControls => new OverviewMapControlsActions(this);

    // LayerControls
    private readonly InputActionMap m_LayerControls;
    private List<ILayerControlsActions> m_LayerControlsActionsCallbackInterfaces = new List<ILayerControlsActions>();
    private readonly InputAction m_LayerControls_PreviousLayer;
    private readonly InputAction m_LayerControls_NextLayer;
    public struct LayerControlsActions
    {
        private @PlayerActions m_Wrapper;
        public LayerControlsActions(@PlayerActions wrapper) { m_Wrapper = wrapper; }
        public InputAction @PreviousLayer => m_Wrapper.m_LayerControls_PreviousLayer;
        public InputAction @NextLayer => m_Wrapper.m_LayerControls_NextLayer;
        public InputActionMap Get() { return m_Wrapper.m_LayerControls; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(LayerControlsActions set) { return set.Get(); }
        public void AddCallbacks(ILayerControlsActions instance)
        {
            if (instance == null || m_Wrapper.m_LayerControlsActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_LayerControlsActionsCallbackInterfaces.Add(instance);
            @PreviousLayer.started += instance.OnPreviousLayer;
            @PreviousLayer.performed += instance.OnPreviousLayer;
            @PreviousLayer.canceled += instance.OnPreviousLayer;
            @NextLayer.started += instance.OnNextLayer;
            @NextLayer.performed += instance.OnNextLayer;
            @NextLayer.canceled += instance.OnNextLayer;
        }

        private void UnregisterCallbacks(ILayerControlsActions instance)
        {
            @PreviousLayer.started -= instance.OnPreviousLayer;
            @PreviousLayer.performed -= instance.OnPreviousLayer;
            @PreviousLayer.canceled -= instance.OnPreviousLayer;
            @NextLayer.started -= instance.OnNextLayer;
            @NextLayer.performed -= instance.OnNextLayer;
            @NextLayer.canceled -= instance.OnNextLayer;
        }

        public void RemoveCallbacks(ILayerControlsActions instance)
        {
            if (m_Wrapper.m_LayerControlsActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(ILayerControlsActions instance)
        {
            foreach (var item in m_Wrapper.m_LayerControlsActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_LayerControlsActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public LayerControlsActions @LayerControls => new LayerControlsActions(this);
    private int m_GameSchemeIndex = -1;
    public InputControlScheme GameScheme
    {
        get
        {
            if (m_GameSchemeIndex == -1) m_GameSchemeIndex = asset.FindControlSchemeIndex("Game");
            return asset.controlSchemes[m_GameSchemeIndex];
        }
    }
    public interface IStandardMovementActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnTouchMove(InputAction.CallbackContext context);
        void OnPosition(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnCrouch(InputAction.CallbackContext context);
        void OnSprint(InputAction.CallbackContext context);
        void OnRotatePlayer(InputAction.CallbackContext context);
    }
    public interface IOverviewMapControlsActions
    {
        void OnSelect(InputAction.CallbackContext context);
    }
    public interface ILayerControlsActions
    {
        void OnPreviousLayer(InputAction.CallbackContext context);
        void OnNextLayer(InputAction.CallbackContext context);
    }
}
