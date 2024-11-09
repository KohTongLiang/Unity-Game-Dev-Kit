using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace GameCore
{
    public class PlayerState : MonoBehaviour
    {
        public static PlayerState Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        [Header("Player Stats")] [Header("Stamina")] [SerializeField]
        private float stamina;

        [SerializeField] private float maxStamina;
        [SerializeField] private float staminaDeclineRate;
        [SerializeField] private float staminaRecoveryRate;
        private float staminaFactor;

        public enum MovementState
        {
            DEFAULT,
            WALKING,
            SPRINTING,
            CROUCHING,
            AIR,
            SLIDING,
            MOUNTED
        }

        public MovementState PlayerMovementState { get; private set; } = MovementState.DEFAULT;

        public float Stamina
        {
            get => stamina;
            set => stamina = value;
        }

        public float StaminaFactor
        {
            get => staminaFactor;
            set => staminaFactor = value;
        }

        public void DecreaseStamina(float multiplier = 1)
        {
            if (Stamina > 0) Stamina -= Mathf.Clamp(staminaDeclineRate * multiplier * Time.deltaTime, 0f, maxStamina);
            UpdateStaminaFactor();
        }

        private void RecoverStamina()
        {
            if (Stamina < 100) Stamina += staminaRecoveryRate * Time.deltaTime;
            UpdateStaminaFactor();
        }

        private void UpdateStaminaFactor()
        {
            StaminaFactor = Stamina > 0f ? 1f : 0f;
        }

        private void Update()
        {
            RecoverStamina();
        }

        private void Start()
        {
            // Test Vehicle Control
            // TransitionState(MovementState.MOUNTED);
        }
        
        public void TransitionState(MovementState state)
        {
            PlayerMovementState = state;
        }
    }
}