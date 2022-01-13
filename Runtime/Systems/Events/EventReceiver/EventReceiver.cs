using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    /// <summary>
    /// Base class for receiving an event
    /// </summary>
    [DefaultExecutionOrder(-10000)]
    public abstract class EventReceiver : MonoBehaviour
    {
#if UNITY_EDITOR
        public abstract ScriptableEvent Event { get; }
#endif
    }
}