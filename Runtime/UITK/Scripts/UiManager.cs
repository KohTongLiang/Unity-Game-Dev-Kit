using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityServiceLocator;

namespace GameCore.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class UiManager : MonoBehaviour
    {
        public string[] DefaultTags;
        public UIDocument rootDocument { get; private set; }

        private Datastore datastore = new();
        public Datastore Datastore => datastore;

        private void Awake()
        {
            ServiceLocator.Global.Register(this);
            rootDocument = GetComponent<UIDocument>();
            HashSet<string> initialTags = new();
            foreach (var tag in DefaultTags)
            {
                initialTags.Add(tag);
            }
            datastore.AddOrUpdate("tags", initialTags);
        }
        
        /// <summary>
        /// Override all existing tags.
        /// </summary>
        /// <param name="tag"></param>
        public void WriteTag (string tag) => datastore.AddOrUpdate("tags", new HashSet<string>(){tag});
        
        /// <summary>
        /// Append to existing tags.
        /// </summary>
        /// <param name="tag"></param>
        public void AppendTag (string tag)
        {
            if (datastore.TryGetValue("tags", out HashSet<string> tags))
            {
                tags.Add(tag);
                datastore.AddOrUpdate("tags", tags);
            }
            else datastore.AddOrUpdate("tags", new HashSet<string>() { tag });
        }

        /// <summary>
        /// Remove from existing tags.
        /// </summary>
        /// <param name="tag"></param>
        public void RemoveTag(string tag)
        {
            if (datastore.TryGetValue("tags", out HashSet<string> tags))
            {
                tags.Remove(tag);
                datastore.AddOrUpdate("tags", tags);
            }
            else datastore.AddOrUpdate("tags", new HashSet<string>());
        }
    }
}