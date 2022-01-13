using System;
using System.Linq;
using UnityEngine;

namespace Daniell.Runtime.Helpers.Singletons
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        // Instanciated version of this ScriptableObject
        public static T Instance
        {
            get
            {
                var instances = Resources.FindObjectsOfTypeAll<SingletonScriptableObject<T>>();
                for (int i = 0; i < instances.Length; i++)
                {
                    if (instances[i].IsActiveInstance)
                        return instances[i] as T;
                }

                return null;
            }
        }

        /// <summary>
        /// Is this the currently active ScriptableObject?
        /// </summary>
        public bool IsActiveInstance => _isActiveInstance;

        [SerializeField]
        private bool _isActiveInstance;
    }
}
