using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class Sliding : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        private Transform orientation;

        [SerializeField] private Transform playerObj;

        private Rigidbody rb;
        private PlayerMovement playerMovement;

        [Header("Sliding")] [SerializeField] private float maxSlideTime;
        [SerializeField] private float slideForce;

        private float slideTimer;

        [SerializeField] private float slideYScale;
        private float startYScale;

        [Header("Input")] [SerializeField] private KeyCode slideKey = KeyCode.LeftControl;
        private float horizontalInput;
        private float verticalInput;

        // private bool sliding;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovement>();
            startYScale = playerObj.localScale.y;
        }


        private void Update()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
            {
                StartSlide();
            }

            if (Input.GetKeyUp(slideKey) && playerMovement.sliding)
            {
                StopSlide();
            }
        }

        private void FixedUpdate()
        {
            if (playerMovement.sliding)
            {
                SldingMovement();
            }
        }

        private void StartSlide()
        {
            playerMovement.sliding = true;
            playerObj.localScale = new Vector3(playerObj.localScale.x, slideYScale, playerObj.localScale.z);
            rb.AddForce(Vector3.down * 50f, ForceMode.Force);

            slideTimer = maxSlideTime;
        }

        private void SldingMovement()
        {
            Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
            if (!playerMovement.OnSlope() || rb.linearVelocity.y > -0.1f)
            {
                rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
                slideTimer -= Time.deltaTime;
            }
            else
            {
                rb.AddForce(playerMovement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
            }

            if (slideTimer <= 0)
            {
                StopSlide();
            }
        }

        private void StopSlide()
        {
            playerMovement.sliding = false;
            playerObj.localScale = new Vector3(playerObj.localScale.x, startYScale, playerObj.localScale.z);
        }
    }
}