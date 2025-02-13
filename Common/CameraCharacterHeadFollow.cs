using UnityEngine;

namespace GameCore
{
    public class CameraCharacterHeadFollow : MonoBehaviour
    {
        [SerializeField] private Transform armatureHead;

        private void Update()
        {
            if (armatureHead == null) return;
            transform.position = armatureHead.position;
        }
    }
}