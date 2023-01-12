using System;
using UnityEngine;

namespace FM.Runtime.References
{
	/// <summary>
	/// Add / Remove reference capabilities
	/// </summary>
	public abstract class RuntimeReference : ScriptableObject
	{
		/* ==========================
		 * > Properties
		 * -------------------------- */

		/// <summary>
		/// Is this reference loaded?
		/// </summary>
		public abstract bool IsLoaded { get; }


		/* ==========================
		 * > Events
		 * -------------------------- */

		/// <summary>
		/// Called when this reference is loaded
		/// </summary>
		public event Action OnReferenceLoaded;

		/// <summary>
		/// Called when this reference is unloaded
		/// </summary>
		public event Action OnReferenceUnloaded;


		/* ==========================
		 * > Methods
		 * -------------------------- */

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