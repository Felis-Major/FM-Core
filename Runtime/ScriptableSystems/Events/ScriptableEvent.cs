﻿using System;
using System.Threading.Tasks;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// Event that can be saved and referenced
    /// </summary>
    public abstract class ScriptableEvent : ScriptableObject
    {
        /// <summary>
        /// Base menu path
        /// </summary>
        public const string MENU_PATH_BASE = "Felis Major/Events/";

        /// <summary>
        /// Is this event active?
        /// </summary>
        public bool IsActive { get; protected set; }

        /// <summary>
        /// Internal Event
        /// </summary>
        private event Action OnEventRaised;

#if UNITY_EDITOR

        /// <summary>
        /// Description of the event
        /// </summary>
        public string Description => _description;

        [SerializeField]
        [Tooltip("Description of the event")]
        private string _description;

#endif

        /// <summary>
        /// Raise a given event with a null check
        /// </summary>
        /// <param name="scriptableEvent">Scriptable event to be raised</param>
        public static void RaiseEvent(ScriptableEvent scriptableEvent)
        {
            if (scriptableEvent != null)
            {
                scriptableEvent.Raise();
            }
        }

        /// <summary>
        /// Raise a given event with a null check
        /// </summary>
        /// <param name="scriptableEvent">Scriptable event to be raised</param>
        public static void RaiseEvent<TEvent, TValue>(TEvent scriptableEvent, TValue value) where TEvent : GenericScriptableEvent<TValue>
        {
            if (scriptableEvent != null)
            {
                scriptableEvent.Raise(value);
            }
        }

        /// <summary>
        /// Raise void event
        /// </summary>
#if UNITY_EDITOR
        [ContextMenu("Raise Event")]
#endif
        public virtual void Raise()
        {
            OnEventRaised?.Invoke();

            // Set event to active and sustain
            IsActive = true;
            SustainEvent();
        }

        /// <summary>
        /// Add a new listener to the event
        /// </summary>
        /// <param name="action">Delegate to subscribe</param>
        public void AddListener(Action action)
        {
            OnEventRaised += action;
        }

        /// <summary>
        /// Remove a listener from the event
        /// </summary>
        /// <param name="action">Delegate to unsubscribe</param>
        public void RemoveListener(Action action)
        {
            OnEventRaised -= action;
        }

        /// <summary>
        /// Maintain the event active until the whole frame is done
        /// </summary>
        protected async void SustainEvent()
        {
            await Task.Yield();
            OnEventExpired();
        }

        /// <summary>
        /// Called when the event has expired
        /// </summary>
        protected virtual void OnEventExpired()
        {
            IsActive = false;
        }
    }
}
