using FM.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveToolEditorWindow : EditorWindow
{
	[MenuItem("Felis Major/Tools/Save Tool")]
	public static void Open()
	{
		SaveToolEditorWindow window = GetWindow<SaveToolEditorWindow>();
		window.titleContent = new GUIContent("Save Tool");
		window.Show();
	}

	private bool _isSaveOnPlayEnabled = false;
	private bool _isAutoSaveEnabled = false;
	private int _saveInterval = 1;

	private void OnEnable()
	{
		// Add visual tree instance
		EditorUtilities.GetVisualTreeAssetFromEditorScript(
			this,
			"SaveToolEditorTemplate.uxml",
			out VisualTreeAsset visualTreeAsset);

		rootVisualElement.Add(visualTreeAsset.Instantiate());

		// Save on play
		Toggle saveOnPlayToggle = rootVisualElement.Q<Toggle>("SaveOnPlayToggle");

		if (EditorPrefs.HasKey("isSaveOnPlayEnabled"))
		{
			bool value = EditorPrefs.GetBool("isSaveOnPlayEnabled");
			saveOnPlayToggle.SetValueWithoutNotify(value);
			_isSaveOnPlayEnabled = value;
		}

		_isSaveOnPlayEnabled = saveOnPlayToggle.value;
		saveOnPlayToggle.RegisterValueChangedCallback(x =>
		{
			_isSaveOnPlayEnabled = x.newValue;
			EditorPrefs.SetBool("isSaveOnPlayEnabled", x.newValue);
		});

		// Auto save
		Toggle autoSaveToggle = rootVisualElement.Q<Toggle>("AutoSaveToggle");
		autoSaveToggle.SetEnabled(false);
		//_isAutoSaveEnabled = autoSaveToggle.value;
		//autoSaveToggle.RegisterValueChangedCallback(x => _isAutoSaveEnabled = x.newValue);

		// Save interval
		SliderInt saveIntervalSlider = rootVisualElement.Q<SliderInt>("SaveIntervalSlider");
		saveIntervalSlider.SetEnabled(false);
		//_saveInterval = saveIntervalSlider.value;
		//saveIntervalSlider.RegisterValueChangedCallback(x => _saveInterval = x.newValue);

		// Register to play mode callback
		EditorApplication.playModeStateChanged += EditorApplication_playModeStateChanged;
	}

	private void OnDisable()
	{
		// Unregister from play mode callback
		EditorApplication.playModeStateChanged -= EditorApplication_playModeStateChanged;
	}

	/// <summary>
	/// Called when the editor app play state has changed
	/// </summary>
	/// <param name="obj">Playmode state object</param>
	private void EditorApplication_playModeStateChanged(PlayModeStateChange obj)
	{
		if (obj == PlayModeStateChange.ExitingEditMode && _isSaveOnPlayEnabled)
		{
			EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
		}
	}
}
