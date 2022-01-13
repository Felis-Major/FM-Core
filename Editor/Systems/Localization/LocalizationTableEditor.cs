using Daniell.Runtime.Helpers.WebServices;
using Daniell.Runtime.Systems.Localization;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.Localization
{
    [CustomEditor(typeof(LocalizationTable))]
    public class LocalizationTableEditor : UnityEditor.Editor
    {
        private string _searchKey = "";

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            var v = (LocalizationTable)target;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Update from Google Sheet"))
            {
                v.UpdateData();
            }

            if (GUILayout.Button("Rebuild Database"))
            {
                v.CompileFromRawData();
            }

            if (GUILayout.Button("Edit..."))
            {
                Application.OpenURL(GoogleSheetDownloadHandler.BuildURL(v.GoogleSheetID));
            }

            GUILayout.EndHorizontal();

            GUILayout.Space(20);

            GUILayout.BeginHorizontal("box");
            
            GUILayout.Label("Search Key");

            _searchKey = EditorGUILayout.TextField(_searchKey);

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();

            GUILayout.Label($"Keys ({v.Keys.Length})", GUILayout.Width(150));

            for (int i = 0; i < v.AvailableLanguages.Length; i++)
            {
                GUILayout.Label(v.AvailableLanguages[i], GUILayout.Width(150));
            }

            GUILayout.EndHorizontal();

            foreach (var t in v.Keys)
            {
                DrawLine(t, v[t]);
            }
        }

        private void DrawLine(string key, string[] data)
        {
            if (_searchKey != "" && !key.Contains(_searchKey))
            {
                return;
            }

            GUILayout.BeginHorizontal("box");

            GUILayout.Label(key, GUILayout.Width(150));

            for (int i = 0; i < data.Length; i++)
            {
                GUILayout.Label(data[i], GUILayout.Width(150));
            }

            GUILayout.EndHorizontal();
        }
    }
}