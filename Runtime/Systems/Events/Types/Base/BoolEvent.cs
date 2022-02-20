using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [CreateAssetMenu(fileName = "New Bool Event", menuName = MENU_PATH_BASE + "C#/Bool")]
    public class BoolEvent : GenericScriptableEvent<bool> { }
}