using UnityEngine;

namespace GameCore
{
    public class PlayerInteractable : MonoBehaviour, IHover, IInteractable
    {
        [SerializeField] private LayerMask interactorMask;
        [SerializeField] private float maxDistance = 2f;
        [SerializeField] private float dotProductThreshold = 0.5f;

        public bool persistent = false;
        public bool interactable = true;

        private PlayerInteractor interactor;
        public PlayerInteractor Interactor => interactor;

        private bool hovered = false;

        public delegate void OnHoverEnterEvent();
        public delegate void OnHoverLeaveEvent();
        public delegate void OnInteractEvent();

        public event OnHoverEnterEvent OnHoverEnterTrigger;
        public event OnHoverLeaveEvent OnHoverLeaveTrigger;
        public event OnInteractEvent OnInteractTrigger;

        private float dotProduct;

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (LayerUtilities.CompareLayerToMask(interactorMask, other.gameObject.layer) && interactable)
            {
                interactor = other.gameObject.GetComponent<PlayerInteractor>();
            }
        }

        protected virtual void OnTriggerExit(Collider other)
        {
            if (LayerUtilities.CompareLayerToMask(interactorMask, other.gameObject.layer) && interactable)
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
            dotProduct = Vector3.Dot(direction.normalized, interactor.InteractorView.transform.forward);

            if (dotProduct > dotProductThreshold && distance < maxDistance) // adjust maxDistance to your needs
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
            bool toSet = interactor.CurrentInteractable == null || interactor.CurrentInteractableInView < dotProduct;

            if (!toSet || hovered) return;
            hovered = true;

            // Override current interactable if
            if (toSet)
            {
                interactor.CurrentInteractableInView = dotProduct;
                interactor.CurrentInteractable = this;
            }

            OnHoverEnterTrigger?.Invoke();
        }

        public virtual void OnHoverLeave()
        {
            if (!hovered) return;
            hovered = false;
            interactor.CurrentInteractable = interactor.CurrentInteractable == this ? null : interactor.CurrentInteractable;
            OnHoverLeaveTrigger?.Invoke();
        }

        public virtual void Interact()
        {
            Interactor.CurrentInteractable = persistent ? this : null;
            OnInteractTrigger?.Invoke();
        }
    }
}