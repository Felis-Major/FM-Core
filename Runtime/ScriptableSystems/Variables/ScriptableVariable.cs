using System;
using UnityEngine;

namespace FM.Runtime.Systems.Variables
{
	public abstract class ScriptableVariable : ScriptableObject
	{
 		private static List<ScriptableVariable> _instances = new();
	
 		private void OnEnable()
   		{
     			_instances.Add(this);
   		}

		private void OnDisable()
  		{
			_instances.Remove(this);
		}
 	}

	/// <summary>
	/// Holds data in a scriptable object
	/// </summary>
	/// <typeparam name="T">Type of data to be held</typeparam>
	public abstract class ScriptableVariable<T> : ScriptableVariable
	{
		/// <summary>
		/// Value of the variable
		/// </summary>
		public T Value
		{
			get => _isOverrideValueEnabled ? _overrideValue : _value;
			set => _value = value;
		}

		[NonSerialized]
		[Tooltip("Value of the variable")]
		private T _value;

		[SerializeField]
		[Tooltip("Should the override value be used?")]
		private bool _isOverrideValueEnabled;

		[SerializeField]
		[Tooltip("Fixed value to be used instead")]
		private T _overrideValue;

		[SerializeField]
		private T _defaultValue;

		private void Awake()
		{
			_value = _defaultValue;
			// hideFlags = HideFlags.HideAndDontSave;
		}
	}
}
