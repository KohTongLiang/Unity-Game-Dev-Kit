using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore.UI
{
    /// <summary>
    /// Base layout for view model. It shall reference the UI Asset and automatically look for the element in the UI hierarchy.
    /// </summary>
    public class RootViewModel : MonoBehaviour
    {
        #region Callback Binding

        private struct ButtonCallback
        {
            public Button button;
            public Action callback;

            public ButtonCallback(Button button, Action callback)
            {
                this.button = button;
                this.callback = callback;
            }
        }

        private readonly List<ButtonCallback> _buttonCallbacks = new();

        #endregion


        [SerializeField]
        protected string pageName;

        [SerializeField]
        private VisualTreeAsset UIAsset;

        protected VisualElement UIComponent;
        private VisualElement _root, _gameContentContainer;
        protected float templateContainerFlexGrow = 1f;

        protected virtual void OnEnable()
        {
            _gameContentContainer ??= UiManager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("GameContent");
            UIComponent ??= UIAsset.Instantiate().contentContainer;
            UIComponent.style.flexGrow = new StyleFloat(templateContainerFlexGrow);
            _gameContentContainer.contentContainer.Add(UIComponent);
        }

        protected virtual void OnDisable()
        {
            _gameContentContainer.contentContainer.Remove(UIComponent);
            // Clean up callbacks
            _buttonCallbacks.ForEach(b => b.button.clicked -= b.callback);

            UIComponent = null;
        }

        /// <summary>
        /// Automatically look up button and binds button. Automatically unregister callback on disable.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="callback"></param>
        /// <returns>Optional Button</returns>
        [CanBeNull]
        protected Button BindButton(string elementId, Action callback)
        {
            var button = UIComponent.Q<Button>(elementId);
            if (button == null)
            {
                Debug.LogError($"Button {elementId}: Not Found.");
                return null;
            }

            button.clicked += callback;
            _buttonCallbacks.Add(new(button, callback));
            return button;
        }


        #if UNITY_EDITOR

        private void OnValidate()
        {
            gameObject.name = pageName;
        }

        #endif
    }
}