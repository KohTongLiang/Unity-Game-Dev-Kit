using System;
using UnityEngine;

namespace GameCore
{
    public class DragAndDrop : MonoBehaviour
    {
        private Vector3 mousePosition;
        private Camera cam;

        public delegate void OnDragEventStarted();
        public delegate void OnDragEventStopped();

        public event OnDragEventStarted OnDragStarted;
        public event OnDragEventStopped OnDragStopped;

        private void Awake()
        {
            cam = Camera.main;
        }

        private Vector3 GetMouseWorldPos()
        {
            return cam.WorldToScreenPoint(transform.position);
        }

        private void OnMouseDown()
        {
            OnDragStarted?.Invoke();
            mousePosition = Input.mousePosition - GetMouseWorldPos();
        }

        private void OnMouseUp()
        {
            OnDragStopped?.Invoke();
        }

        private void OnMouseDrag()
        {
            Vector3 newPos = cam.ScreenToWorldPoint(Input.mousePosition - mousePosition);
            transform.position = newPos;
        }
    }
}