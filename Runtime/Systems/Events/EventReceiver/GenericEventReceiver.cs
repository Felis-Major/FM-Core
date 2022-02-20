using UnityEngine;
using UnityEngine.Events;

namespace Daniell.Runtime.Systems.Events
{
    /// <summary>
    /// Handles creating dynamic unity event types and dispatching an Event
    /// </summary>
    /// <typeparam name="TValue">Type of the event argument</typeparam>
    /// <typeparam name="TEvent">Type of the event</typeparam>
    public class GenericEventReceiver<TValue, TEvent> : EventReceiver where TEvent : GenericScriptableEvent<TValue>
    {
        /* ==========================
         * > Data Structures
         * -------------------------- */

        [System.Serializable]
        public class GenericUnityEvent : UnityEvent<TValue> { }


        /* ==========================
         * > Properties
         * -------------------------- */

        public override ScriptableEvent Event => _event;


        /* ==========================
         * > Private Serialized Fields
         * -------------------------- */

        [SerializeField]
        [Tooltip("Event linked to this receiver")]
        private TEvent _event;

        [SerializeField]
        [Tooltip("Execute this event only if the received value is in this list")]
        private TValue[] _filteredValues;

        [Space]
        [SerializeField]
        [Tooltip("Response called when the linked event is raised")]
        private GenericUnityEvent _response;


        /* ==========================
         * > Methods
         * -------------------------- */

        protected virtual void OnEnable()
        {
            _event.AddListener(OnEventReceived);

            // Check if the event is still active
            if (_event.IsActive)
            {
                OnEventReceived(_event.LastValue);
            }
        }

        protected virtual void OnDisable()
        {
            _event.RemoveListener(OnEventReceived);
        }

        protected virtual void OnEventReceived(TValue value)
        {
            // If there are values to filter
            if (_filteredValues.Length > 0)
            {
                // Find a matching value in the filter list
                for (int i = 0; i < _filteredValues.Length; i++)
                {
                    if (_filteredValues[i].Equals(value))
                    {
                        _response.Invoke(value);
                    }
                }
            }
            else
            {
                _response.Invoke(value);
            }
        }
    }
}