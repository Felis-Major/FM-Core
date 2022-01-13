using System;
using System.Collections;
using UnityEngine;

namespace Daniell.Runtime.Helpers.Coroutines
{
    /// <summary>
    /// Templates for coroutine
    /// </summary>
    public static class CoroutineTemplates
    {
        /// <summary>
        /// Execute a System.Action when a condition is met
        /// </summary>
        /// <param name="coroutine">Ref to the coroutine</param>
        /// <param name="caller">Ref to the monobehaviour calling this coroutine</param>
        /// <param name="condition">Condition that when true will execute the Action</param>
        /// <param name="action">Action to execute</param>
        public static void ExecuteWhen(ref Coroutine coroutine, MonoBehaviour caller, Func<bool> condition, Action action)
            => caller.StartCoroutine(ref coroutine, ExecuteWhenCoroutine(condition, action));

        private static IEnumerator ExecuteWhenCoroutine(Func<bool> condition, Action action)
        {
            yield return new WaitUntil(condition);
            action();
        }

        /// <summary>
        /// Execute a System.Action while a condition is met
        /// </summary>
        /// <param name="coroutine">Ref to the coroutine</param>
        /// <param name="caller">Ref to the monobehaviour calling this coroutine</param>
        /// <param name="condition">Condition that while true will execute the Action</param>
        /// <param name="action">Action to execute</param>
        /// <param name="yieldInstruction">Yield instruction (null == every frame)</param>
        /// <param name="endAction">Action to execute at the end</param>
        public static void ExecuteWhile(ref Coroutine coroutine, MonoBehaviour caller, Func<bool> condition, Action action, YieldInstruction yieldInstruction, Action endAction = null)
            => caller.StartCoroutine(ref coroutine, ExecuteWhileCoroutine(condition, action, yieldInstruction, endAction));

        private static IEnumerator ExecuteWhileCoroutine(Func<bool> condition, Action action, YieldInstruction yieldInstruction, Action endAction = null)
        {
            while (condition())
            {
                action();
                yield return yieldInstruction;
            }

            endAction?.Invoke();
        }

        /// <summary>
        /// Execute a System.Action after x seconds
        /// </summary>
        /// <param name="coroutine">Ref to the coroutine</param>
        /// <param name="caller">Ref to the monobehaviour calling this coroutine</param>
        /// <param name="action">Action to execute</param>
        /// <param name="timeInSeconds">Time before executing the action</param>
        /// <param name="realtime">Should the time use unscaled time instead? (will not respond to timescale)</param>
        public static void ExecuteAfter(ref Coroutine coroutine, MonoBehaviour caller, Action action, float timeInSeconds, bool realtime = false)
            => caller.StartCoroutine(ref coroutine, ExecuteAfterCoroutine(action, timeInSeconds, realtime));

        private static IEnumerator ExecuteAfterCoroutine(Action action, float timeInSeconds, bool realtime = false)
        {
            if (realtime)
                yield return new WaitForSecondsRealtime(timeInSeconds);
            else
                yield return new WaitForSeconds(timeInSeconds);

            action.Invoke();
        }

        /// <summary>
        /// Execute a Cycle. Can be use for fading stuff, etc...
        /// </summary>
        /// <param name="coroutine">Ref to the coroutine</param>
        /// <param name="caller">Ref to the monobehaviour calling this coroutine</param>
        /// <param name="cycle">Cycle to be executed</param>
        /// <param name="endAction">Action to execute at the end</param>
        public static void ExecuteCycle(ref Coroutine coroutine, MonoBehaviour caller, Cycle cycle, Action endAction = null)
        => caller.StartCoroutine(ref coroutine, ExecuteCycleCoroutine(cycle, endAction));

        /// <summary>
        /// Execute a Cycle. Can be use for fading stuff, etc...
        /// </summary>
        /// <param name="coroutine">Ref to the coroutine</param>
        /// <param name="caller">Ref to the monobehaviour calling this coroutine</param>
        /// <param name="cycleTime">Time to finish the cycle</param>
        /// <param name="cycleAction">Action to be executed in the cycle</param>
        /// <param name="cycleCompletionRate">Completion rate of the cycle (usually Time.deltaTime)</param>
        /// <param name="endAction">Action to execute at the end</param>
        public static void ExecuteCycle(ref Coroutine coroutine, MonoBehaviour caller, float cycleTime, Action<float> cycleAction, Func<float> cycleCompletionRate, Action endAction = null)
        => caller.StartCoroutine(ref coroutine, ExecuteCycleCoroutine(new Cycle(cycleTime, cycleAction, cycleCompletionRate), endAction));

        private static IEnumerator ExecuteCycleCoroutine(Cycle cycle, Action endAction = null)
        {
            while (cycle.Next())
                yield return null;

            endAction?.Invoke();
        }
    }
}
