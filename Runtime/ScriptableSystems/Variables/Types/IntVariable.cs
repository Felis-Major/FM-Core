using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Variables
{
	/// <summary>
	/// Represents a float variable
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.VariableSystemPath + "/" + nameof(IntVariable), fileName = "New " + nameof(IntVariable))]
	public class IntVariable : ScriptableVariable<int> { }
}
