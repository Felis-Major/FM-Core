using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace FM.Editor.Tools
{
	/// <summary>
	/// Window containing various Level Design editor utilities
	/// </summary>
	public class SnappingTools : EditorWindow
	{
		[MenuItem("Felis Major/Tools/Snapping Tools")]
		public static void Open()
		{
			SnappingTools window = GetWindow<SnappingTools>();
			window.titleContent = new GUIContent("Snapping Tools");
			window.Show();
		}

		private void OnEnable()
		{
			// Instantiate & add visual tree
			EditorUtilities.GetVisualTreeAssetFromEditorScript(this, "SnappingToolsEditorWndow.uxml", out UnityEngine.UIElements.VisualTreeAsset editorTemplateAsset);

			rootVisualElement.Add(editorTemplateAsset.Instantiate());

			// Snap Selected button
			Button snapSelectedButton = rootVisualElement.Q<Button>("SnapSelectedButton");
			snapSelectedButton.clicked += () => SnapSelectedToGround();

			// Use Physics toggle
			Toggle usePhysicsToggle = rootVisualElement.Q<Toggle>("UsePhysicsToggle");
			usePhysicsToggle.SetEnabled(false);

		}

		private void OnDisable()
		{

		}

		/// <summary>
		/// Snap selected objects to either y 0 or bounds of the object below
		/// </summary>
		public void SnapSelectedToGround()
		{
			Transform[] currentSelection = Selection.transforms;
			Undo.RecordObjects(currentSelection, "Undo snap to ground");
			for (int i = 0; i < currentSelection.Length; i++)
			{
				Transform obj = currentSelection[i];
				SnapObjectToGround(obj.gameObject);
			}
		}

		/// <summary>
		/// Snap a <see cref="GameObject"/> to the first surface hit below
		/// </summary>
		/// <param name="gameObject">Object to be snapped to the ground</param>
		private void SnapObjectToGround(GameObject gameObject)
		{
			// Get the gameobject transform
			Transform transform = gameObject.transform;

			// Find bounds using collider or renderer
			Renderer renderer = gameObject.GetComponent<Renderer>();
			Collider collider = gameObject.GetComponent<Collider>();

			Bounds bounds;
			if (renderer != null)
			{
				bounds = renderer.bounds;
			}
			else if (collider != null)
			{
				bounds = collider.bounds;
			}
			else
			{
				// Don't execute if there are no bounds
				Debug.LogWarning($"Cannot snap object {gameObject.name} because there's no renderer or collider");
				return;
			}

			// Default the new position to the ground
			float x = transform.position.x;
			float y = bounds.extents.y;
			float z = transform.position.z;

			// If there was a hit, snap to the closest position
			bool boxCast = Physics.BoxCast(bounds.center, bounds.extents, Vector3.down, out RaycastHit hit);
			if (boxCast)
			{
				y = hit.point.y + bounds.extents.y;
			}

			transform.position = new Vector3(x, y, z);
		}
	}
}
