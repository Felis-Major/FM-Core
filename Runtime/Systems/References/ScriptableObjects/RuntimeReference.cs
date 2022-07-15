using System;
using UnityEngine;

namespace FM.Runtime.References
{
	/// <summary>
	/// Add / Remove reference capabilities
	/// </summary>
	public abstract class RuntimeReference : ScriptableObject
	{
		public event Action OnReferenceLoaded;
		public event Action OnReferenceUnloaded;

		/// <summary>
		/// Add a reference
		/// </summary>
		/// <param name="target">Reference to add</param>
		public virtual void Add(GameObject target)
		{
			OnReferenceLoaded?.Invoke();
		}

		/// <summary>
		/// Remove a reference
		/// </summary>
		/// <param name="target">Reference to remove</param>
		public virtual void Remove(GameObject target)
		{
			OnReferenceUnloaded?.Invoke();
		}
	}
}