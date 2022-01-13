using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    [CreateAssetMenu(fileName = "New Int Event", menuName = MENU_PATH_BASE + "Int")]
    public class IntEvent : GenericScriptableEvent<int> { }
}