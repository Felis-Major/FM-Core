using FM.Runtime.Core;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [CreateAssetMenu(menuName = PackageConstants.BasePath + "/" + PackageConstants.EventSystemPath + "/C#/" + nameof(BoolEvent), fileName = "New " + nameof(BoolEvent))]
    public class BoolEvent : GenericScriptableEvent<bool> { }
}