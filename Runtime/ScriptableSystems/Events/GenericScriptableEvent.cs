﻿using System;
using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// Generic event that can be saved and referenced
    /// </summary>
    /// <typeparam name="T">Event value type</typeparam>
    public abstract class GenericScriptableEvent<T> : ScriptableEvent
    {
        /// <summary>
        /// Last value of the event
        /// </summary>
        public T LastValue { get; protected set; }

        /// <summary>
        /// Internal event
        /// </summary>
        private event Action<T> OnEventRaised;

        /// <summary>
        /// Raise event with value of type T
        /// </summary>
        /// <param name="value">Value of the event</param>
        public virtual void Raise(T value)
        {
            OnEventRaised?.Invoke(value);

            // Set event to active and sustain
            IsActive = true;
            LastValue = value;
            SustainEvent();
        }

        /// <summary>
        /// Add a new listener to the event
        /// </summary>
        /// <param name="action">Action</param>
        public void AddListener(Action<T> action)
        {
            OnEventRaised += action;
        }

        /// <summary>
        /// Remove a listener from the event
        /// </summary>
        /// <param name="action">Delegate to unsubscribe</param>
        public void RemoveListener(Action<T> action)
        {
            OnEventRaised -= action;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnEventExpired()
        {
            base.OnEventExpired();

            // Reset value
            LastValue = default;
        }
    }
}
