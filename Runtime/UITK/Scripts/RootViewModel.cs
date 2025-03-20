using System;
using System.Collections.Generic;
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

        protected struct ButtonCallback
        {
            public Button button;
            public Action callback;

            public ButtonCallback(Button button, Action callback)
            {
                this.button = button;
                this.callback = callback;
            }
        }

        protected readonly List<ButtonCallback> ButtonCallbacks = new();
        private readonly Queue<Action> _callbackQueue = new();

        #endregion


        [SerializeField] protected string pageName;
        [SerializeField] protected VisualTreeAsset UIAsset;
        [SerializeField] protected GameObject[] objectDependents;

        protected VisualElement UIComponent;
        protected VisualElement Root, GameContentContainer;
        protected float templateContainerFlexGrow = 1f;
        protected TemplateContainer container;

        public readonly Stack<RootViewModel> OpenedComponents = new();

        public bool Active { get; protected set; }

        /// <summary>
        /// Mount component into UI Hierarchy.
        /// </summary>
        public virtual void Mount()
        {
            Active = true;
            GameContentContainer = UiManager.Instance.rootDocument.rootVisualElement.Q<VisualElement>("GameContent");
            container = UIAsset.Instantiate();
            UIComponent = container.contentContainer;
            UIComponent.style.flexGrow = new StyleFloat(templateContainerFlexGrow);
            GameContentContainer.contentContainer.Add(UIComponent);

            foreach (var obj in objectDependents)
            {
                obj.SetActive(true);
            }
        }

        /// <summary>
        /// Remove component from UI Hierarchy.
        /// </summary>
        public virtual void DisMount()
        {
            Active = false;
            GameContentContainer.contentContainer.Remove(UIComponent);
            // Clean up callbacks
            ButtonCallbacks.ForEach(b => b.button.clicked -= b.callback);

            while (_callbackQueue.Count > 0) _callbackQueue.Dequeue().Invoke();
            while (OpenedComponents.Count > 0) OpenedComponents.Pop().DisMount();

            UIComponent = null;

            foreach (var obj in objectDependents)
            {
                obj.SetActive(false);
            }
        }

        #region Visual Element Bindings

        /// <summary>
        /// Automatically look up button and binds button. Automatically unregister callback on disable.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="button"></param>
        /// <param name="callback"></param>
        /// <returns>Optional Button</returns>
        protected bool BindButton(string elementId, out Button button, Action callback)
        {
            button = UIComponent.Q<Button>(elementId);
            if (button == null)
            {
                Debug.LogError($"Button {elementId}: Not Found.");
                return false;
            }

            button.clicked += callback;
            ButtonCallbacks.Add(new ButtonCallback(button, callback));
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="label"></param>
        /// <param name="listenCallback"></param>
        /// <param name="unListenCallback"></param>
        /// <returns></returns>
        protected bool BindLabel(string elementId, out Label label, Action listenCallback = null,
            Action unListenCallback = null)
        {
            label = UIComponent.Q<Label>(elementId);
            if (label == null)
            {
                Debug.LogError($"Label {elementId}: Not Found.");
                return false;
            }

            listenCallback?.Invoke();
            if (unListenCallback is not null) _callbackQueue.Enqueue(unListenCallback);
            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        protected bool BindContainer(string elementId, out VisualElement container)
        {
            container = UIComponent.Q<VisualElement>(elementId);
            if (container == null)
            {
                Debug.LogError($"Container {elementId}: Not Found.");
                return false;
            }

            return true;
        }

        #endregion

        #if UNITY_EDITOR

        private void OnValidate()
        {
            gameObject.name = pageName;
        }

        #endif
    }
}