using UnityEngine;

namespace GameCore
{
    public class PlayerInteractable : MonoBehaviour, IPickable, IInteractable
    {
        [SerializeField] private LayerMask targetMask;
        [SerializeField] private float maxDistance = 2f;
        public bool interactable = true;

        private PlayerInteractor interactor;
        private bool hovered = false;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (LayerUtilities.CompareLayerToMask(targetMask, other.gameObject.layer) && interactable)
            {
                interactor = other.gameObject.GetComponent<PlayerInteractor>();
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (LayerUtilities.CompareLayerToMask(targetMask, other.gameObject.layer) && interactable)
            {
                interactor = other.gameObject.GetComponent<PlayerInteractor>();
                OnHoverLeave();
            }
        }

        private void Update()
        {
            if (interactor is null) return;
            var direction = transform.position - interactor.InteractorView.position;
            var distance = direction.magnitude;
            var dotProduct = Vector3.Dot(direction.normalized, interactor.transform.forward);
            if (dotProduct > 0.5f && distance < maxDistance) // adjust maxDistance to your needs
            {
                OnHover();
            }
            else
            {
                OnHoverLeave();
            }
        }

        public virtual void OnHover()
        {
            if (hovered) return;
            hovered = true;
            interactor.CurrentInteractable = this;
        }

        public virtual void OnHoverLeave()
        {
            if (!hovered) return;
            hovered = false;
            interactor.CurrentInteractable = null;
        }

        public virtual void OnPickup()
        {
        }

        public virtual void OnDropped()
        {
        }

        public virtual void Interact()
        {
            Debug.Log("Interact");
        }
    }
}