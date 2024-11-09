using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace GameCore
{
    public class IdleCamera : MonoBehaviour
    {
        public List<CinemachineVirtualCamera> cameras;
        public float switchTime = 5f;
        private int _currentCameraIndex = 0;

        private void Start()
        {
            StartCoroutine(SwitchCameras());
        }

        private IEnumerator SwitchCameras()
        {
            while (true)
            {
                // Disable all cameras
                foreach (var camera in cameras)
                {
                    camera.gameObject.SetActive(false);
                }

                // Enable the current camera
                cameras[_currentCameraIndex].gameObject.SetActive(true);

                // Wait for the specified amount of time
                yield return new WaitForSeconds(switchTime);

                // Switch to the next camera
                _currentCameraIndex++;

                // If we've reached the end of the camera list, start over
                if (_currentCameraIndex >= cameras.Count)
                {
                    _currentCameraIndex = 0;
                }
            }
        }
    }
}