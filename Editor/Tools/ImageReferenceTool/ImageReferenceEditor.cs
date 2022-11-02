using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Debug = UnityEngine.Debug;

[CustomEditor(typeof(ImageReference))]
public class ImageReferenceEditor : Editor
{
	private bool _isEditingNotes;

	public override VisualElement CreateInspectorGUI()
	{
		// Build path to the local asset
		var currentObject = MonoScript.FromScriptableObject(this);
		string path = AssetDatabase.GetAssetPath(currentObject);
		string currentObjectName = $"{currentObject.name}.cs";
		string newObjectName = "ImageReferenceEditorTemplate.uxml";
		path = path.Replace(currentObjectName, newObjectName);

		VisualTreeAsset editorTemplate = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);

		TemplateContainer visualTreeInstance = editorTemplate.Instantiate();

		// Create root element
		var root = new VisualElement();

		root.Add(visualTreeInstance);

		// Handle components
		HandleNotes(root);
		HandleLinks(root);

		return root;
	}

	/// <summary>
	/// Handle notes section
	/// </summary>
	/// <param name="root">Root of the inspector</param>
	private void HandleNotes(VisualElement root)
	{
		// Find the note property
		SerializedProperty noteProperty = serializedObject.FindProperty("_notes");

		// Setup & bind the note field
		TextField noteField = root.Q<TextField>("NotesTextField");
		noteField.SetValueWithoutNotify(noteProperty.stringValue);

		// Update serialized field on value changed
		noteField.RegisterValueChangedCallback(x =>
		{
			noteProperty.stringValue = x.newValue;
			serializedObject.ApplyModifiedProperties();
		});

		// Setup the edit button
		Button editButton = root.Q<Button>("EditButton");
		SetEditState(false);
		editButton.clicked += () => SetEditState(!_isEditingNotes);

		void SetEditState(bool isEditing)
		{
			_isEditingNotes = isEditing;
			noteField.SetEnabled(isEditing);
			editButton.text = isEditing ? "Done" : "Edit";
		}
	}

	/// <summary>
	/// Handles the link section
	/// </summary>
	/// <param name="root">Root of the inspector</param>
	private void HandleLinks(VisualElement root)
	{
		// Get the target component to start a coroutine
		var targetComponent = target as ImageReference;

		// Cache the List view
		ListView imageList = root.Q<ListView>("ImageList");

		// Draw default links property
		SerializedProperty serializedLinksProperty = serializedObject.FindProperty("_links");
		PropertyField linksProperty = root.Q<PropertyField>("LinksProperty");
		linksProperty.BindProperty(serializedLinksProperty);

		// Handle buttons
		Button redrawButton = root.Q<Button>("RedrawButton");
		redrawButton.clicked += () => UpdateLinks();

		Button openAllButton = root.Q<Button>("OpenLinksButton");
		openAllButton.clicked += () =>
		{
			for (int i = 0; i < serializedLinksProperty.arraySize; i++)
			{
				string link = serializedLinksProperty.GetArrayElementAtIndex(i).stringValue;
				Process.Start(link);
			}
		};

		// Update links once when building UI
		UpdateLinks();

		void UpdateLinks()
		{
			targetComponent.StopAllCoroutines();

			imageList.Clear();

			// Add all links
			var links = new List<string>();
			for (int i = 0; i < serializedLinksProperty.arraySize; i++)
			{
				string link = serializedLinksProperty.GetArrayElementAtIndex(i).stringValue;
				links.Add(link);
			}

			// Setup listview
			imageList.itemsSource = links;
			imageList.makeItem = () => new Image();
			imageList.bindItem = (e, i) =>
			{
				var image = e as Image;
				targetComponent.StartCoroutine(DownloadImage(links[i], image));
			};
		}

		IEnumerator DownloadImage(string link, Image imageToUpdate)
		{
			UnityWebRequest www = UnityWebRequestTexture.GetTexture(link);
			yield return www.SendWebRequest();

			if (www.result != UnityWebRequest.Result.Success)
			{
				Debug.Log(www.error);
			}
			else
			{
				Texture tex = ((DownloadHandlerTexture)www.downloadHandler).texture;
				imageToUpdate.image = tex;
			}
		}
	}
}
