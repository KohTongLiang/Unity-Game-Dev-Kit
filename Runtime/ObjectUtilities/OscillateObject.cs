using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class OscillateObject : MonoBehaviour
    {
        private Vector3 startingPosition;
        private float movementFactor;
        [SerializeField] private Vector3 movementVector;
        [SerializeField] private float period = 10f;

        // Start is called before the first frame update
        void Start()
        {
            startingPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (period == Mathf.Epsilon)
            {
                return;
            }

            float cycles = Time.time / period;
            const float tau = Mathf.PI * 2f;
            float rawSinWave = Mathf.Sin(cycles * tau);

            movementFactor = rawSinWave / 2f + 0.5f;

            Vector3 offset = movementVector * movementFactor;
            transform.position = startingPosition + offset;
        }
    }
}