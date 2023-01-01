using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.Callbacks;
#endif

/// <summary>
/// Component used to leave notes and images onto an object
/// </summary>
public class ImageReference : MonoBehaviour
{
    /* ==========================
     * > Properties
     * -------------------------- */

    /// <summary>
    /// Notes
    /// </summary>
    public string Notes => _notes;

    /// <summary>
    /// List of image links
    /// </summary>
    public string[] Links => _links;


    /* ==========================
     * > Private Serialized Fields
     * -------------------------- */

    [SerializeField]
    [Tooltip("Notes")]
    private string _notes = string.Empty;

    [SerializeField]
    [Tooltip("List of image links")]
    private string[] _links = new string[0];


#if UNITY_EDITOR
    /// <summary>
    /// Remove all the <see cref="ImageReference"/> components from the scene after building the scene
    /// </summary>
    [PostProcessScene()]
    public static void RemoveOnPostProcessScene()
    {
        // Find and remove all the image references
        ImageReference[] imageReferencesComponents = FindObjectsOfType<ImageReference>();
        for (int i = 0; i < imageReferencesComponents.Length; i++)
        {
            DestroyImmediate(imageReferencesComponents[i]);
        }

        if (imageReferencesComponents.Length > 0)
        {
            Debug.Log($"Removed {imageReferencesComponents.Length} Image Reference components");
        }
    }
#endif
}
