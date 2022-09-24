using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

namespace FM.Runtime.Helpers.DataStructures
{
	[Serializable]
	public class NewSDitct<TKey, TValue> : ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> _serializedKeys = new List<TKey>();

		[SerializeField]
		private List<TValue> _serializedValues = new List<TValue>();

		[SerializeField]
		private int _dataCount;

		private Dictionary<TKey, TValue> _data = new Dictionary<TKey, TValue>();

		public void Add(TKey key, TValue value)
		{
			_data.Add(key, value);
		}

		public void OnBeforeSerialize()
		{
			_serializedKeys.Clear();
			_serializedValues.Clear();

			// Create lists from the data
			foreach (KeyValuePair<TKey, TValue> item in _data)
			{
				_serializedKeys.Add(item.Key);
				_serializedValues.Add(item.Value);
			}

			_dataCount = _data.Count;
		}

		public void OnAfterDeserialize()
		{
			_data.Clear();

			if (_dataCount != _serializedKeys.Count || _dataCount != _serializedValues.Count)
			{
				throw new IndexOutOfRangeException("Key and value counts don't match.");
			}

			for (var i = 0; i < _dataCount; ++i)
			{
				_data.Add(_serializedKeys[i], _serializedValues[i]);
			}

			_serializedKeys.Clear();
			_serializedValues.Clear();
		}
	}
}
