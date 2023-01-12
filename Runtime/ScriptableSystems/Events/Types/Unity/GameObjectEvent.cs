using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    [CreateAssetMenu(fileName = "New GameObject Event", menuName = MENU_PATH_BASE + "Unity/GameObject")]
    public class GameObjectEvent : GenericScriptableEvent<GameObject> { }
}