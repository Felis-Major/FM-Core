using Daniell.Runtime.Helpers.GUIDSystem;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.GUIDSystem
{
    [CustomPropertyDrawer(typeof(GUIDAttribute))]
    public class GUIDPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Create the property
            EditorGUI.BeginProperty(position, label, property);

            // Show button to generate ID
            if (GUI.Button(new Rect(position.x, position.y, 100, 18), "Generate ID") || property.stringValue == "")
            {
                // Generate ID
                property.stringValue = GUIDGenerator.GenerateGUID();
            }

            // Show current ID
            EditorGUI.LabelField(new Rect(position.x + 102, position.y, position.width - 102, 18), "Save ID: " + property.stringValue, EditorStyles.helpBox);

            EditorGUI.EndProperty();
        }
    }
}