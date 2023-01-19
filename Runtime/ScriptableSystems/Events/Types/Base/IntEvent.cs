using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/C#/" + nameof(IntEvent), fileName = "New " + nameof(IntEvent))]
	public class IntEvent : GenericScriptableEvent<int> { }
}