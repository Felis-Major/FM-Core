using System.Threading.Tasks;
using UnityEngine;

namespace FM.Runtime.Core.Coroutines
{
    /// <summary>
    /// <see cref="CustomYieldInstruction"/> to wait for an awaitable <see cref="Task"/> to finish
    /// </summary>
    public class WaitForTask : CustomYieldInstruction
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool keepWaiting => !_task.IsCompleted;


        /* ==========================
         * > Private Fields
         * -------------------------- */

        private readonly Task _task;    // Task to wait for


        /* ==========================
         * > Constructor
         * -------------------------- */

        public WaitForTask(Task task)
        {
            _task = task;
        }
    }
}
