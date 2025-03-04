using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    [SerializeField] private Transform playerCameraPos;

    // Update is called once per frame
    private void Update()
    {
        transform.position = playerCameraPos.position;
        transform.rotation = playerCameraPos.rotation;
    }
}