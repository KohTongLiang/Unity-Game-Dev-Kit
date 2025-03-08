using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UiManager : Singleton<UiManager>
    {
        [field: SerializeField]
        public List<GameObject> Pages { get; set; }

        [field: SerializeField]
        public List<GameObject> Components { get; set; }

        private GameObject currentPage { get; set; }
        public UIDocument rootDocument { get; private set; }

        public readonly Datastore Datastore = new Datastore();

        private void Awake()
        {
            if (Pages.Count <= 0) return;

            rootDocument = GetComponent<UIDocument>();
            currentPage = Pages[0];
            currentPage.SetActive(true);
        }

        /// <summary>
        /// Pass in the name of the page using its game object name
        /// </summary>
        /// <param name="name"></param>
        public void ShowPage(string name)
        {
            currentPage.SetActive(false);
            var page = Pages.Find(p => p.name == name);
            currentPage = page;
            currentPage.SetActive(true);
        }

        /// <summary>
        /// Show components.
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="show"></param>
        public void ShowComponent(string componentName, RootViewModel source, bool show = true)
        {
            var component = Components.Find(p => p.name == componentName);
            component.SetActive(show);
            source.OpenedComponents.Push(component);
        }

        /// <summary>
        /// Show floating components (not tagged to any page or component).
        /// </summary>
        /// <param name="componentName"></param>
        /// <param name="show"></param>
        public void ShowComponent(string componentName, bool show = true)
        {
            var component = Components.Find(p => p.name == componentName);
            if (show && component.activeSelf) return;
            component.SetActive(show);
        }

        public bool IsComponentVisible(string componentName)
        {
            var component = Components.Find(p => p.name == componentName);
            return component.activeSelf;
        }
    }
}