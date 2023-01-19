using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/Unity/" + nameof(Vector2Event), fileName = "New " + nameof(Vector2Event))]
	public class Vector2Event : GenericScriptableEvent<Vector2> { }
}