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

        public override VisualElement CreateInspectorGUI()
        {
            manager = target as UiManager;
            return base.CreateInspectorGUI();
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (!Application.isPlaying || manager == null) return;

            EditorGUILayout.LabelField("Test Datastore Label");
            stringKey = EditorGUILayout.TextField("Datastore Key", stringKey);
            stringValue = EditorGUILayout.TextField("Datastore Value", stringValue);
            if (GUILayout.Button("Update Datastore"))
            {
                manager.Datastore.AddOrUpdate(stringKey, stringValue);
            }
        }
    }
}