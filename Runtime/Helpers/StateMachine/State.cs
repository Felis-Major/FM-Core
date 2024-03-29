﻿using System;

namespace FM.Runtime.Systems.StateMachine
{
	/// <summary>
	/// Represents a state that can be used by a controller
	/// </summary>
	public class State
	{
		/* ==========================
		 * > Properties
		 * -------------------------- */

		/// <summary>
		/// The <see cref="StateController{T}"/> controlling this state. This property is guaranteed to not be null when this is the current state
		/// </summary>
		public StateController StateController { get; set; }


		/* ==========================
         * > Events
         * -------------------------- */

		/// <summary>
		/// Called when the next state is ready
		/// </summary>
		public event Action<State> OnNextStateReady;


		/* ==========================
         * > Methods
         * -------------------------- */

		/// <summary>
		/// Begin the state
		/// </summary>
		public virtual void OnBeginState() { }

		/// <summary>
		/// End the state
		/// </summary>
		public virtual void OnEndState() { }

		/// <summary>
		/// End the current state
		/// </summary>
		/// <param name="nextState">State to transition to</param>
		protected virtual void Finish(State nextState)
		{
			OnNextStateReady?.Invoke(nextState);
		}
	}
}