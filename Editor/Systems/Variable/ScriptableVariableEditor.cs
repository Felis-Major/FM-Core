using System.Collections;
using System.Collections.Generic;
using FM.Runtime.Systems.Variables;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(ScriptableVariable), editorForChildClasses: true)]
public class ScriptableVariableEditor : Editor
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public override VisualElement CreateInspectorGUI()
	{
		var rootElement = new VisualElement();

		// Value field
		SerializedProperty propValue = serializedObject.FindProperty("_value");
		var propValueField = new PropertyField(propValue);
		propValueField.SetEnabled(false);
		rootElement.Add(propValueField);

		// Enable override field
		SerializedProperty propEnableOverride = serializedObject.FindProperty("_isOverrideValueEnabled");
		var propEnableOverrideField = new PropertyField(propEnableOverride, "Enable Override");
		rootElement.Add(propEnableOverrideField);

		// Override value field
		SerializedProperty propOverrideValue = serializedObject.FindProperty("_overrideValue");
		var propOverrideValueField = new PropertyField(propOverrideValue);
		propOverrideValueField.SetEnabled(propEnableOverride.boolValue);

		// Track changes to the override value
		rootElement.TrackPropertyValue(propEnableOverride, (x) =>
		{
			propOverrideValueField.SetEnabled(x.boolValue);
		});

		rootElement.Add(propOverrideValueField);

		return rootElement;
	}
}
