using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/Unity/" + nameof(GameObjectEvent), fileName = "New " + nameof(GameObjectEvent))]
	public class GameObjectEvent : GenericScriptableEvent<GameObject> { }
}