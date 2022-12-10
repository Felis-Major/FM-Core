using System.Text.RegularExpressions;
using FM.Runtime.Systems.Events;
using UnityEditor;
using UnityEngine;

namespace FM.Editor.Events
{
	[CustomEditor(typeof(EventReceiver), true)]
	public class EventReceiverEditor : UnityEditor.Editor
	{
		private const string EventTitleColor = "#6bce7b";
		private const string EventDescriptionColor = "#5f8c48";
		private const string UnassignedEventColor = "#f43a72";

		public override void OnInspectorGUI()
		{
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);

			var t = (EventReceiver)target;

			string text;
			string color = EventDescriptionColor;
			var descriptionStyle = new GUIStyle();
			descriptionStyle.richText = true;
			descriptionStyle.fontSize = 14;

			var eventNameStyle = new GUIStyle();
			eventNameStyle.richText = true;
			eventNameStyle.fontSize = 16;

			if (t.Event != null)
			{
				string eventFriendlyName = Regex.Replace(t.Event.name, "([a-z])([A-Z])", "$1 $2");

				EditorGUILayout.LabelField($"<color={EventTitleColor}><b>{eventFriendlyName}</b></color>", eventNameStyle);

				text = !string.IsNullOrEmpty(t.Event.Description) ? t.Event.Description : "No Description...";
			}
			else
			{
				color = UnassignedEventColor;
				text = "No event assigned";
			}

			EditorGUILayout.LabelField($"<color={color}><b>{text}</b></color>", descriptionStyle);

			EditorGUILayout.EndVertical();

			EditorGUILayout.Space(10);
			// Draw default inspector without the script property
			serializedObject.Update();
			DrawPropertiesExcluding(serializedObject, "m_Script");
			serializedObject.ApplyModifiedProperties();

		}
	}
}