using UnityEngine;

namespace FM.Runtime.References
{
	/// <summary>
	/// Single reference to an object at runtime
	/// </summary>
	[CreateAssetMenu(menuName = "Felis Major/References/Runtime Reference Single")]
	public class RuntimeReferenceSingle : RuntimeReference
	{
		/* ==========================
         * > Properties
         * -------------------------- */

		/// <summary>
		/// Is this reference ready to be used?
		/// </summary>
		public bool IsReady => _target != null;

		/// <summary>
		/// Reference target
		/// </summary>
		public GameObject Target => _target;


		/* ==========================
         * > Private Fields
         * -------------------------- */

		private GameObject _target = null;


		/* ==========================
         * > Methods
         * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Add(GameObject target)
		{
			base.Add(target);
			_target = target;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Remove(GameObject target)
		{
			base.Remove(target);
			_target = null;
		}

		/// <summary>
		/// Get the reference as T
		/// </summary>
		/// <typeparam name="T">Component Type</typeparam>
		/// <returns>Reference as T</returns>
		public T Get<T>() where T : Object
		{
			var component = _target.GetComponent<T>();
			if (component != null)
			{
				return component;
			}

			return null;
		}
	}
}