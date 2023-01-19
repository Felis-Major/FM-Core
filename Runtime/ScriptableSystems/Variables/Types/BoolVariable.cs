using FM.Runtime.Core;
using FM.Runtime.References;
using UnityEngine;

namespace FM.Runtime.Systems.Variables
{
	/// <summary>
	/// Represents a float variable
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.VariableSystemPath + "/" + nameof(BoolVariable), fileName = "New " + nameof(BoolVariable))]
	public class BoolVariable : ScriptableVariable<bool> { }
}
