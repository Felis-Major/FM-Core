using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Helpers.General
{
    /// <summary>
    /// Various consistent execution orders
    /// </summary>
    public static class ExecutionOrders
    {
        /// <summary>
        /// Event System default execution order
        /// </summary>
        public const int EVENT_SYSTEM = -10000;

        /// <summary>
        /// Reference System default execution order
        /// </summary>
        public const int REFERENCE_SYSTEM = -10000;

        /// <summary>
        /// Save System default execution order
        /// </summary>
        public const int SAVE_SYSTEM = -5000;

        /// <summary>
        /// Execution order after all Systems are loaded
        /// </summary>
        public const int LOADED = -4999;
    }
}