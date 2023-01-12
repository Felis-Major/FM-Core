using System;
using UnityEngine;

namespace FM.Runtime.Attributes
{
	/// <summary>
	/// Attribute used to mark a field as a scene reference
	/// </summary>
	[AttributeUsage(AttributeTargets.Field)]
	public class SceneReferenceAttribute : PropertyAttribute { }
}