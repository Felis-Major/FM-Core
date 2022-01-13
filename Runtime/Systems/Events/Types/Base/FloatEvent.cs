using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    [CreateAssetMenu(fileName = "New Float Event", menuName = MENU_PATH_BASE + "Float")]
    public class FloatEvent : GenericScriptableEvent<float> { }
}
