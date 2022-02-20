using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [CreateAssetMenu(fileName = " New String Event", menuName = MENU_PATH_BASE + "C#/String")]
    public class StringEvent : GenericScriptableEvent<string> { }
}