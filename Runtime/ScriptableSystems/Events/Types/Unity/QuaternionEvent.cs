using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	[CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/Unity/" + nameof(QuaternionEvent), fileName = "New " + nameof(QuaternionEvent))]
	public class QuaternionEvent : GenericScriptableEvent<Quaternion> { }
}