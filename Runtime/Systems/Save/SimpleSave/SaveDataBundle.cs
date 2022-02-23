using Daniell.Runtime.Helpers.DataStructures;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Systems.SimpleSave
{
    /// <summary>
    /// Handles reading and writing a list of Save Data
    /// </summary>
    [Serializable]
    public class SaveDataBundle
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// GUID of this object
        /// </summary>
        public string GUID => _guid;


        /* ==========================
         * > Private Fields
         * -------------------------- */

        [SerializeField]
        private string _guid;

        [SerializeField]
        private List<SaveData> _saveDataBundle = new List<SaveData>();

        /* ==========================
         * > Constructors
         * -------------------------- */

        public SaveDataBundle() : this(Guid.NewGuid().ToString()) { }

        public SaveDataBundle(string guid)
        {
            _guid = guid;
        }


        /* ==========================
         * > Methods
         * -------------------------- */

        #region Update List

        /// <summary>
        /// Set an new entry to the bundle
        /// </summary>
        /// <typeparam name="T">Original type of the entry</typeparam>
        /// <param name="key">Key of the entry</param>
        /// <param name="data">Data of the entry</param>
        public void Set<T>(string key, T data)
        {
            // Remove key if it exists
            if (TryGetIndexFromKey(key, out int index))
            {
                _saveDataBundle.RemoveAt(index);
            }

            // Save new data
            var json = JsonUtility.ToJson(new ValueWrapper<T>(data));
            var saveData = new SaveData(key, json);
            _saveDataBundle.Add(saveData);
        }

        /// <summary>
        /// Get an entry in the bundle
        /// </summary>
        /// <typeparam name="T">Original type of the entry</typeparam>
        /// <param name="key">Key of the entry</param>
        /// <returns>Entry as T</returns>
        public T Get<T>(string key)
        {
            if (TryGetIndexFromKey(key, out int index))
            {
                var jsonData = _saveDataBundle[index].JsonData;
                return JsonUtility.FromJson<ValueWrapper<T>>(jsonData).value;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Clear the stored bundle data
        /// </summary>
        public void Clear()
        {
            _saveDataBundle.Clear();
        }

        #endregion

        #region Helpers

        /// <summary>
        /// Does the save data bundle contain a given key?
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>True if there is a matching key in the data bundle</returns>
        public bool ContainsKey(string key)
        {
            for (int i = 0; i < _saveDataBundle.Count; i++)
            {
                SaveData saveData = _saveDataBundle[i];
                if (saveData.Key == key)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Outputs the index of a given key
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <param name="index">Index value</param>
        /// <returns>True if the key was found</returns>
        private bool TryGetIndexFromKey(string key, out int index)
        {
            for (int i = 0; i < _saveDataBundle.Count; i++)
            {
                SaveData saveData = _saveDataBundle[i];
                if (saveData.Key == key)
                {
                    index = i;
                    return true;
                }
            }

            index = -1;
            return false;
        }

        #endregion
    }
}