using UnityEngine;

namespace FM.Runtime.Systems.Events
{
    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public class BoolEventReceiver : GenericEventReceiver<bool, BoolEvent>
    {
        /* ==========================
         * > Private Serialized fields
         * -------------------------- */

        [SerializeField]
        [Tooltip("Should the value be inverted")]
        private bool _shouldInvertValue;


        /* ==========================
         * > Methods
         * -------------------------- */

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        protected override void OnEventReceived(bool value)
        {
            // Process incoming value
            bool newValue = _shouldInvertValue ? !value : value;

            // Execute the base method with the new value
            base.OnEventReceived(newValue);
        }
    }
}