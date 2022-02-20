using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.References
{
    /// <summary>
    /// Adds the GameObject this component is placed on to a given Runtime Group
    /// </summary>
    [DefaultExecutionOrder(ExecutionOrders.REFERENCE_SYSTEM)]
    public class RuntimeReferenceSetter : MonoBehaviour
    {
        /* ==========================
         * > Private Serialized Fields
         * -------------------------- */

        [SerializeField]
        [Tooltip("Runtime Group to add this Game Object to")]
        private RuntimeReference _runtimeReference;


        /* ==========================
         * > Methods
         * -------------------------- */

        private void Awake()
        {
            _runtimeReference.Add(gameObject);
        }

        private void OnDestroy()
        {
            _runtimeReference.Remove(gameObject);
        }
    }
}