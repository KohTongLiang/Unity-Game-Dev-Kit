using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GameCore
{
    public abstract class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")] protected float _moveSpeed;

        [SerializeField] protected float walkSpeed;
        [SerializeField] protected float sprintSpeed;
        [SerializeField] protected float rotationSpeed;
        public bool sliding;
        public float slidingSpeed;
        protected float desiredMoveSpeed;
        protected float lastDesiredMoveSpeed;

        [Header("Jumping")] [SerializeField] protected float jumpForce;
        [SerializeField] protected float jumpCooldown;
        [SerializeField] protected float airMultiplier;
        protected bool _jumpReady = true;

        [Header("Crouching")] [SerializeField] protected float crouchSpeed;
        [SerializeField] protected float crouchYScale;
        protected float _startYScale;

        [Header("Slope Handling")] [SerializeField]
        protected float maxSlopeAngle;

        protected RaycastHit _slopeHit;
        protected bool _exitingSlope;

        [Header("Ground Check")] [SerializeField]
        protected float playerHeight;

        [SerializeField] protected LayerMask groundMask;
        [SerializeField] protected float groundDrag;
        protected bool _grounded;

        [SerializeField] protected Transform orientation;

        protected float _horizontalInput;
        protected float _verticalInput;
        protected Vector3 _moveDirection;
        protected Rigidbody _rb;

        private MovementState _playerMovementState;
        protected MovementState GetPlayerMovementState
        {
            get => _playerMovementState;

            private set
            {
                if (_playerMovementState == value) return;
                UpdateMovementState(value);
                _playerMovementState = value;
            }
        }

        protected virtual void UpdateMovementState(MovementState state)
        {
        }

        protected enum MovementState
        {
            Default,
            Walking,
            Air,
            Sliding
        }

        protected PlayerState _playerState;
        protected PlayerActions _playerActions = null;

        protected float _walkingSpeedModifier = 1f;
        protected Vector2 _playerRotation;

        #region Movement State

        protected void StateHandler()
        {
            if (sliding)
            {
                GetPlayerMovementState = MovementState.Sliding;
                if (OnSlope() && _rb.velocity.y < 0.1f)
                {
                    desiredMoveSpeed = slidingSpeed;
                }
                else
                {
                    desiredMoveSpeed = sprintSpeed;
                }
            }

            if (_grounded)
            {
                GetPlayerMovementState = _rb.velocity.magnitude == 0f ? MovementState.Default : MovementState.Walking;
                desiredMoveSpeed = walkSpeed * _walkingSpeedModifier;
            }
            else
            {
                GetPlayerMovementState = MovementState.Air;
            }

            if (Mathf.Abs(desiredMoveSpeed - lastDesiredMoveSpeed) > (sprintSpeed - walkSpeed) && _moveSpeed != 0)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothlyLerpMoveSpeeD());
            }
            else
            {
                _moveSpeed = desiredMoveSpeed;
            }

            lastDesiredMoveSpeed = desiredMoveSpeed;
        }

        #endregion

        protected void OnEnable()
        {
            _playerActions = new();
            _playerActions.StandardMovement.Enable();
            _playerActions.StandardMovement.Move.performed += HandleBasicMovement;
            _playerActions.StandardMovement.Move.canceled += HandleBasicMovement;
            _playerActions.StandardMovement.Jump.performed += HandleJump;
            _playerActions.StandardMovement.Crouch.performed += HandleCrouching;
            _playerActions.StandardMovement.Crouch.canceled += HandleCrouching;
            _playerActions.StandardMovement.Sprint.performed += HandleSprinting;
            _playerActions.StandardMovement.Sprint.canceled += HandleSprinting;
            _playerActions.StandardMovement.RotatePlayer.performed += HandleRotatePlayer;
            _playerActions.StandardMovement.RotatePlayer.canceled += HandleRotatePlayer;
        }

        protected void OnDisable()
        {
            _playerActions.StandardMovement.Move.performed -= HandleBasicMovement;
            _playerActions.StandardMovement.Move.canceled -= HandleBasicMovement;
            _playerActions.StandardMovement.Jump.performed -= HandleJump;
            _playerActions.StandardMovement.Crouch.performed -= HandleCrouching;
            _playerActions.StandardMovement.Crouch.canceled -= HandleCrouching;
            _playerActions.StandardMovement.Sprint.performed -= HandleSprinting;
            _playerActions.StandardMovement.Sprint.canceled -= HandleSprinting;
            _playerActions.StandardMovement.RotatePlayer.performed -= HandleRotatePlayer;
            _playerActions.StandardMovement.RotatePlayer.canceled -= HandleRotatePlayer;
            _playerActions.StandardMovement.Disable();
        }

        protected void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _rb.freezeRotation = true;
            _startYScale = transform.localScale.y;
            _playerState = GetComponent<PlayerState>();
        }

        protected void Update()
        {
            GroundCheck();
            SpeedControl();
            StateHandler();
        }

        protected void FixedUpdate()
        {
            MovePlayer();
        }

        #region Player Input

        protected void HandleBasicMovement(InputAction.CallbackContext ctx)
        {
            var position = ctx.ReadValue<Vector2>();
            _horizontalInput = position.x;
            _verticalInput = position.y;
        }

        protected void HandleRotatePlayer(InputAction.CallbackContext ctx)
        {
            _playerRotation = ctx.ReadValue<Vector2>();
        }

        protected void HandleJump(InputAction.CallbackContext ctx)
        {
            if (!_grounded || !_jumpReady) return;
            _jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        protected void HandleSprinting(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<float>();
            if (input > 0)
            {
                _walkingSpeedModifier *= 2f;
            }
            else
            {
                _walkingSpeedModifier *= 0.5f;
            }
        }

        protected void HandleCrouching(InputAction.CallbackContext ctx)
        {
            var input = ctx.ReadValue<float>();
            if (input > 0)
            {
                transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                _rb.AddForce(Vector3.down * 10f, ForceMode.Force);
                _walkingSpeedModifier *= 0.5f;
            }
            else
            {
                _walkingSpeedModifier *= 2f;
                transform.localScale = new Vector3(transform.localScale.x, _startYScale, transform.localScale.z);
            }
        }

        #endregion

        #region Player Movement Functions

        protected void MovePlayer()
        {
            _moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

            /// Slope Movement
            if (OnSlope() && !_exitingSlope)
            {
                _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _moveSpeed * _playerState.StaminaFactor * 2f,
                    ForceMode.Force);

                if (_rb.velocity.y > 0)
                {
                    _rb.AddForce(Vector3.down * 50f, ForceMode.Force);
                }
            }

            /// Grounded Movement
            if (_grounded)
            {
                _rb.AddForce(_moveDirection.normalized * _moveSpeed * _playerState.StaminaFactor, ForceMode.Force);
            }
            else if (!Physics.Raycast(transform.position, orientation.forward, out var hit, playerHeight * 0.5f + 0.1f))
            {
                _rb.AddForce(_moveDirection.normalized * _moveSpeed * airMultiplier * _playerState.StaminaFactor,
                    ForceMode.Force);
            }

            /// Turn Off Gravity on slope to prevent sliding
            _rb.useGravity = !OnSlope();

            if (_moveDirection.normalized * _moveSpeed != Vector3.zero)
            {
                switch (GetPlayerMovementState)
                {
                    case MovementState.Walking:
                        _playerState.DecreaseStamina();
                        break;
                        // case MovementState.SPRINTING:
                        //     player.DecreaseStamina(2f);
                        //     break;
                        // case MovementState.CROUCHING:
                        //     player.DecreaseStamina(0.20f);
                        break;
                    default:
                        break;
                }
            }

            transform.Rotate(Vector3.up, _playerRotation.x * rotationSpeed);
        }

        protected void Jump()
        {
            _playerState.DecreaseStamina(20f);
            _exitingSlope = true;
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            _rb.AddForce(transform.up * jumpForce * _playerState.StaminaFactor, ForceMode.Impulse);
        }

        protected void ResetJump()
        {
            _jumpReady = true;
            _exitingSlope = false;
        }

        #endregion

        #region Utility

        protected void GroundCheck()
        {
            _grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundMask);
            _rb.drag = _grounded ? groundDrag : 0;
        }

        protected void SpeedControl()
        {
            if (OnSlope() && !_exitingSlope)
            {
                if (_rb.velocity.magnitude > _moveSpeed) _rb.velocity = _rb.velocity.normalized * _moveSpeed;
            }
            else
            {
                Vector3 flatVelocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);

                if (flatVelocity.magnitude > _moveSpeed)
                {
                    Vector3 limitedVel = flatVelocity.normalized * _moveSpeed;
                    _rb.velocity = new Vector3(limitedVel.x, _rb.velocity.y, limitedVel.z);
                }
            }
        }


        public bool OnSlope()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out _slopeHit, playerHeight * 0.5f + 0.2f))
            {
                float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                return angle < maxSlopeAngle && angle != 0;
            }

            return false;
        }

        public Vector3 GetSlopeMoveDirection(Vector3 direction)
        {
            return Vector3.ProjectOnPlane(_moveDirection, _slopeHit.normal).normalized;
        }

        private IEnumerator SmoothlyLerpMoveSpeeD()
        {
            float time = 0;
            float difference = Mathf.Abs(desiredMoveSpeed - _moveSpeed);
            float startMoveSpeed = _moveSpeed;

            while (time < difference)
            {
                _moveSpeed = Mathf.Lerp(startMoveSpeed, desiredMoveSpeed, time / difference);
                time += Time.deltaTime;
                yield return null;
            }

            _moveSpeed = desiredMoveSpeed;
        }

        #endregion
    }
}