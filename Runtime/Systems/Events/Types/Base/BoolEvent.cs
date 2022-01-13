using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    [CreateAssetMenu(fileName = "New Bool Event", menuName = MENU_PATH_BASE + "Bool")]
    public class BoolEvent : GenericScriptableEvent<bool> { }
}