using System.Collections.Generic;
using UnityEngine;

public static class Singletons
{
	public static Dictionary<string, Object> _references = new();

	public static bool Get<T>(out T reference) where T : UnityEngine.Object
	{
		var name = nameof(T);
		var wasReferenceFound = _references.TryGetValue(name, out var instance);
		reference = (T)instance;
		return wasReferenceFound;
	}

	public static void Set<T>(T instance) where T : UnityEngine.Object
	{
		var name = nameof(T);
		_references[name] = instance;
	}

	public static void Remove<T>()
	{
		var name = nameof(T);
		if (_references.ContainsKey(name))
		{
			_references.Remove(name);
		}
	}
}
