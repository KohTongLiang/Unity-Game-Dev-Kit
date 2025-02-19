using UnityEngine;

namespace GameCore
{
    public class SyncTransform : MonoBehaviour
    {
        public Transform targetTransform;
        [SerializeField] private bool SyncRotation;
        [SerializeField] private bool SyncPosition;
        [SerializeField] private bool SyncScale;

        private void Update()
        {
            if (targetTransform == null) return;
            if (SyncRotation) transform.rotation = targetTransform.rotation;
            if (SyncPosition) transform.position = targetTransform.position;
            if (SyncScale) transform.localScale = targetTransform.localScale;
        }
    }
}