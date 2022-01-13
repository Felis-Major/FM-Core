using System;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Helpers.DataStructures
{
    /// <summary>
    /// A Serializable version of a KeyValuePair.
    /// </summary>
    /// <typeparam name="TKey">Key type</typeparam>
    /// <typeparam name="TValue">Value type</typeparam>
    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        /// <summary>
        /// Key.
        /// </summary>
        public TKey Key => _key;

        /// <summary>
        /// Value.
        /// </summary>
        public TValue Value => _value;

        [SerializeField]
        [Tooltip("Key")]
        private TKey _key;

        [SerializeField]
        [Tooltip("Value")]
        private TValue _value;

        public SerializableKeyValuePair(TKey key, TValue value)
        {
            _key = key;
            _value = value;
        }

        public static implicit operator SerializableKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> keyValuePair)
        {
            return new SerializableKeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
        }

        public static implicit operator KeyValuePair<TKey, TValue>(SerializableKeyValuePair<TKey, TValue> keyValuePair)
        {
            return new KeyValuePair<TKey, TValue>(keyValuePair.Key, keyValuePair.Value);
        }
    }
}
