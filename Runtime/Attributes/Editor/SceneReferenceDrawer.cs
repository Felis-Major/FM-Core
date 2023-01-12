using FM.Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace FM.Editor.Drawers
{
	/// <summary>
	/// <see cref="PropertyDrawer"/> for <see cref="SceneReferenceAttribute"/>
	/// </summary>
	[CustomPropertyDrawer(typeof(SceneReferenceAttribute))]
	public class SceneReferenceDrawer : PropertyDrawer
	{
		/* ==========================
		 * > Methods
		 * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.String)
			{
				// Display the scene object with an object field
				SceneAsset sceneObject = GetSceneFromName(property.stringValue);
				Object scene = EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);

				// If a scene object was assigned in the editor
				if (scene != null)
				{
					SceneAsset sceneObj = GetSceneFromName(scene.name);

					if (sceneObj != null)
					{
						property.stringValue = scene.name;
					}
				}
				else
				{
					property.stringValue = "";
				}
			}
			else
			{
				string errorMessage = $"Invalid attribute for field type '{property.propertyType}'.";
				EditorGUI.LabelField(position, label.text, errorMessage);
			}
		}

		/// <summary>
		/// Get <see cref="SceneAsset"/> from its name. Will look into the Editor build settings
		/// </summary>
		/// <param name="sceneName">Name of the scene</param>
		/// <returns>The <see cref="SceneAsset"/> or null if not found</returns>
		protected SceneAsset GetSceneFromName(string sceneName)
		{
			// Return null if no name was specified
			if (string.IsNullOrEmpty(sceneName))
			{
				return null;
			}

			// Find the first scene matching the scene name
			for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
			{
				EditorBuildSettingsScene editorScene = EditorBuildSettings.scenes[i];

				if (editorScene.path.IndexOf(sceneName) != -1)
				{
					return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
				}
			}

			// Display a warning if no scene was found
			Debug.LogWarning($"Scene {sceneName} couldn't be found. Ensure it's in the build settings.");

			return null;
		}
	}
}