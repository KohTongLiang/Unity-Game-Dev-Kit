using UnityEngine;

namespace GameCore.UI
{
    public class Components : MonoBehaviour
    {
        protected RootViewModel viewModel;

        private void OnEnable()
        {
            viewModel = GetComponent<RootViewModel>();
            viewModel.OnComponentMounted += OnMounted;
            viewModel.OnComponentDisMounted += OnDisMounted;
        }

        private void OnDisable()
        {
            viewModel.OnComponentMounted -= OnMounted;
            viewModel.OnComponentDisMounted -= OnDisMounted;
        }

        protected virtual void OnMounted()
        {
        }

        protected virtual void OnDisMounted()
        {
        }
    }
}