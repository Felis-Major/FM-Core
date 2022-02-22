using Daniell.Runtime.Helpers.General;
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
    [DefaultExecutionOrder(ExecutionOrders.SAVE_SYSTEM)]
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
         * > Private Serialized Fields
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
        private Dictionary<int, SaveDataContainer> _saveDataContainers = new Dictionary<int, SaveDataContainer>();


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
                _saveDataContainers.Add(i, new SaveDataContainer(i));
            }

            GameDataHandler.Register(this);
        }

        private void OnDestroy()
        {
            GameDataHandler.Unregister(this);
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
                saveable.Save(_saveDataContainers[i]);
            }

            return _saveDataContainers.Values.ToArray();
        }

        /// <summary>
        /// Load all Saveable objects from save data containers
        /// </summary>
        public void Load(SaveDataContainer[] saveDataContainers)
        {
            for (int i = 0; i < saveDataContainers.Length; i++)
            {
                SaveDataContainer container = saveDataContainers[i];
                _saveDataContainers[container.Order] = container;
            }

            for (int i = 0; i < _saveables.Count; i++)
            {
                ISaveable saveable = _saveables[i];
                saveable.Load(_saveDataContainers[i]);
            }
        }

        #endregion
    }
}