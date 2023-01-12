using UnityEngine;
using UnityEngine.Events;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class VoidEventReceiver : EventReceiver
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override ScriptableEvent Event => _event;


        /* ==========================
         * > Private Serialized Fields
         * -------------------------- */

        [SerializeField]
        [Tooltip("Event linked to this receiver")]
        private VoidEvent _event;

        [Space]
        [SerializeField]
        [Tooltip("Response called when the linked event is raised")]
        private UnityEvent _response;


        /* ==========================
         * > Methods
         * -------------------------- */

        private void OnEnable()
        {
            _event.AddListener(OnEventReceived);

            // Check if the event is still active
            if (_event.IsActive)
            {
                OnEventReceived();
            }
        }

        private void OnDisable()
        {
            _event.RemoveListener(OnEventReceived);
        }

        private void OnEventReceived()
        {
            _response.Invoke();
        }
    }
}