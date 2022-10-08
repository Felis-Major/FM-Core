using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableDatabase<T> : ScriptableObject where T : DatabaseItem
{
	[SerializeField]
	private List<T> _items = new List<T>();

}
