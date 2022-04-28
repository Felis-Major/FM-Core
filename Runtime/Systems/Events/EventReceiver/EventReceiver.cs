using FM.Runtime.Helpers.General;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// Base class for receiving an event
    /// </summary>
    [DefaultExecutionOrder(ExecutionOrders.EVENT_SYSTEM)]
    public abstract class EventReceiver : MonoBehaviour
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// Event linked to this receiver
        /// </summary>
        public abstract ScriptableEvent Event { get; }
    }
}