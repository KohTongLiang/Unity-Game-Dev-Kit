using UnityEngine;

namespace GameCore.UI
{
    public class Components : MonoBehaviour
    {
        protected RootViewModel ViewModel;

        private void OnEnable()
        {
            ViewModel = GetComponent<RootViewModel>();
            ViewModel.OnComponentMounted += OnMounted;
            ViewModel.OnComponentDisMounted += OnDisMounted;
        }

        private void OnDisable()
        {
            ViewModel.OnComponentMounted -= OnMounted;
            ViewModel.OnComponentDisMounted -= OnDisMounted;
        }

        protected virtual void OnMounted()
        {
        }

        protected virtual void OnDisMounted()
        {
        }
    }
}