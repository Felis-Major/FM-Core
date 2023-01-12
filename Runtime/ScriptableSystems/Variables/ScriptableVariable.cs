using UnityEngine;

namespace FM.Runtime.Systems.Variables
{
	public abstract class ScriptableVariable : ScriptableObject
	{

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
#if UNITY_EDITOR
		public T Value => _isOverrideValueEnabled ? _overrideValue : _value;
#else
	public T Value => _value;
#endif

		[SerializeField]
		[Tooltip("Value of the variable")]
		private T _value;

#if UNITY_EDITOR

		[SerializeField]
		[Tooltip("Should the override value be used?")]
		private bool _isOverrideValueEnabled;

		[SerializeField]
		[Tooltip("Fixed value to be used instead")]
		private T _overrideValue;

#endif
	}
}
