using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Variables
{
	/// <summary>
	/// Represents a float variable
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.VariableSystemPath + "/" + nameof(StringVariable), fileName = "New " + nameof(StringVariable))]
	public class StringVariable : ScriptableVariable<string> { }
}
