using Daniell.Runtime.Helpers.GUIDSystem;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Daniell.Runtime.Systems.Save
{
    /// <summary>
    /// Handles saving and loading various targets in a GameObject
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-5000)]
    public class DataSaver : MonoBehaviour
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// Unique ID of this data saver
        /// </summary>
        public string GUID => _GUID;

        /// <summary>
        /// Is this data saver used accross multiple scenes?
        /// </summary>
        public bool IsGlobal => _isGlobal;

        /// <summary>
        /// SceneID of this Data Saver
        /// </summary>
        public int SceneID => gameObject.scene.buildIndex;


        /* ==========================
         * > Serialized Fields
         * -------------------------- */

        [SerializeField]
        [GUID]
        private string _GUID;

        [SerializeField]
        [Tooltip("Is this data saver used accross multiple scenes?")]
        private bool _isGlobal;


        /* ==========================
         * > Private Fields
         * -------------------------- */

        private List<ISaveable> _saveables = new List<ISaveable>();
        private Dictionary<string, SaveDataContainer> _saveDataContainers = new Dictionary<string, SaveDataContainer>();


        /* ==========================
         * > Methods
         * -------------------------- */

        #region Unity Messages

        private void Awake()
        {
            var saveTargets = GetComponents<ISaveable>();

            // Add all valid targets to the saveable dictionary
            for (int i = 0; i < saveTargets.Length; i++)
            {
                ISaveable target = saveTargets[i];
                _saveables.Add(target);
                _saveDataContainers.Add(target.GUID, new SaveDataContainer(target.GUID));
            }

            SaveManager.Register(this);
        }

        private void OnDestroy()
        {
            SaveManager.Unregister(this);
        }

        #endregion

        #region Save & Load

        /// <summary>
        /// Save all Saveable objects to their containers
        /// </summary>
        public SaveDataContainer[] Save()
        {
            for (int i = 0; i < _saveables.Count; i++)
            {
                ISaveable saveable = _saveables[i];
                saveable.Save(_saveDataContainers[saveable.GUID]);
            }

            return _saveDataContainers.Values.ToArray();
        }

        /// <summary>
        /// Load all Saveable objects from their containers
        /// </summary>
        public void Load(SaveDataContainer[] saveDataContainers)
        {
            for (int i = 0; i < saveDataContainers.Length; i++)
            {
                SaveDataContainer container = saveDataContainers[i];
                _saveDataContainers[container.TargetGUID] = container;
            }

            for (int i = 0; i < _saveables.Count; i++)
            {
                ISaveable saveable = _saveables[i];
                saveable.Load(_saveDataContainers[saveable.GUID]);
            }
        }

        #endregion
    }
}