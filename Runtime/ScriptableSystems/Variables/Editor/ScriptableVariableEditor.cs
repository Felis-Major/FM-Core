using FM.Runtime.Systems.Variables;
using UnityEditor;
using UnityEditor.UIElements;
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

		SerializedProperty propDefaultValue = serializedObject.FindProperty("_defaultValue");
		var propDefaultValueField = new PropertyField(propDefaultValue);
		rootElement.Add(propDefaultValueField);

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
