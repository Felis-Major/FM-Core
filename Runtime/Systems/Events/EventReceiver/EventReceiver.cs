using UnityEngine;

namespace Daniell.Runtime.Systems.Events
{
    /// <summary>
    /// Base class for receiving an event
    /// </summary>
    [DefaultExecutionOrder(ExecutionOrders.EVENT_SYSTEM)]
    public abstract class EventReceiver : MonoBehaviour
    {
        public abstract ScriptableEvent Event { get; }
    }
}