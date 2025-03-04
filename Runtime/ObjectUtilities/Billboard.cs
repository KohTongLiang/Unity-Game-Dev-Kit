#if UNITY_6000
using Unity.Cinemachine;
#else
using Cinemachine;
#endif

using UnityEngine;
using UnityEngine.Serialization;

namespace GameCore
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private bool billboardX = false;
        [SerializeField] private bool billboardY = false;
        [SerializeField] private bool billboardZ = false;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (_camera is null) return;
            transform.rotation = Quaternion.Euler(
                billboardX ? _camera.transform.rotation.eulerAngles.x : 0f,
                billboardY ? _camera.transform.rotation.eulerAngles.y : 0f,
                billboardZ ? _camera.transform.rotation.eulerAngles.z : 0f
            );
        }
    }
}