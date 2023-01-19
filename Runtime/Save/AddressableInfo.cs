using System;
using UnityEngine;

namespace FM.Runtime.Core.DataManagement
{
	/// <summary>
	/// Serializable addressable item info to be loaded at runtime
	/// </summary>
	[Serializable]
	public class AddressableInfo
	{
		/* ==========================
		 * > Properties
		 * -------------------------- */

		/// <summary>
		/// Tag of the asset
		/// </summary>
		public string Tag => _tag;

		/// <summary>
		/// Name of the asset
		/// </summary>
		public string Name => _name;


		/* ==========================
		 * > Private Serialized Fields
		 * -------------------------- */

		[SerializeField]
		[Tooltip("Tag of the asset")]
		private string _tag;

		[SerializeField]
		[Tooltip("Name of the asset")]
		private string _name;


		/* ==========================
		 * > Methods
		 * -------------------------- */

		public AddressableInfo(string tag, string name)
		{
			_tag = tag;
			_name = name;
		}
	}
}
