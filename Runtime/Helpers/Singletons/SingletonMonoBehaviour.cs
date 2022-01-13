using UnityEngine;
using System;
using System.Collections.Generic;

namespace Daniell.Runtime.Helpers.Singletons
{
    /// <summary>
    /// Singleton implementation for a MonoBehaviour
    /// </summary>
    /// <typeparam name="T">Type of the child class</typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        /// <summary>
        /// Instance of this singleton
        /// </summary>
        public static T Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    Debug.LogWarning($"{typeof(T).Name} is not yet ready.");
                }
#endif
                return _instance;
            }
            set { _instance = value; }
        }

        /// <summary>
        /// Is this singleton ready?
        /// </summary>
        public static bool IsInstanceReady => _instance != null;

        // Private fields
        private static T _instance;
        private static List<Action<T>> _delayedInstanceCalls = new List<Action<T>>();

        /// <summary>
        /// Called when the instance is ready
        /// </summary>
        private static event Action OnInstanceReady;

        protected virtual void Awake()
        {
            // If the Instance is already set, destroy this instance
            if (_instance != null)
            {
                Destroy(this);
            }
            else
            {
                // Set the instance
                Instance = this as T;

#if UNITY_EDITOR
                Debug.Log($"Singleton instance created for {typeof(T).Name}");
#endif

                // Call Instance ready
                OnInstanceReady?.Invoke();

                // Execute delayed instance calls actions
                for (int i = 0; i < _delayedInstanceCalls.Count; i++)
                {
                    _delayedInstanceCalls[i]?.Invoke(_instance);
                }

                _delayedInstanceCalls.Clear();
            }
        }

        protected virtual void OnDestroy()
        {
            // Set the instance to null when this instance is destroyed
            Instance = null;
        }

        /// <summary>
        /// Delay an action to when the instance is ready
        /// </summary>
        /// <param name="action">Action to be delayed</param>
        protected static void DelayedInstanceCall(Action<T> action)
        {
            if (_instance == null)
            {
                _delayedInstanceCalls.Add(action);
            }
            else
            {
                action?.Invoke(_instance);
            }
        }
    }
}