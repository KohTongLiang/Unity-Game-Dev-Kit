using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityServiceLocator;

namespace GameCore.UI
{
    public class RootViewModel : MonoBehaviour
    {
        // Track callbacks attached to elements. When component is dismounted, callbacks are removed to prevent
        // dangling callbacks
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

        public enum MountPoint
        {
            GameContent,
            OverlayContent
        }

        [Header("Component Config")]
        [SerializeField] protected MountPoint mountPoint = MountPoint.GameContent;
        [SerializeField] protected string pageName;
        [SerializeField] protected VisualTreeAsset UIAsset;
        [Tooltip("Mount component when this tag exists")] [SerializeField] protected string tag;
        public string Tag => tag;

        [Header("Component Bindings")]
        [SerializeField] private LabelBinding[] labelBinding;
        [SerializeField] private ButtonBinding[] buttonBinding;

        [Header("Dependencies")]
        [Tooltip("Bind any game object that should be activated when this component mouts.")]
        [SerializeField] protected GameObject[] objectDependents;

        protected VisualElement UIComponent;
        protected VisualElement GameContentContainer;
        protected float templateContainerFlexGrow = 1f;
        protected TemplateContainer container;
        public readonly Stack<RootViewModel> OpenedComponents = new();
        [field: SerializeField]
        public bool Active { get; protected set; }
        protected UiManager uiManager;

        public delegate void OnComponentMountedEvent();
        public delegate void OnComponentDisMountedEvent();

        public event OnComponentMountedEvent OnComponentMounted;
        public event OnComponentDisMountedEvent OnComponentDisMounted;

        private void Start()
        {
            uiManager = ServiceLocator.For(this).Get<UiManager>();
            uiManager.Datastore.RegisterCallback<HashSet<string>>("tags", value =>
            {
                if (value.Contains(tag)) Mount();
                else DisMount();
            });
        }

        /// <summary>
        /// Mount component into UI Hierarchy.
        /// </summary>
        public virtual void Mount()
        {
            if (Active) return;
            Active = true;
            uiManager = ServiceLocator.For(this).Get<UiManager>();
            if (uiManager == null) return;

            if (uiManager.Datastore.TryGetValue("tags", out HashSet<string> tagList))
            {
                tagList.Add(tag);
                uiManager.Datastore.AddOrUpdate("tags", tagList);
            }

            GameContentContainer = uiManager
                .rootDocument.rootVisualElement.Q<VisualElement>(mountPoint.ToString());
            container = UIAsset.Instantiate();
            UIComponent = container.contentContainer;
            UIComponent.style.flexGrow = new StyleFloat(templateContainerFlexGrow);
            GameContentContainer.contentContainer.Add(UIComponent);
            
            if (mountPoint == MountPoint.OverlayContent) GameContentContainer.style.visibility = Visibility.Visible;

            foreach (var obj in objectDependents)
            {
                obj.SetActive(true);
            }

            foreach (var binding in labelBinding)
            {
                BindLabel(binding.elementId, out var _, binding.dataKey);
            }

            foreach (var binding in buttonBinding)
            {
                List<Action> callbacks = new();
                foreach (var buttonEvent in binding.buttonEvents)
                {
                    switch (buttonEvent.type)
                    {
                        case ButtonType.MountComponent:
                            callbacks.Add(() => uiManager.ShowPage(buttonEvent.target));
                            break;
                        case ButtonType.FireEvents:
                            callbacks.Add(() => EventManager.Instance.TriggerEvent(buttonEvent.target));
                            break;
                        case ButtonType.WriteDatastore:
                            callbacks.Add(() => uiManager.Datastore.AddOrUpdate(buttonEvent.dataKey, buttonEvent.target));
                            break;
                        case ButtonType.WriteTag:
                            callbacks.Add(
                                () =>
                                {
                                    HashSet<string> tags = new();
                                    foreach (var tag in buttonEvent.tags)
                                    {
                                        tags.Add(tag);
                                    }
                                    uiManager.Datastore.AddOrUpdate("tags", tags);
                                });
                            break;
                    }
                }

                BindButton(binding.elementId, out _, callbacks);
            }

            OnComponentMounted?.Invoke();
        }

        /// <summary>
        /// Remove component from UI Hierarchy.
        /// </summary>
        public virtual void DisMount()
        {
            if (!Active) return;
            Active = false;

            if (uiManager.Datastore.TryGetValue("tags", out HashSet<string> dataTags) && dataTags != null)
            {
                dataTags.Remove(tag);
                uiManager.Datastore.AddOrUpdate("tags", dataTags);
            }
            
            if (mountPoint == MountPoint.OverlayContent) GameContentContainer.style.visibility = Visibility.Hidden;

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

            OnComponentDisMounted?.Invoke();
        }

        #region Visual Element Bindings

        /// <summary>
        /// Automatically look up button and binds button. Automatically unregister callback on disable.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="button"></param>
        /// <param name="callback"></param>
        /// <returns>Optional Button</returns>
        public bool BindButton(string elementId, out Button button, Action callback)
        {
            button = UIComponent.Q<Button>(elementId);
            if (button == null)
            {
                return false;
            }

            button.clicked += callback;
            ButtonCallbacks.Add(new ButtonCallback(button, callback));
            return true;
        }

        /// <summary>
        /// Automatically look up button and binds button. Automatically unregister callback on disable.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="button"></param>
        /// <param name="callbacks"></param>
        /// <returns>Optional Button</returns>
        public bool BindButton(string elementId, out Button button, List<Action> callbacks)
        {
            button = UIComponent.Q<Button>(elementId);
            if (button == null)
            {
                Debug.LogError($"Button {elementId}: Not Found.");
                return false;
            }

            foreach (var action in callbacks)
            {
                button.clicked += action;
                ButtonCallbacks.Add(new ButtonCallback(button, action));
            }

            return true;
        }

        /// <summary>
        /// Bind label, passing in callbacks to listen and a callback on dismount.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="label"></param>
        /// <param name="listenCallback"></param>
        /// <param name="unListenCallback"></param>
        /// <returns></returns>
        public bool BindLabel(string elementId, out Label label)
        {
            label = UIComponent.Q<Label>(elementId);
            if (label == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Bind label, passing in callbacks to listen and a callback on dismount.
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="label"></param>
        /// <param name="listenCallback"></param>
        /// <param name="unListenCallback"></param>
        /// <returns></returns>
        public bool BindLabel(string elementId, out Label label, Action listenCallback, Action unListenCallback)
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
        /// <param name="label"></param>
        /// <param name="listenCallback"></param>
        /// <param name="unListenCallback"></param>
        /// <returns></returns>
        public bool BindLabel(string elementId, out Label outputLabel, string dataKey = "")
        {
            Label label = UIComponent.Q<Label>(elementId);

            if (label == null)
            {
                Debug.LogError($"Label {elementId}: Not Found.");
                outputLabel = null;
                return false;
            }

            if (!string.IsNullOrEmpty(dataKey))
            {
                string value = "";
                uiManager.Datastore.TryGetReference(dataKey, ref value);
                label.text = value;

                uiManager.Datastore.RegisterCallback<string>(dataKey, UpdateCallback);
                _callbackQueue.Enqueue(() =>
                {
                    uiManager.Datastore.UnregisterCallback<string>(dataKey, UpdateCallback);
                });

                void UpdateCallback(string value)
                {
                    Label label = UIComponent.Q<Label>(elementId);
                    label.text = value;
                }
            }

            outputLabel = label;
            return true;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="elementId"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public bool BindContainer(string elementId, out VisualElement container)
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