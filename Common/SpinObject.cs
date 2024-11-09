using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinObject : MonoBehaviour
{
    [SerializeField] private Vector3 rotationAxis = Vector3.up;
    [SerializeField] private float rotationSpeed = 10f;

    private void FixedUpdate()
    {
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
