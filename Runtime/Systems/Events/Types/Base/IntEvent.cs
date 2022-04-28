using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [CreateAssetMenu(fileName = "New Int Event", menuName = MENU_PATH_BASE + "C#/Int")]
    public class IntEvent : GenericScriptableEvent<int> { }
}