namespace Daniell.Runtime.Helpers.General
{
    /// <summary>
    /// Various consistent execution orders
    /// </summary>
    public static class ExecutionOrders
    {
        #region Event System

        /// <summary>
        /// Before Event System default execution order
        /// </summary>
        public const int BEFORE_EVENT_SYSTEM = EVENT_SYSTEM - 1;

        /// <summary>
        /// Event System default execution order
        /// </summary>
        public const int EVENT_SYSTEM = -10000;

        /// <summary>
        /// After Event System default execution order
        /// </summary>
        public const int AFTER_EVENT_SYSTEM = EVENT_SYSTEM + 1;

        #endregion

        #region Reference System

        /// <summary>
        /// Before Reference System default execution order
        /// </summary>
        public const int BEFORE_REFERENCE_SYSTEM = REFERENCE_SYSTEM - 1;

        /// <summary>
        /// Reference System default execution order
        /// </summary>
        public const int REFERENCE_SYSTEM = -9000;

        /// <summary>
        /// After Reference System default execution order
        /// </summary>
        public const int AFTER_REFERENCE_SYSTEM = REFERENCE_SYSTEM + 1;


        #endregion

        #region Save System 

        /// <summary>
        /// Before Save System default execution order
        /// </summary>
        public const int BEFORE_SAVE_SYSTEM = SAVE_SYSTEM - 1;

        /// <summary>
        /// Save System default execution order
        /// </summary>
        public const int SAVE_SYSTEM = -8000;

        /// <summary>
        /// After Save System default execution order
        /// </summary>
        public const int AFTER_SAVE_SYSTEM = SAVE_SYSTEM + 1;

        #endregion

        /// <summary>
        /// Execution order after all Systems are loaded
        /// </summary>
        public const int LOADED = -4000;
    }
}