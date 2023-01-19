using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/C#/" + nameof(FloatEvent), fileName = "New " + nameof(FloatEvent))]
	public class FloatEvent : GenericScriptableEvent<float> { }
}
