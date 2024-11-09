using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore
{
    public class PlayerCameraControls : MonoBehaviour
    {
        [Header("Camera Properties")] [SerializeField]
        private float rotationSpeed = 1f;

        [SerializeField] private GameObject playerCamera = null;

        private PlayerActions _playerActions = null;
        
        private Transform _cameraTransform = null;
        private Vector2 _cameraRotationDelta = Vector2.zero;
        
        private void Awake()
        {
            if (playerCamera == null) return;

            if (playerCamera.GetComponentInChildren<Camera>() != null)
            {
                _cameraTransform = playerCamera.GetComponentInChildren<Camera>().transform;
                return;
            }
            
            if (playerCamera.GetComponentInChildren<CinemachineVirtualCamera>() != null)
            {
                _cameraTransform = playerCamera.GetComponentInChildren<CinemachineVirtualCamera>().Follow.transform;
            }
        }

        private void OnEnable()
        {
            _playerActions = new();
            _playerActions.StandardMovement.Enable();
            // _playerActions.StandardMovement.RotateCamera.performed += HandleLeftRightRotationInput;
            // _playerActions.StandardMovement.RotateCamera.canceled += HandleLeftRightRotationInput;
        }

        private void OnDisable()
        {
            // _playerActions.StandardMovement.RotateCamera.performed -= HandleLeftRightRotationInput;
            // _playerActions.StandardMovement.RotateCamera.canceled -= HandleLeftRightRotationInput;
            _playerActions.StandardMovement.Disable();
        }

        private void FixedUpdate()
        {
            HandleCameraRotation();
        }
        
        private void HandleCameraRotation()
        {
            if (playerCamera == null) return;
            
            var rotation = _cameraTransform.rotation.eulerAngles;
            rotation.y = _cameraRotationDelta.x * rotationSpeed;
            _cameraTransform.rotation = Quaternion.Euler(rotation);
        }
        
        private void HandleLeftRightRotationInput(InputAction.CallbackContext ctx)
        {
            _cameraRotationDelta = ctx.ReadValue<Vector2>();
            // if (playerCamera == null) return;
            // _cameraRotationDelta += delta.x * rotationSpeed;
            // var rotation = _cameraTransform.rotation.eulerAngles;
            // _cameraTransform.rotation = Quaternion.Euler(rotation);
        }
    }
}