using Daniell.Runtime.Systems.Events;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.Events
{
    [CustomEditor(typeof(EventReceiver), true)]
    public class EventReceiverEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var t = (EventReceiver)target;

            string text = "";
            string color = "#97f229";
            GUIStyle descriptionStyle = new GUIStyle();
            descriptionStyle.richText = true;
            descriptionStyle.fontSize = 14;

            GUIStyle eventNameStyle = new GUIStyle();
            eventNameStyle.richText = true;
            eventNameStyle.fontSize = 16;

            if (t.Event != null)
            {
                string eventFriendlyName = Regex.Replace(t.Event.name, "([a-z])([A-Z])", "$1 $2");

                EditorGUILayout.LabelField($"<color=#3afff5><b>{eventFriendlyName}</b></color>", eventNameStyle);


                if (!string.IsNullOrEmpty(t.Event.Description))
                {
                    text = t.Event.Description;
                }
                else
                {
                    text = "No Description...";
                }
            }
            else
            {
                color = "#f43a72";
                text = "No event assigned";
            }

            EditorGUILayout.LabelField($"<color={color}><b>{text}</b></color>", descriptionStyle);

            EditorGUILayout.Space();

            base.OnInspectorGUI();
        }
    }
}