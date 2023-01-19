using System.Text.RegularExpressions;
using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/Unity/" + nameof(Vector4Event), fileName = "New " + nameof(Vector4Event))]
	public class Vector4Event : GenericScriptableEvent<Vector4> { }
}