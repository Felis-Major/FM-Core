using FM.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace FM.Editor.Drawers
{
	/// <summary>
	/// <see cref="PropertyDrawer"/> for <see cref="FieldNameAttribute"/>
	/// </summary>
	[CustomPropertyDrawer(typeof(FieldNameAttribute))]
	public class FieldNameAttributeDrawer : PropertyDrawer
	{
		/* ==========================
		 * > Methods
		 * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var fieldNameAttribute = (FieldNameAttribute)attribute;
			EditorGUI.PropertyField(position, property, new GUIContent(fieldNameAttribute.Name));
		}
	}
}