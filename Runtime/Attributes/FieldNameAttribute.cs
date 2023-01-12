using UnityEngine;

namespace FM.Runtime.Attributes
{
	/// <summary>
	/// Attribute used to change the name of a field
	/// </summary>
	public class FieldNameAttribute : PropertyAttribute
	{
		/* ==========================
		 * > Properties
		 * -------------------------- */

		/// <summary>
		/// Name of the field
		/// </summary>
		public string Name { get; }


		/* ==========================
		 * > Constructor
		 * -------------------------- */

		/// <summary>
		/// Default constructor for <see cref="FieldNameAttribute"/>
		/// </summary>
		/// <param name="name">Name of the attribute</param>
		public FieldNameAttribute(string name)
		{
			Name = name;
		}
	}
}