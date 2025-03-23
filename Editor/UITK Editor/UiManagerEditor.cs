using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore.UI
{
    [CustomEditor(typeof(UiManager))]
    public class UiManagerEditor : Editor
    {
        private UiManager manager;
        private static string stringKey = "";
        private static string stringValue = "";

        private VisualElement inspectorElement;
        private VisualElement tagContainer;

        public override VisualElement CreateInspectorGUI()
        {
            manager = target as UiManager;
            var mInspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Game-Core/Editor/UITK Editor/Custom UI Manager Inspector.uxml");
            inspectorElement = mInspectorXML.Instantiate();
            tagContainer = inspectorElement.Q<VisualElement>("tag-container");

            BuildLoadTagsButton();

            return inspectorElement;
        }

        private void BuildLoadTagsButton()
        {
            Button loadTagBtn = inspectorElement.Q<Button>("load-tags");
            loadTagBtn.clicked += () =>
            {
                manager.Datastore.TryGetValue("tags", out HashSet<string> tags);
                tagContainer.contentContainer.Clear();
                foreach (var tag in tags)
                {
                    tagContainer.contentContainer.Add(new Label(tag));
                }
            };
            loadTagBtn.style.display= Application.isPlaying ? DisplayStyle.Flex : DisplayStyle.None;
        }
    }
}