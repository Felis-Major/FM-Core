using UnityEngine;

namespace FM.Runtime.References
{
    /// <summary>
    /// Add / Remove reference capabilities
    /// </summary>
    public abstract class RuntimeReference : ScriptableObject
    {
        /// <summary>
        /// Add a reference
        /// </summary>
        /// <param name="target">Reference to add</param>
        public abstract void Add(GameObject target);

        /// <summary>
        /// Remove a reference
        /// </summary>
        /// <param name="target">Reference to remove</param>
        public abstract void Remove(GameObject target);
    }
}