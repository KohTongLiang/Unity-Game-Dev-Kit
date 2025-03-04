using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace GameCore.UI
{
    public class Modal : RootViewModel
    {
        [SerializeField] private string Directory = "Assets/GameCore/UI/VisualAssets/";

        [Header("Modal Stylesheet Override")]
        [SerializeField] private StyleSheet modalStyleSheet;

        private AsyncOperationHandle<VisualTreeAsset> assetOperation;
        private VisualElement _modalMask;
        private VisualTreeAsset modalAsset;

        private WaitForSeconds transitionDelay = new(0.5f);

        protected VisualElement modalHeader, modalContent, modalFooter, modal;

        private string _modalPath;

        // Child Template
        protected VisualElement ModalContentAsset;

        protected CoroutineHandler coroutines;

        protected override void OnEnable()
        {
            coroutines = CoroutineHandler.Instance;
            _modalPath = Directory + "modal.uxml";
            GameContentContainer ??= UiManager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("GameContent");
            _ = coroutines.StartCoroutineIfNotRunning(GetHashCode().ToString(), RunAssetLoading());
        }

        private IEnumerator RunAssetLoading()
        {
            assetOperation = Addressables.LoadAssetAsync<VisualTreeAsset>(_modalPath);
            yield return assetOperation;

            if (assetOperation.Status == AsyncOperationStatus.Succeeded)
            {
                while (coroutines.IsCoroutineRunning("CloseModal"))
                {
                    yield return null;
                }

                modalAsset = assetOperation.Result;
                var modalTemplate = modalAsset.Instantiate();
                UIComponent = modalTemplate.contentContainer;
                UIComponent.style.flexGrow = new StyleFloat(1f);
                UIComponent.AddToClassList("modal-template");
                _modalMask = UIComponent.Q<VisualElement>("ModalMask");

                GameContentContainer.Add(UIComponent);
                modal = UIComponent.Q<VisualElement>("Modal");
                _ = coroutines.StartCoroutineIfNotRunning("OpenModal", OpenModal());

                if (modalStyleSheet != null)
                {
                    modal.styleSheets.Clear();
                    modal.styleSheets.Add(modalStyleSheet);
                }

                LoadModalContent();
            }
            else if (assetOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogWarning($"Failed to load modal asset for {_modalPath}");
            }
        }

        protected override void OnDisable()
        {
            UnloadModalContent();
        }

        /// <summary>
        /// Load UI once modal loaded in
        /// </summary>
        protected virtual void LoadModalContent()
        {
            modalHeader = UIComponent.Q<VisualElement>("ModalHeader");
            modalContent = UIComponent.Q<VisualElement>("ModalContent");
            modalFooter = UIComponent.Q<VisualElement>("ModalFooter");

            ModalContentAsset = UIAsset.Instantiate();

            var childAsset = UIAsset.Instantiate().contentContainer;
            childAsset.style.flexGrow = new StyleFloat(templateContainerFlexGrow);
            modalContent.Add(childAsset);
        }

        private IEnumerator OpenModal()
        {
            while (coroutines.IsCoroutineRunning("CloseModal"))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
            modal.style.translate = new Translate(0f, 0f, 0f);
            yield return transitionDelay;
            _modalMask.RegisterCallback<ClickEvent>(CloseModal);
        }

        private IEnumerator CloseModal()
        {
            while (coroutines.IsCoroutineRunning("OpenModal"))
            {
                yield return null;
            }

            yield return new WaitForSeconds(0.01f);
            modal.style.translate = new Translate(0f, new Length(300, LengthUnit.Percent), 0f);
            yield return transitionDelay;
            GameContentContainer.Remove(UIComponent);
            Addressables.Release(assetOperation);
        }

        protected virtual void UnloadModalContent()
        {
            modal.styleSheets.Clear();
            _modalMask.UnregisterCallback<ClickEvent>(CloseModal);
            _ = coroutines.StartCoroutineIfNotRunning("CloseModal", CloseModal());
            modalAsset = null;
        }

        private void CloseModal(ClickEvent evt)
        {
            gameObject.SetActive(false);
        }
    }
}