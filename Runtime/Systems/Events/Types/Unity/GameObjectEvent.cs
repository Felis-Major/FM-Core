using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    [CreateAssetMenu(fileName = "New GameObject Event", menuName = MENU_PATH_BASE + "Unity/GameObject")]
    public class GameObjectEvent : GenericScriptableEvent<GameObject> { }
}