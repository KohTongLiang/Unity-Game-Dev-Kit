using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace GameCore
{
    [CustomEditor(typeof(CoroutineHandler))]
    public class CoroutineHandlerEditor : Editor
    {
        private static string filterKey = "";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var coroutineHandler = (CoroutineHandler) target;

            filterKey = EditorGUILayout.TextField("Filter Key: ", filterKey);
            if (coroutineHandler == null || !Application.isPlaying) return;

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Running Coroutines");
            coroutineHandler.DebugRunningCoroutines.FindAll(key => key.Contains(filterKey)).ForEach(key => EditorGUILayout.LabelField(key));
            EditorGUILayout.EndVertical();
        }
    }
}