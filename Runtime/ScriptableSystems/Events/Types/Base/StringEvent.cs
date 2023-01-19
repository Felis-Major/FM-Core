using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/C#/" + nameof(StringEvent), fileName = "New " + nameof(StringEvent))]
	public class StringEvent : GenericScriptableEvent<string> { }
}