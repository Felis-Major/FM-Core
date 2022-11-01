using UnityEditor;
using UnityEngine;

namespace FM.Editor.Tools.LevelDesign
{
	/// <summary>
	/// Class containing various Level Design editor utilities
	/// </summary>
	public static class LevelDesignUtilities
	{
		/// <summary>
		/// Snap selected objects to either y 0 or bounds of the object below
		/// </summary>
		[MenuItem("Felis Major/Tools/Snap Selected Objects To Ground")]
		public static void SnapSelectedToGround()
		{
			Transform[] currentSelection = Selection.transforms;
			for (int i = 0; i < currentSelection.Length; i++)
			{
				Transform obj = currentSelection[i];
				SnapObjectToGround(obj.gameObject);
			}
		}

		/// <summary>
		/// Snap an object to the ground
		/// </summary>
		/// <param name="gameObject">Object to be snapped to the ground</param>
		private static void SnapObjectToGround(GameObject gameObject)
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
