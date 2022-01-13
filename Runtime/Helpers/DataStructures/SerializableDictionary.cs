using System;
using System.Collections.Generic;

namespace Daniell.Runtime.Helpers.DataStructures
{
    /// <summary>
    /// A Serializable version of a Dictionary. First get will take more time, so it's best to update manually before accessing.
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : List<SerializableKeyValuePair<TKey, TValue>>
    {
        public TValue this[TKey key]
        {
            get
            {
                if (_internalDictionary == null)
                {
                    UpdateInternalDictionary();
                }
                return _internalDictionary[key];
            }
        }

        // Private fields
        private Dictionary<TKey, TValue> _internalDictionary;

        /// <summary>
        /// Update the internal dictionary manually.
        /// </summary>
        public void UpdateInternalDictionary()
        {
            _internalDictionary = new Dictionary<TKey, TValue>();

            for (int i = 0; i < Count; i++)
            {
                SerializableKeyValuePair<TKey, TValue> keyValuePair = this[i];
                _internalDictionary.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public static implicit operator SerializableDictionary<TKey, TValue>(Dictionary<TKey, TValue> d)
        {
            var dictionary = new SerializableDictionary<TKey, TValue>();
            foreach (var kvp in d)
            {
                dictionary.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
            }
            return dictionary;
        }

        public static implicit operator Dictionary<TKey, TValue>(SerializableDictionary<TKey, TValue> d)
        {
            var dictionary = new Dictionary<TKey, TValue>();
            foreach (var kvp in d)
            {
                dictionary.Add(kvp.Key, kvp.Value);
            }
            return dictionary;
        }
    }
}
