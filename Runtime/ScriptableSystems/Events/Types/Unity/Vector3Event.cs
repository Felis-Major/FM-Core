using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/Unity/" + nameof(Vector3Event), fileName = "New " + nameof(Vector3Event))]
	public class Vector3Event : GenericScriptableEvent<Vector3> { }
}