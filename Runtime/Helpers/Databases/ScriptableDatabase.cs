using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableDatabase<T> : ScriptableObject where T : DatabaseItem
{
	public T this[string guid]
	{
		get
		{
			// If both databases don't have the same content
			if (_guidItems.Count != _items.Length)
			{
				BuildItemList();
			}

			return _guidItems[guid];
		}
	}

	[SerializeField]
	private T[] _items;

	private Dictionary<string, T> _guidItems = new Dictionary<string, T>();

	private void OnEnable()
	{
		BuildItemList();
	}

	private void OnDisable()
	{
		_guidItems.Clear();
	}

	private void BuildItemList()
	{
		if (_items == null)
		{
			return;
		}

		_guidItems = new Dictionary<string, T>();

		for (var i = 0; i < _items.Length; i++)
		{
			T item = _items[i];
			_guidItems.Add(item.GUID, item);
		}
	}
}
