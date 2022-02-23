using System;
using UnityEngine;

namespace Daniell.Runtime.Systems.SimpleSave
{
    /// <summary>
    /// JSON data associated with a string key
    /// </summary>
    [Serializable]
    public struct SaveData
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// Key used to find this bundle
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// Data as JSON
        /// </summary>
        public string JsonData => _jsonData;


        /* ==========================
         * > Private Fields
         * -------------------------- */

        [SerializeField]
        private string _key;

        [SerializeField]
        private string _jsonData;


        /* ==========================
         * > Constructors
         * -------------------------- */

        public SaveData(string key, string jsonData)
        {
            _key = key;
            _jsonData = jsonData;
        }
    }
}