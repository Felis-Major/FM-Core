using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Helpers.StateMachine
{
    /// <summary>
    /// State that can be added to an FSMController
    /// </summary>
    public abstract class FSMState
    {
        public event Action<string> OnStateFinished;

        /// <summary>
        /// End the current state and go to the next one
        /// </summary>
        /// <param name="nextState">Next state</param>
        protected void Finish(string nextState)
        {
            OnStateFinished?.Invoke(nextState);
        }

        /// <summary>
        /// When entering the state
        /// </summary>
        public virtual void Enter() { }

        /// <summary>
        /// When exiting the state
        /// </summary>
        public virtual void Exit() { }

        /// <summary>
        /// On Every Unity Update tick
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// On Every Unity Fixed Update tick
        /// </summary>
        public virtual void FixedUpdate() { }
    }
}
