using Daniell.Runtime.Systems.Localization;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.Localization
{
    [CustomPropertyDrawer(typeof(LocalizedString))]
    public class LocalizedStringDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            float fieldHeight = EditorGUIUtility.singleLineHeight;

            Rect keyLabelRect = new Rect(position.x, position.y, 25, fieldHeight);
            Rect keyRect = new Rect(position.x + 35, position.y, position.width - 57, fieldHeight);
            Rect keySelectorRect = new Rect(position.x + position.width - 20, position.y, 20, fieldHeight);
            Rect tableRect = new Rect(position.x, position.y + fieldHeight + 2, position.width, fieldHeight);

            EditorGUI.LabelField(keyLabelRect, "Key");
            EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("_key"), GUIContent.none);
            if (EditorGUI.DropdownButton(keySelectorRect, new GUIContent(""), FocusType.Passive))
            {
                GenericMenu menu = new GenericMenu();

                LocalizationTable t = (LocalizationTable)property.FindPropertyRelative("_table").objectReferenceValue;

                foreach (var v in t.Keys)
                {
                    if (v.Contains(property.FindPropertyRelative("_key").stringValue))
                    {
                        AddKeyToMenu(v);
                    }
                }

                void AddKeyToMenu(string key)
                {
                    menu.AddItem(new GUIContent(key), false, delegate
                    {
                        property.FindPropertyRelative("_key").stringValue = key;
                        property.serializedObject.ApplyModifiedProperties();
                    });
                }

                menu.ShowAsContext();
            }

            EditorGUI.PropertyField(tableRect, property.FindPropertyRelative("_table"), GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight * 2;
        }
    }
}
