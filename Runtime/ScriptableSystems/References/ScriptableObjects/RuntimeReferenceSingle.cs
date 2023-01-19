using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.References
{
	/// <summary>
	/// Single reference to an object at runtime
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.ReferenceSystemPath + "/" + nameof(RuntimeReferenceSingle), fileName = "New " + nameof(RuntimeReferenceSingle))]
	public class RuntimeReferenceSingle : RuntimeReference
	{
		/* ==========================
         * > Properties
         * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override bool IsLoaded => Target != null;

		/// <summary>
		/// Reference target
		/// </summary>
		public GameObject Target { get; private set; } = null;


		/* ==========================
         * > Methods
         * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Add(GameObject target)
		{
			base.Add(target);
			Target = target;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Remove(GameObject target)
		{
			base.Remove(target);
			Target = null;
		}

		/// <summary>
		/// Get the reference as T
		/// </summary>
		/// <typeparam name="T">Component Type</typeparam>
		/// <returns>Reference as T</returns>
		public T Get<T>()
		{
			return Target != null && Target.TryGetComponent(out T component) ? component : default;
		}
	}
}