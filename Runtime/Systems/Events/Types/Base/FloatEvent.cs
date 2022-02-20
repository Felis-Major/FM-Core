using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [CreateAssetMenu(fileName = "New Float Event", menuName = MENU_PATH_BASE + "C#/Float")]
    public class FloatEvent : GenericScriptableEvent<float> { }
}
