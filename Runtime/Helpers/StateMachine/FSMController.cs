using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Helpers.StateMachine
{
    /// <summary>
    /// A simple State Pattern implementation
    /// </summary>
    public abstract class FSMController<T> : MonoBehaviour, IEnumerable<T> where T : FSMState
    {
        /// <summary>
        /// The default state for this FSMController
        /// </summary>
        public abstract string DefaultState { get; }

        /// <summary>
        /// The currently updated state
        /// </summary>
        public virtual T CurrentState
        {
            get => _currentState;
            private set
            {
                // Exit last state
                _currentState?.Exit();

                // Update current state
                _currentState = value;

                // Enter current state
                _currentState?.Enter();
            }
        }

        private T _currentState;
        private Dictionary<string, T> _states = new Dictionary<string, T>();

        #region Manage States

        /// <summary>
        /// Add a new state to the State machine
        /// </summary>
        /// <param name="stateName">Name of the state to be added</param>
        /// <param name="state">State to be added</param>
        public virtual void AddState(string stateName, T state)
        {
            _states.Add(stateName, state);
        }

        /// <summary>
        /// Remove a state from the State machine
        /// </summary>
        /// <param name="stateName">Name of the state to be removed</param>
        public virtual void RemoveState(string stateName) => _states.Remove(stateName);

        /// <summary>
        /// Switch to a new state
        /// </summary>
        /// <param name="newState">Name of the state to switch to</param>
        public virtual void SetCurrentState(string newState) => CurrentState = _states[newState];

        /// <summary>
        /// Set the current state to the default state
        /// </summary>
        public virtual void SetDefaultState() => SetCurrentState(DefaultState);

        #endregion

        #region Update States

        protected virtual void Update() => _currentState?.Update();

        protected virtual void FixedUpdate() => _currentState?.FixedUpdate();



        #endregion

        #region Enumerator Implementation

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return _states.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _states.Values.GetEnumerator();
        }

        #endregion
    }
}