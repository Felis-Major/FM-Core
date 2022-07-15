using FM.Runtime.References;
using UnityEditor;
using UnityEngine;

namespace FM.Editor.References
{
    [CustomEditor(typeof(RuntimeReferenceSetter), true)]
    public class RuntimeReferenceSetterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            /*
            base.OnInspectorGUI();
            var targetReferenceSetter = (RuntimeReferenceSetter)target;

            if (targetReferenceSetter.Reference != null)
            {
                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                if (GUILayout.Button("Set"))
                {
                    targetReferenceSetter.SetReference();
                }

                if (GUILayout.Button("Clear"))
                {
                    targetReferenceSetter.ClearReference();
                }

                EditorGUILayout.EndHorizontal();
            }*/
        }
    }
}