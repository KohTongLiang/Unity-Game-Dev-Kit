using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UIElements;

namespace GameCore.UI
{
    public class Modal : MonoBehaviour
    {
        [SerializeField] protected VisualTreeAsset modalBodyAsset;

        private AsyncOperationHandle<VisualTreeAsset> assetOperation;
        private VisualElement UIComponent;
        private VisualElement _gameContentContainer, _modalMask;
        private VisualTreeAsset modalAsset;

        private WaitForSeconds transitionDelay = new(0.5f);

        protected VisualElement modalHeader, modalContent, modalFooter, modal;

        private const string ModalPath = "Assets/GameCore/UI Framework/VisualAssets/modal.uxml";

        protected virtual void OnEnable()
        {
            _gameContentContainer ??= UiManager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("GameContent");
            CoroutineHandler.Instance.StartHandlerCoroutine(GetHashCode().ToString(), RunAssetLoading());
        }

        private IEnumerator RunAssetLoading()
        {
            assetOperation = Addressables.LoadAssetAsync<VisualTreeAsset>(ModalPath);
            yield return assetOperation;

            if (assetOperation.Status == AsyncOperationStatus.Succeeded)
            {
                modalAsset = assetOperation.Result;
                var modalTemplate = modalAsset.Instantiate();
                UIComponent = modalTemplate.contentContainer;
                UIComponent.style.flexGrow = new StyleFloat(1f);
                UIComponent.AddToClassList("modal-template");
                _modalMask = UIComponent.Q<VisualElement>("ModalMask");

                _gameContentContainer.Add(UIComponent);
                modal = UIComponent.Q<VisualElement>("Modal");
                CoroutineHandler.Instance.StartHandlerCoroutine("CloseModal", AnimateModal(true));
                LoadModalContent();
            }
        }

        protected virtual void OnDisable()
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
        }

        private IEnumerator AnimateModal(bool show)
        {
            yield return new WaitForSeconds(0.01f);
            if (show)
            {
                modal.style.translate = new Translate(0f, 0f, 0f);
                yield return transitionDelay;
                _modalMask.RegisterCallback<ClickEvent>(CloseModal);
            }
            else
            {
                modal.style.translate = new Translate(0f, new Length(300, LengthUnit.Percent), 0f);
                yield return transitionDelay;
                _gameContentContainer.Remove(UIComponent);
                Addressables.Release(assetOperation);
            }
        }

        protected virtual void UnloadModalContent()
        {
            _modalMask.UnregisterCallback<ClickEvent>(CloseModal);
            CoroutineHandler.Instance.StartHandlerCoroutine("CloseModal", AnimateModal(false));
            modalAsset = null;
        }

        private void CloseModal(ClickEvent evt)
        {
            gameObject.SetActive(false);
        }
    }
}