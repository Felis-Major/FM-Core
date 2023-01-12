using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    [CreateAssetMenu(fileName = "New Float Event", menuName = MENU_PATH_BASE + "C#/Float")]
    public class FloatEvent : GenericScriptableEvent<float> { }
}
