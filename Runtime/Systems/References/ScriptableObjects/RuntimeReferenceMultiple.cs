using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

		private List<GameObject> _targets = new List<GameObject>();


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
			}
		}

		/// <summary>
		/// Get the content of this group as T
		/// </summary>
		/// <typeparam name="T">Component Type</typeparam>
		/// <returns>List of T</returns>
		public T[] Get<T>() where T : Object
		{
			List<T> components = new List<T>();

			for (int i = 0; i < _targets.Count; i++)
			{
				var component = _targets[i].GetComponent<T>();
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