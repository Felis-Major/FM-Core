using Daniell.Runtime.Systems.Events;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.Events
{
    [CustomPropertyDrawer(typeof(ScriptableEvent), true)]
    public class ScriptableEventEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), new GUIContent($"<color=#d8a82d><b>[E]</b></color> <color=silver>{label.text}</color>"), new GUIStyle() { richText = true });
            EditorGUIUtility.labelWidth = 20;
            var iconContent = EditorGUIUtility.IconContent($"winbtn_mac_{(property.objectReferenceValue == null ? "close" : "max")}");
            EditorGUI.PropertyField(position, property, iconContent);
            EditorGUI.EndProperty();
        }
    }
}