using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityServiceLocator;

namespace GameCore.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UiManager : Singleton<UiManager>
    {
        [field: SerializeField]
        public List<RootViewModel> Pages { get; set; }

        [field: SerializeField]
        public List<OverlayViewModel> Overlays { get; set; }

        private RootViewModel currentPage { get; set; }
        public UIDocument rootDocument { get; private set; }


        private Datastore datastore = new();
        public Datastore Datastore => datastore;

        private void Awake()
        {
            if (Pages.Count <= 0) return;
            ServiceLocator.Global.Register(this);
            rootDocument = GetComponent<UIDocument>();
            currentPage = Pages[0];
            datastore.AddOrUpdate("tags", new HashSet<string>());
            if (!currentPage.Active) currentPage.Mount();
        }

        /// <summary>
        /// Pass in the name of the page using its game object name
        /// </summary>
        /// <param name="name"></param>
        public void ShowPage(string name)
        {
            if (currentPage.Active) currentPage.DisMount();
            var page = Pages.Find(p => p.name == name);
            currentPage = page;
            if (!currentPage.Active) currentPage.Mount();
        }

        /// <summary>
        /// Show components.
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="show"></param>
        public void ShowComponent(string componentName, RootViewModel source, bool show = true)
        {
            var component = Overlays.Find(p => p.name == componentName);

            if (show)
            {
                if (!component.Active) component.Mount();
            }
            else
            {
                if (component.Active) component.DisMount();
            }

            source.OpenedComponents.Push(component);
        }

        /// <summary>
        /// Show floating components (not tagged to any page or component).
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="show"></param>
        public void ShowComponent(string componentName, bool show = true)
        {
            var component = Overlays.Find(p => p.name == componentName);
            if (show && component.Active) return;

            if (show)
            {
                if (!component.Active) component.Mount();
            }
            else
            {
                if (component.Active) component.DisMount();
            }
        }

        public bool IsComponentVisible(string componentName)
        {
            var component = Overlays.Find(p => p.name == componentName);
            return component.Active;
        }
    }
}