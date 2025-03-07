using System;
using UnityEngine;

namespace GameCore
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("View of the player where the interaction will be done.")]
        [SerializeField] private Transform playerView;
        public Transform InteractorView => playerView;

        [NonSerialized] public float CurrentInteractableInView;
        [NonSerialized] public PlayerInteractable CurrentInteractable;

        public void Interact()
        {
            CurrentInteractable.Interact();
        }
    }
}