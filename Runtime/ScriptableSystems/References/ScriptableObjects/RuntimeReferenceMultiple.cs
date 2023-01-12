using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FM.Runtime.References
{
	/// <summary>
	/// Object updated at runtime with a list of linked objects
	/// </summary>
	[CreateAssetMenu(menuName = "Felis Major/References/Runtime Reference Multiple")]
	public class RuntimeReferenceMultiple : RuntimeReference, IEnumerable<GameObject>
	{
		/* ==========================
		 * > Properties
		 * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override bool IsLoaded => _targets != null && _targets.Count > 0;


		/* ==========================
         * > Private Fields
         * -------------------------- */

		private List<GameObject> _targets = new();


		/* ==========================
		 * > Events
		 * -------------------------- */

		/// <summary>
		/// Called when a new <see cref="GameObject"/> is added to this runtime reference 
		/// </summary>
		public event Action<GameObject> OnTargetAdded;

		/// <summary>
		/// Called when a new <see cref="GameObject"/> is removed to this runtime reference 
		/// </summary>
		public event Action<GameObject> OnTargetRemoved;

		/// <summary>
		/// Called when changes happened in the <see cref="RuntimeReferenceMultiple"/>
		/// </summary>
		public event Action OnRuntimeReferenceModified;


		/* ==========================
         * > Methods
         * -------------------------- */

		#region Runtime Reference

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Add(GameObject target)
		{
			base.Add(target);

			if (!_targets.Contains(target))
			{
				_targets.Add(target);
				OnTargetAdded?.Invoke(target);
				OnRuntimeReferenceModified?.Invoke();
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Remove(GameObject target)
		{
			base.Remove(target);

			if (_targets.Contains(target))
			{
				_targets.Remove(target);
				OnTargetRemoved?.Invoke(target);
				OnRuntimeReferenceModified?.Invoke();
			}
		}

		/// <summary>
		/// Get the content of this group as T
		/// </summary>
		/// <typeparam name="T">Component Type</typeparam>
		/// <returns>List of T</returns>
		public T[] Get<T>()
		{
			if (_targets.Count == 0)
			{
				return new T[0];
			}

			var components = new List<T>();

			for (int i = 0; i < _targets.Count; i++)
			{
				T component = _targets[i].GetComponent<T>();
				if (component != null)
				{
					components.Add(component);
				}
			}

			return components.ToArray();
		}

		#endregion

		#region IEnumerable 

		public IEnumerator<GameObject> GetEnumerator()
		{
			return ((IEnumerable<GameObject>)_targets).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)_targets).GetEnumerator();
		}

		#endregion
	}
}