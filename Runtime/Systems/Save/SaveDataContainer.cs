using Daniell.Runtime.Helpers.DataStructures;
using Daniell.Runtime.Helpers.Encoding;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Systems.Save
{
    /// <summary>
    /// Handles converting and retrieving json data
    /// </summary>
    [System.Serializable]
    public class SaveDataContainer
    {
        /* ==========================
         * > Data Structures
         * -------------------------- */

        /// <summary>
        /// Serialized id and json string for a specific value
        /// </summary>
        [System.Serializable]
        private struct SaveData
        {
            public string key;
            public string data;

            public SaveData(string key, string data)
            {
                this.key = key;
                this.data = data;
            }
        }

        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// GUID of the target of this data container
        /// </summary>
        public string TargetGUID => _targetGUID;


        /* ==========================
         * > Serialized Fields
         * -------------------------- */

        [SerializeField]
        private string _targetGUID;

        [SerializeField]
        private List<SaveData> _data = new List<SaveData>();


        /* ==========================
         * > Methods
         * -------------------------- */

        public SaveDataContainer() : this("undefined") { }

        public SaveDataContainer(string targetGUID)
        {
            _targetGUID = targetGUID;
        }

        /// <summary>
        /// Set a value using a unique ID
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">ID</param>
        /// <param name="data">Value</param>
        public void Set<T>(string key, T data)
        {
            // Remove data if it already exists
            if (Contains(key, out int index))
            {
                _data.RemoveAt(index);
            }

            // Convert the data to a json string
            var dataAsJson = JsonUtility.ToJson(new ValueWrapper<T>(data));

            // Encode data
            var dataEncoder = new SymetricEncoder(_targetGUID);
            var encodedData = dataEncoder.EncodeString(dataAsJson);

            // Store it in a new container
            var saveData = new SaveData(key, encodedData);

            // Add the data to the serialized list of data
            _data.Add(saveData);
        }

        /// <summary>
        /// Get a stored value using its unique ID
        /// </summary>
        /// <typeparam name="T">Type of the value</typeparam>
        /// <param name="key">ID</param>
        /// <returns>Value of the data</returns>
        public T Get<T>(string key)
        {
            if (Contains(key, out int index))
            {
                var dataEncoder = new SymetricEncoder(_targetGUID);
                var json = dataEncoder.DecodeString(_data[index].data);
                ValueWrapper<T> data = JsonUtility.FromJson<ValueWrapper<T>>(json);
                return data.value;
            }

            Debug.LogWarning($"{key} was not present in the saved data");

            return default;
        }

        /// <summary>
        /// Does the currently saved data contains a given key?
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <returns>True if the key was found</returns>
        public bool Contains(string key) => Contains(key, out int _);

        /// <summary>
        /// Does the currently saved data contains a given key?
        /// </summary>
        /// <param name="key">Key to look for</param>
        /// <param name="index">index of the found key</param>
        /// <returns>True if the key was found</returns>
        public bool Contains(string key, out int index)
        {
            index = -1;

            for (int i = 0; i < _data.Count; i++)
            {
                if (_data[i].key == key)
                {
                    index = i;
                    return true;
                }
            }

            return false;
        }
    }
}