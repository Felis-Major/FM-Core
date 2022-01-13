using UnityEngine;
using UnityEngine.Events;

namespace Daniell.Runtime.Systems.Events
{
    public class VoidEventReceiver : EventReceiver
    {
        public override ScriptableEvent Event => _event;

        [SerializeField]
        [Tooltip("Event linked to this receiver")]
        private VoidEvent _event;

        [Space]
        [SerializeField]
        [Tooltip("Response called when the linked event is raised")]
        private UnityEvent _response;

        private void OnEnable()
        {
            _event.AddListener(OnEventReceived);
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