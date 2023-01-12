using System;
using System.Collections;
using UnityEngine;

namespace FM.Runtime.Core.Coroutines
{
    /// <summary>
    /// Templates for coroutine
    /// </summary>
    public static class CoroutineTemplates
    {
        /// <summary>
        /// Cycle and execute an action for a set duration
        /// </summary>
        /// <param name="action">Action to be executed. float parameter is progress, from 0 to 1</param>
        /// <param name="duration">Duration of the cycle</param>
        /// <returns><see cref="IEnumerator"/> to be used as a <see cref="Coroutine"/></returns>
        public static IEnumerator DoCycleForDuration(Action<float> action, float duration)
        {
            for (float elapsedTime = 0; elapsedTime < duration; elapsedTime += UnityEngine.Time.deltaTime)
            {
                float progress = elapsedTime / duration;
                action(progress);

                yield return null;
            }

            // Finalize action to ensure correct behavior
            action(1);
        }
    }
}
