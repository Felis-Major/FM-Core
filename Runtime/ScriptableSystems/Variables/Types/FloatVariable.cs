using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Variables
{
	/// <summary>
	/// Represents a float variable
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.VariableSystemPath + "/" + nameof(FloatVariable), fileName = "New " + nameof(FloatVariable))]
	public class FloatVariable : ScriptableVariable<float> { }
}
