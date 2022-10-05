namespace FM.Runtime.Systems.StateMachine
{
	/// <summary>
	/// Handles updating and transitionning between states
	/// </summary>
	/// <typeparam name="T">Type of state</typeparam>
	public class StateController<T> where T : State
	{
		/* ==========================
         * > Private Fields
         * -------------------------- */

		/// <summary>
		/// Current state for this controller
		/// </summary>
		public T CurrentState { get; private set; }


		/* ==========================
         * > Methods
         * -------------------------- */

		#region State Logic

		/// <summary>
		/// Set a new state as the current state in the controller
		/// </summary>
		/// <param name="newState">State to be set as current state</param>
		public void SetState(State newState)
		{
			// Do not execute if both states are null
			if (CurrentState == null || newState == null)
			{
				return;
			}

			// Unsubscribe from the old state
			CurrentState.OnNextStateReady -= SetState;

			// End current state if it exists
			CurrentState.OnEndState();

			if (newState != null)
			{
				// Subscribe to the new state
				newState.OnNextStateReady += SetState;

				// Begin the new state and set as current state
				newState.OnBeginState();
				CurrentState = (T)newState;
			}
		}

		#endregion
	}
}