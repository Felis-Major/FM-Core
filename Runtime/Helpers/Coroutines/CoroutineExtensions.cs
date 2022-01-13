using System.Collections;
using UnityEngine;

namespace Daniell.Runtime.Helpers.Coroutines
{
    /// <summary>
    /// Extension Methods for Coroutines
    /// </summary>
    public static class CoroutineExtensions
    {
        /// <summary>
        ///  Starts a coroutine and automatically set its reference
        /// </summary>
        /// <param name="monoBehaviour">Monobehaviour on which to run the coroutine</param>
        /// <param name="thread">Coroutine ref</param>
        /// <param name="coroutine">Actual Coroutine</param>
        public static void StartCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine thread, IEnumerator coroutine)
        {
            if (thread != null)
                monoBehaviour.StopCoroutine(thread);
            thread = monoBehaviour.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Stops a coroutine
        /// </summary>
        /// <param name="monoBehaviour">Monobehaviour to stop the coroutine with</param>
        /// <param name="thread">Coroutine ref</param>
        public static void StopCoroutine(this MonoBehaviour monoBehaviour, ref Coroutine thread)
        {
            if (thread != null)
            {
                monoBehaviour.StopCoroutine(thread);
                thread = null;
            }
        }
    }
}
