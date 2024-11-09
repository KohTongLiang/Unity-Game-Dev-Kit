using System;
using UnityEngine;

namespace GameCore
{
    public class CameraCharacterHeadFollow : MonoBehaviour
    {
        [SerializeField] private Transform armatureHead;

        private void Update()
        {
            transform.position = armatureHead.position;
        }
    }
}