using System;

namespace Daniell.Runtime.Helpers.Coroutines
{
    /// <summary>
    /// Represents an action that can be executed over a period of time
    /// </summary>
    public class Cycle
    {
        /// <summary>
        /// Length of the cycle
        /// </summary>
        public float Length { get; private set; }

        /// <summary>
        /// Current Completion of the cycle
        /// </summary>
        public float Completion { get; private set; }

        /// <summary>
        /// Action of the cycle
        /// </summary>
        public Action<float> Action { get; set; }

        /// <summary>
        /// Completion rate of the cycle
        /// </summary>
        public Func<float> CompletionRate { get; set; }

        public Cycle(float cycleTime, Action<float> cycleAction)
        {
            Length = cycleTime;
            Action = cycleAction;
            CompletionRate = () => 1;
            Completion = 0;
        }

        public Cycle(float cycleTime, Func<float> cycleCompletionRate)
        {
            Length = cycleTime;
            Action = null;
            CompletionRate = cycleCompletionRate;
            Completion = 0;
        }

        public Cycle(float cycleTime, Action<float> cycleAction, Func<float> cycleCompletionRate)
        {
            Length = cycleTime;
            Action = cycleAction;
            CompletionRate = cycleCompletionRate;
            Completion = 0;
        }

        /// <summary>
        /// Restart the Cycle
        /// </summary>
        public void Reset() => Completion = 0;

        /// <summary>
        /// Go to the next iteration
        /// </summary>
        /// <returns>Has the cycle completed?</returns>
        public bool Next()
        {
            Completion += CompletionRate();
            Completion = Math.Min(Completion, Length);
            Action?.Invoke(Completion / Length);
            return Completion < Length;
        }
    }
}
