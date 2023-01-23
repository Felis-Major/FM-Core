using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/C#/" + nameof(IntArrayEvent), fileName = "New " + nameof(IntArrayEvent))]
	public class IntArrayEvent : GenericScriptableEvent<int[]> { }
}