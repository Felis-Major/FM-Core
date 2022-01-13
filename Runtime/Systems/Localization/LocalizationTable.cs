using Daniell.Runtime.Helpers.DataStructures;
using Daniell.Runtime.Helpers.WebServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Daniell.Runtime.Systems.Localization
{
    /// <summary>
    /// List of Keys and translation in every supported languages.
    /// </summary>
    [CreateAssetMenu(fileName = "New Localization Table", menuName = "Daniell/Localization/Table")]
    public class LocalizationTable : ScriptableObject, ISerializationCallbackReceiver
    {
        /// <summary>
        /// Get data from Key and Language.
        /// </summary>
        /// <param name="key">Key of the data</param>
        /// <param name="lang">Language of the data</param>
        /// <returns>Translated data</returns>
        public string this[string key, string lang]
        {
            get
            {
                string value = "undefined";

                try
                {
                    value = _tableData[key][_langIndexes[lang]];
                }
                catch
                {
                    Debug.LogWarning("Key not found or invalid language. Check that the right Table is assigned.");
                }

                return value;
            }
        }

        /// <summary>
        /// Get data from Key.
        /// </summary>
        /// <param name="key">Key of the data</param>
        /// <returns>Translated datas</returns>
        public string[] this[string key]
        {
            get
            {
                string[] values = null;

                try
                {
                    values = _tableData[key];
                }
                catch
                {
                    Debug.LogWarning("Key Not found!");
                }

                return values;
            }
        }

        /// <summary>
        /// Get all the available keys in the table.
        /// </summary>
        public string[] Keys => _tableData.Keys.ToArray();

        /// <summary>
        /// ID of the linked google sheet document.
        /// </summary>
        public string GoogleSheetID => _googleSheetID;

        /// <summary>
        /// List of all the available languages.
        /// </summary>
        public string[] AvailableLanguages => _langIndexes.Keys.ToArray();

        /// <summary>
        /// RAW CSV data.
        /// </summary>
        public string RawData { get; private set; } = string.Empty;

        // Serialized Fields
        [SerializeField]
        [Tooltip("ID of the linked google sheet document.")]
        private string _googleSheetID = "";

        // Private fields
        private Dictionary<string, string[]> _tableData = new Dictionary<string, string[]>();
        private List<SerializableKeyValuePair<string, string[]>> _serializedTableData = new List<SerializableKeyValuePair<string, string[]>>();

        private Dictionary<string, int> _langIndexes = new Dictionary<string, int>();
        private List<SerializableKeyValuePair<string, int>> _serializedLangIndexes = new List<SerializableKeyValuePair<string, int>>();

        private GoogleSheetDownloadHandler _downloadHandler;

        /// <summary>
        /// Called when the table is recompiled.
        /// </summary>
        public event Action OnTableUpdated;

        #region Data Handling

        /// <summary>
        /// Update Data from Google Sheet file.
        /// </summary>
        public void UpdateData()
        {
            _downloadHandler = new GoogleSheetDownloadHandler(_googleSheetID);
            _downloadHandler.OnDocumentDownloaded += OnDataDownloaded;

            bool hasDownloadStarted = _downloadHandler.Download();

            // If the download hasn't started, stop execution
            if (!hasDownloadStarted)
            {
                _downloadHandler.OnDocumentDownloaded -= OnDataDownloaded;
                return;
            }

            void OnDataDownloaded(string data)
            {
                // Update Data
                RawData = data;

                // Rebuild database
                CompileFromRawData();

                // Unsubscribe from download callback
                _downloadHandler.OnDocumentDownloaded -= OnDataDownloaded;
            }
        }

        /// <summary>
        /// Rebuild internal dictionary from current Data.
        /// </summary>
        public void CompileFromRawData()
        {
            _tableData = new Dictionary<string, string[]>();
            _langIndexes = new Dictionary<string, int>();

            // Define regex csv pattern
            Regex csvParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");

            string[] lines = RawData.Split('\n');

            // Create an entry for each line
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                string[] data = csvParser.Split(line);

                if (data.Length == 0)
                {
                    continue;
                }

                // Find key
                string key = data[0];

                // Find values
                string[] values = new string[data.Length - 1];
                for (int j = 0; j < values.Length; j++)
                {
                    values[j] = data[j + 1];
                }

                // If we are on the first line, fill language indexes
                if (i == 0)
                {
                    for (int j = 0; j < values.Length; j++)
                    {
                        // Get value and remove whitespaces
                        string val = values[j].Trim();

                        // Remove control chars from CSV
                        val = Regex.Replace(val, @"[\u0000-\u001F]", string.Empty);

                        _langIndexes.Add(val, j);
                    }
                }
                // Else, fill data
                else
                {
                    _tableData.Add(key, values);
                }
            }

            OnTableUpdated?.Invoke();
        }

        #endregion

        #region Serialization

        public void OnBeforeSerialize()
        {
            // Unload the dictionaries into serializable data
            UnloadDictionary(ref _tableData, ref _serializedTableData);
            UnloadDictionary(ref _langIndexes, ref _serializedLangIndexes);
        }

        public void OnAfterDeserialize()
        {
            // Load the dictionaries from serialized data
            LoadDictionary(ref _tableData, ref _serializedTableData);
            LoadDictionary(ref _langIndexes, ref _serializedLangIndexes);
        }

        private void UnloadDictionary<TKey, TValue>(ref Dictionary<TKey, TValue> dict, ref List<SerializableKeyValuePair<TKey, TValue>> list)
        {
            list = new List<SerializableKeyValuePair<TKey, TValue>>();
            foreach (var data in dict)
            {
                list.Add(new SerializableKeyValuePair<TKey, TValue>(data.Key, data.Value));
            }
        }

        private void LoadDictionary<TKey, TValue>(ref Dictionary<TKey, TValue> dict, ref List<SerializableKeyValuePair<TKey, TValue>> list)
        {
            dict = new Dictionary<TKey, TValue>();
            for (int i = 0; i < list.Count; i++)
            {
                SerializableKeyValuePair<TKey, TValue> data = list[i];
                dict.Add(data.Key, data.Value);
            }
        }

        #endregion
    }
}