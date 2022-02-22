using Daniell.Runtime.Helpers.Files;
using Daniell.Runtime.Helpers.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Daniell.Runtime.Systems.Save
{
    /// <summary>
    /// Handles saving and loading game data
    /// </summary>
    public static class GameDataHandler
    {
        /* ==========================
         * > Data Structures
         * -------------------------- */

        /// <summary>
        /// Mode of operation of the Save Manager
        /// </summary>
        public enum DataPersistency
        {
            /// <summary>
            /// Stores the data in a temporary folder
            /// </summary>
            Temporary,

            /// <summary>
            /// Stores the data in a persistent folder
            /// </summary>
            Permanent
        }

        /// <summary>
        /// Processes information about the active data
        /// </summary>
        private class ActiveDataInfo
        {
            /* ==========================
             * > Private Fields
             * -------------------------- */

            private readonly List<int> _activeSceneIDs = new List<int>();
            private readonly List<DataSaver> _globalDataSavers = new List<DataSaver>();
            private readonly Dictionary<int, List<DataSaver>> _dataSaversByID = new Dictionary<int, List<DataSaver>>();


            /* ==========================
             * > Constructors
             * -------------------------- */

            public ActiveDataInfo(List<DataSaver> registeredDataSavers)
            {
                for (int i = 0; i < registeredDataSavers.Count; i++)
                {
                    var registeredDataSaver = registeredDataSavers[i];
                    var sceneID = registeredDataSaver.SceneID;

                    // Add unique Scene ID
                    if (!_activeSceneIDs.Contains(sceneID))
                    {
                        _activeSceneIDs.Add(sceneID);
                    }

                    // Sort by global or scene ID
                    if (registeredDataSaver.IsGlobal)
                    {
                        _globalDataSavers.Add(registeredDataSaver);
                    }
                    else
                    {
                        if (_dataSaversByID.ContainsKey(sceneID))
                        {
                            _dataSaversByID[sceneID].Add(registeredDataSaver);
                        }
                        else
                        {
                            _dataSaversByID.Add(sceneID, new List<DataSaver>() { registeredDataSaver });
                        }
                    }
                }
            }


            /* ==========================
             * > Methods
             * -------------------------- */

            /// <summary>
            /// Get all the active Scene IDs
            /// </summary>
            /// <returns>Scene IDs</returns>
            public List<int> GetActiveSceneIDs()
            {
                return _activeSceneIDs;
            }

            /// <summary>
            /// Get all the Data Savers associated with a scene ID
            /// </summary>
            /// <param name="sceneID">Scene ID to look for</param>
            /// <returns>List of DataSavers</returns>
            public List<DataSaver> GetDataSaversForSceneID(int sceneID)
            {
                return _dataSaversByID[sceneID];
            }

            /// <summary>
            /// Get all the Data Savers tagged as global
            /// </summary>
            /// <returns>Global Data Savers</returns>
            public List<DataSaver> GetGlobalDataSavers()
            {
                return _globalDataSavers;
            }
        }

        /// <summary>
        /// Holds information about a data saver
        /// </summary>
        [Serializable]
        private struct DataSaverInfo
        {
            /* ==========================
             * > Properties
             * -------------------------- */

            /// <summary>
            /// GUID of the Data Saver
            /// </summary>
            public string GUID { get => _guid; set => _guid = value; }

            /// <summary>
            /// Data Saver's save data containers
            /// </summary>
            public SaveDataContainer[] SaveDataContainers { get => _saveDataContainers; set => _saveDataContainers = value; }


            /* ==========================
             * > Private Serialized Fields
             * -------------------------- */

            [SerializeField]
            private string _guid;

            [SerializeField]
            private SaveDataContainer[] _saveDataContainers;
        }


        /* ==========================
         * > Constants
         * -------------------------- */

        /// <summary>
        /// Name given to the default slot
        /// </summary>
        public const string DEFAULT_SLOT_NAME = "Default_User";

        /// <summary>
        /// File extension for temporary data
        /// </summary>
        public const string TEMPORARY_FILE_EXTENSION = "temp";

        /// <summary>
        /// File extension for permanent data
        /// </summary>
        public const string PERMANENT_FILE_EXTENSION = "dat";


        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// Currently Active Slot (will return the default slot if null or empty string)
        /// </summary>
        public static string ActiveSlot
        {
            get => _activeSlot == string.Empty || _activeSlot == null ? DEFAULT_SLOT_NAME : _activeSlot;
            set => _activeSlot = value;
        }

        /// <summary>
        /// Base path to persistent data
        /// </summary>
        public static string BaseSaveDataPath => Application.persistentDataPath;

        /// <summary>
        /// Directory where the game files should be stored
        /// </summary>
        public static string WorkingDirectory => Path.Combine(BaseSaveDataPath, ActiveSlot);

        /// <summary>
        /// Type of file that will be saved
        /// </summary>
        public static FileHandler.FileType FileHandlerType { get; set; } = FileHandler.FileType.Json;


        /* ==========================
         * > Private Fields
         * -------------------------- */

        private static string _activeSlot;
        private static List<DataSaver> _registeredDataSavers = new List<DataSaver>();
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();


        /* ==========================
         * > Methods
         * -------------------------- */

        #region Register / Unregister

        /// <summary>
        /// Register a DataSaver
        /// </summary>
        /// <param name="dataSaver">DataSaver to register</param>
        public static void Register(DataSaver dataSaver)
        {
            _registeredDataSavers.Add(dataSaver);
        }

        /// <summary>
        /// Unregister a DataSaver
        /// </summary>
        /// <param name="dataSaver">DataSaver to register</param>
        public static void Unregister(DataSaver dataSaver)
        {
            _registeredDataSavers.Remove(dataSaver);
        }

        #endregion

        #region Save / Load

        /// <summary>
        /// Save all loaded game data
        /// </summary>
        /// <param name="dataPersistency">Persistency of the data</param>
        public async static void Save(DataPersistency dataPersistency)
        {
            // Do not execute if there is no data saver to save from
            if (_registeredDataSavers.Count == 0)
            {
                return;
            }

            ConsoleLogger.Log("Saving Game...", Color.magenta, ConsoleLogger.LogType.Important);

            var activeDataInfo = new ActiveDataInfo(_registeredDataSavers);
            var activeSceneIDs = activeDataInfo.GetActiveSceneIDs();

            // Load unregistered data from Scene ID files
            for (int i = 0; i < activeSceneIDs.Count; i++)
            {
                var sceneID = activeSceneIDs[i];
                // Find registered data for this scene ID and convert to containers
                var registeredData = _registeredDataSavers
                    .Where(x => x.SceneID == sceneID)
                    .ToList()
                    .ConvertAll(x => new DataSaverInfo()
                    {
                        GUID = x.GUID,
                        SaveDataContainers = x.Save()
                    });

                // Add registered data savers for this ID to the data to be saved
                var dataToSave = new List<DataSaverInfo>();
                dataToSave.AddRange(registeredData);

                var filePath = Path.Combine(WorkingDirectory, GetFullFileName(sceneID.ToString(), dataPersistency));
                var fileHandler = new FileHandler(filePath, FileHandlerType);

                // If there is already data, replace only what has changed
                if (File.Exists(filePath))
                {
                    // Read file at SceneID path
                    var loadedDataSavers = await fileHandler.Read<List<DataSaverInfo>>();
                    var loadedGUIDs = dataToSave.ConvertAll(x => x.GUID);

                    // Add all data that doesn't have a loaded GUID
                    for (int j = 0; j < loadedDataSavers.Count; j++)
                    {
                        var loadedDataSaver = loadedDataSavers[i];

                        if (!loadedGUIDs.Contains(loadedDataSaver.GUID))
                        {
                            dataToSave.Add(loadedDataSaver);
                        }
                    }
                }

                // Write the modified data to the file
                await fileHandler.Write(dataToSave);

                ConsoleLogger.Log($"Saved Scene {sceneID} \n File Path: {filePath}", Color.green, ConsoleLogger.LogType.Important);
            }
        }

        /// <summary>
        /// Load all saved game data
        /// </summary>
        /// <param name="dataPersistency">Persistency of the data</param>
        public async static void Load(DataPersistency dataPersistency)
        {
            // Do not execute if there is no data saver to load to
            if (_registeredDataSavers.Count == 0)
            {
                return;
            }

            ConsoleLogger.Log("Loading Game...", Color.magenta, ConsoleLogger.LogType.Important);

            var activeDataInfo = new ActiveDataInfo(_registeredDataSavers);
            var activeSceneIDs = activeDataInfo.GetActiveSceneIDs();

            // Load data from Scene ID files that matches loaded GUIDs
            for (int i = 0; i < activeSceneIDs.Count; i++)
            {
                var sceneID = activeSceneIDs[i];

                // Find registered data for this scene ID
                var registeredDataSavers = _registeredDataSavers.Where(x => x.SceneID == sceneID).ToList();

                // Read file at SceneID path
                var filePath = Path.Combine(WorkingDirectory, GetFullFileName(sceneID.ToString(), dataPersistency));
                var fileHandler = new FileHandler(filePath, FileHandlerType);

                // If a save file exists, load data
                if (File.Exists(filePath))
                {
                    var loadedDataSavers = await fileHandler.Read<List<DataSaverInfo>>();

                    // Load data for each Data Saver
                    for (int j = 0; j < registeredDataSavers.Count; j++)
                    {
                        var registeredDataSaver = registeredDataSavers[j];
                        for (int k = 0; k < loadedDataSavers.Count; k++)
                        {
                            var loadedData = loadedDataSavers[k];

                            // If the same GUID was found, load the Data Saver
                            if (loadedData.GUID == registeredDataSaver.GUID)
                            {
                                registeredDataSaver.Load(loadedData.SaveDataContainers);
                            }
                        }
                    }

                    ConsoleLogger.Log($"Scene {sceneID} loaded", Color.green, ConsoleLogger.LogType.Important);
                }
                else
                {
                    ConsoleLogger.Log($"No data found for Scene {sceneID}", Color.yellow, ConsoleLogger.LogType.Important);
                }
            }
        }

        /// <summary>
        /// Save Temp data to Permanent save file
        /// </summary>
        public async static void MakeTemporaryDataPermanent()
        {
            var tempFiles = await GetSaveFiles(DataPersistency.Temporary);

            // For each temp file
            for (int i = 0; i < tempFiles.Count; i++)
            {
                var tempFileName = tempFiles[i];
                var permanentFileName = tempFileName.Replace($".{TEMPORARY_FILE_EXTENSION}", $".{PERMANENT_FILE_EXTENSION}");

                if (File.Exists(permanentFileName))
                {
                    var tempFileHandler = new FileHandler(tempFileName, FileHandlerType);
                    var tempFileData = await tempFileHandler.Read<List<DataSaverInfo>>();

                    var permanentFileHandler = new FileHandler(permanentFileName, FileHandlerType);
                    var permanentFileData = await permanentFileHandler.Read<List<DataSaverInfo>>();

                    // Remove matching data from permanent file
                    for (int j = 0; j < tempFileData.Count; j++)
                    {
                        var tempData = tempFileData[j];
                        permanentFileData.RemoveAll(x => x.GUID == tempData.GUID);
                        tempFileData.AddRange(permanentFileData);

                        await permanentFileHandler.Write(tempFileData);
                    }
                }
                else
                {
                    // Change the file extension if the file doesn't exist
                    await Task.Run(() => File.Move(tempFileName, permanentFileName));
                }
            }

            FlushTempData();
        }

        /// <summary>
        /// Remove all temp data from save folder
        /// </summary>
        public async static void FlushTempData()
        {
            var tempFiles = await GetSaveFiles(DataPersistency.Temporary);

            await Task.Run(() =>
            {
                // Remove all files from directory
                for (int i = 0; i < tempFiles.Count; i++)
                {
                    File.Delete(tempFiles[i]);
                }
            });
        }

        #endregion

        /// <summary>
        /// Get File name with extension depending on data persistency
        /// </summary>
        /// <param name="name">Name of the file</param>
        /// <param name="dataPersistency">Data persistency</param>
        /// <returns>Full file name with extension</returns>
        private static string GetFullFileName(string name, DataPersistency dataPersistency)
        {
            string extension = "file";

            switch (dataPersistency)
            {
                case DataPersistency.Temporary:
                    extension = TEMPORARY_FILE_EXTENSION;
                    break;
                case DataPersistency.Permanent:
                    extension = PERMANENT_FILE_EXTENSION;
                    break;
            }

            return $"{name}.{extension}";
        }

        /// <summary>
        /// Get save files for persistency type
        /// </summary>
        /// <param name="dataPersistency">Persistency of the data</param>
        /// <returns>List of files matching the data persistency</returns>
        private async static Task<List<string>> GetSaveFiles(DataPersistency dataPersistency)
        {
            // Load all temp data files
            var files = new List<string>();
            var targetExtension = dataPersistency == DataPersistency.Temporary ? TEMPORARY_FILE_EXTENSION : PERMANENT_FILE_EXTENSION;

            await Task.Run(() =>
            {
                foreach (var file in Directory.EnumerateFiles(WorkingDirectory))
                {
                    if (file.Contains($".{targetExtension}"))
                    {
                        files.Add(file);
                    }
                }
            });

            return files;
        }
    }
}