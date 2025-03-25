using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
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

        private VisualElement inspectorElement, tagContainer, tagEditorContainer, tagRuntimeContainer,newTagContainer;
        private Button addTagBtn, loadTagBtn;

        private SerializedProperty defaultTagProperty;
        private SerializedObject so;
        

        private int tagSize = 0;

        private void OnEnable()
        {
            so = serializedObject;
            manager = target as UiManager;
            defaultTagProperty = so.FindProperty("DefaultTags");
        }
        
        public override VisualElement CreateInspectorGUI()
        {
            var mInspectorXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this))) ?? string.Empty, "Custom UI Manager Inspector.uxml"));
            inspectorElement = mInspectorXML.Instantiate();
            tagContainer = inspectorElement.Q<VisualElement>("tag-container");
            tagEditorContainer = inspectorElement.Q<VisualElement>("tag-management-editor");
            tagRuntimeContainer = inspectorElement.Q<VisualElement>("tag-management-runtime");

            if (Application.isPlaying)
            {
                LoadTagSetup();
            }
            else
            {
                BuildTagSetup();
            }
            return inspectorElement;
        }

        #region Tag Management

        private void BuildTagSetup()
        {
            addTagBtn = inspectorElement.Q<Button>("add-tag-btn");
            newTagContainer = inspectorElement.Q<VisualElement>("new-tag-container");
            PopulateTagList();
            tagSize = defaultTagProperty.arraySize;
            addTagBtn.clicked += AddTagFieldCallback;
        }

        private void PopulateTagList()
        {
            newTagContainer.contentContainer.Clear();
            for (var i = 0; i < defaultTagProperty.arraySize; i++)
            {
                VisualElement tagRow = new();
                // Retrieve respective values
                var element = defaultTagProperty.GetArrayElementAtIndex(i).stringValue;

                // Create TextField
                var textField = new TextField("Tag")
                {
                    value = element
                };
                textField.style.flexGrow = 1;
                textField.BindProperty(defaultTagProperty.GetArrayElementAtIndex(i));
                textField.RegisterValueChangedCallback(TextFieldChangeCallback);

                // Create Clear Button
                var clearTagBtn = new Button()
                {
                    text = "Remove"
                };
                var idx = i;
                clearTagBtn.clicked += ClearCallback;

                // Closure to clear this line of element
                void ClearCallback()
                {
                    clearTagBtn.clicked -= ClearCallback;
                    textField.UnregisterValueChangedCallback(TextFieldChangeCallback);
                    ClearTagField(idx);
                }

                tagRow.contentContainer.Add(textField);
                tagRow.contentContainer.Add(clearTagBtn);
                tagRow.AddToClassList("dynamic-row");
                newTagContainer.contentContainer.Add(tagRow);
            }

            tagSize = defaultTagProperty.arraySize;
            so.ApplyModifiedProperties();
        }

        private void AddTagFieldCallback()
        {
            defaultTagProperty.arraySize = tagSize++;
            defaultTagProperty.InsertArrayElementAtIndex(tagSize - 1);
            defaultTagProperty.GetArrayElementAtIndex(tagSize - 1).stringValue = string.Empty;
            PopulateTagList();
        }

        private void TextFieldChangeCallback(ChangeEvent<string> evt)
        {
            so.ApplyModifiedProperties();
        }

        private void ClearTagField(int index)
        {
            defaultTagProperty.DeleteArrayElementAtIndex(index);
            PopulateTagList();
            so.ApplyModifiedProperties();
        }

        private void LoadTagSetup()
        {
            loadTagBtn = inspectorElement.Q<Button>("load-tags");
            loadTagBtn.clicked += () =>
            {
                manager.Datastore.TryGetValue("tags", out HashSet<string> tags);
                tagContainer.contentContainer.Clear();
                foreach (var tag in tags) tagContainer.contentContainer.Add(new Label(tag));
            };
            loadTagBtn.style.display = Application.isPlaying ? DisplayStyle.Flex : DisplayStyle.None;
        }

        #endregion
    }
}