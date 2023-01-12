using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FM.Editor
{
	/// <summary>
	/// Collection of utilities for the unity editor
	/// </summary>
	public static class EditorUtilities
	{
		/// <summary>
		/// Find and load a <see cref="VisualTreeAsset"/> placed in the same directory as an editor script.
		/// </summary>
		/// <param name="editorScript">Editor script instance</param>
		/// <param name="editorTemplateName">Name of the <see cref="VisualTreeAsset"/></param>
		/// <param name="editorTemplateAsset"><see cref="VisualTreeAsset"/> if found or null</param>
		/// <returns>True if the asset was loaded</returns>
		public static bool GetVisualTreeAssetFromEditorScript(
			ScriptableObject editorScript,
			string editorTemplateName,
			out VisualTreeAsset editorTemplateAsset)
		{
			// Build path to the local asset
			// Convert editor script instance to the mono source
			var editorScriptAsObject = MonoScript.FromScriptableObject(editorScript);

			// Find the source path
			string path = AssetDatabase.GetAssetPath(editorScriptAsObject);

			// Replace the editor script name 
			string editorScriptObject = $"{editorScriptAsObject.name}.cs";
			path = path.Replace(editorScriptObject, editorTemplateName);

			editorTemplateAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);

			return editorTemplateAsset != null;
		}
	}
}

