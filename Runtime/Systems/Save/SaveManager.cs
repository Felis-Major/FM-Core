using Daniell.Runtime.Helpers.DataStructures;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Daniell.Runtime.Systems.Save
{
    /// <summary>
    /// Handles writing and reading save file
    /// </summary>
    public static class SaveManager
    {
        /* ==========================
         * > Data Structures
         * -------------------------- */

        /// <summary>
        /// Serializable version of DataSaver
        /// </summary>
        [System.Serializable]
        private struct DataSaverContent
        {
            public string guid;
            public SaveDataContainer[] saveDataContainers;

            public DataSaverContent(string guid, SaveDataContainer[] saveDataContainers)
            {
                this.guid = guid;
                this.saveDataContainers = saveDataContainers;
            }
        }


        /* ==========================
         * > Constants
         * -------------------------- */

        public const string SAVE_FOLDER_PATH = "SaveFiles";
        public const string SLOTS_FOLDER_PATH = "Slots";

        public const string DEFAULT_SLOT_NAME = "DEFAULT";

        public const string SETTINGS_FILE_NAME = "GameSettings.json";

        public const string SAVE_FILE_NAME_PREFIX = "SAVE_FILE_";
        public const string GLOBAL_SAVE_FILE_NAME = "GLOBAL";
        public const string SAVE_FILE_EXTENSION = "data";

        /* ==========================
         * > Properties
         * -------------------------- */

        public static string SelectedSlot { get; set; }


        /* ==========================
         * > Private Fields
         * -------------------------- */

        private static List<DataSaver> _registeredDataSavers = new List<DataSaver>();


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

        #region Path Helpers

        /// <summary>
        /// Get base path for persistent data folder
        /// </summary>
        /// <returns>Persistent Data folder path</returns>
        private static string GetPersistentDataFolderPath()
        {
            return Application.persistentDataPath;
        }

        /// <summary>
        /// Get save folder path
        /// </summary>
        /// <returns>Save folder path</returns>
        private static string GetSaveFolderPath()
        {
            return $"{GetPersistentDataFolderPath()}/{SAVE_FOLDER_PATH}";
        }

        /// <summary>
        /// Get slot folder path 
        /// </summary>
        /// <returns>Slot folder path</returns>
        private static string GetSlotFolderPath()
        {
            return $"{GetSaveFolderPath()}/{SLOTS_FOLDER_PATH}";
        }

        /// <summary>
        /// Does the slot exists on the disk?
        /// </summary>
        /// <param name="slotName">Name of the slot</param>
        /// <returns>True if the slot exists</returns>
        private static bool IsSlotDefined(string slotName)
        {
            string slotPath = $"{GetSlotFolderPath()}/{slotName}";
            return Directory.Exists(slotPath);
        }

        /// <summary>
        /// Create a slot on the disk
        /// </summary>
        /// <param name="slotName">Name of the slot</param>
        private static void CreateSlot(string slotName)
        {
            string slotPath = $"{GetSlotFolderPath()}/{slotName}";
            Directory.CreateDirectory(slotPath);
        }

        private static string GetSaveFileName(int sceneIndex)
        {
            return $"{SAVE_FILE_NAME_PREFIX}{sceneIndex}.{SAVE_FILE_EXTENSION}";
        }

        private static string GetGameSettingsFilePath()
        {
            return $"{GetSaveFolderPath()}/{SETTINGS_FILE_NAME}";
        }

        #endregion

        #region Read & Write Data to file

        private static void CreateSaveFile(string fileName, string slotName, DataSaverContent[] dataSaverContentList)
        {
            if (slotName == null || slotName == "")
            {
                slotName = DEFAULT_SLOT_NAME;
            }

            // Create the slot if not defined
            if (!IsSlotDefined(slotName))
            {
                CreateSlot(slotName);
            }

            // Find file path
            string saveFilePath = $"{GetSlotFolderPath()}/{slotName}/{fileName}";

            List<DataSaverContent> dataToSave = new List<DataSaverContent>();

            // If the file exists
            if (File.Exists(saveFilePath))
            {
                // Open the file
                var savedData = ReadSaveDataFromFile(saveFilePath);

                // Convert incoming data to dictionary for fast access
                Dictionary<string, DataSaverContent> dataSaverContentDictionary = new Dictionary<string, DataSaverContent>();
                for (int i = 0; i < dataSaverContentList.Length; i++)
                {
                    var dataSaverContent = dataSaverContentList[i];
                    dataSaverContentDictionary.Add(dataSaverContent.guid, dataSaverContent);
                }

                // Replace saved content by incoming new data
                for (int i = 0; i < savedData.Length; i++)
                {
                    var data = savedData[i];

                    if (dataSaverContentDictionary.ContainsKey(data.guid))
                    {
                        dataToSave.Add(dataSaverContentDictionary[data.guid]);
                    }
                    else
                    {
                        dataToSave.Add(data);
                    }
                }
            }
            else
            {
                dataToSave.AddRange(dataSaverContentList);
            }

            // Replace file
            WriteSaveDataToFile(saveFilePath, dataToSave.ToArray());
        }

        private static void WriteSaveDataToFile(string path, DataSaverContent[] data)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            var serializableData = new ValueWrapper<DataSaverContent[]>(data);

            // Serialize to binary data and save to file
            binaryFormatter.Serialize(stream, serializableData);

            // Close file stream
            stream.Close();
        }

        private static DataSaverContent[] OpenSaveFile(string fileName, string slotName)
        {
            if (slotName == null || slotName == "")
            {
                slotName = DEFAULT_SLOT_NAME;
            }

            // Find file path
            string saveFilePath = $"{GetSlotFolderPath()}/{slotName}/{fileName}";

            if (File.Exists(saveFilePath))
            {
                return ReadSaveDataFromFile(saveFilePath);
            }

            return new DataSaverContent[0];
        }

        private static DataSaverContent[] ReadSaveDataFromFile(string path)
        {
            if (File.Exists(path))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                var serializedData = (ValueWrapper<DataSaverContent[]>)binaryFormatter.Deserialize(stream);

                stream.Close();

                return serializedData.value;
            }

            return new DataSaverContent[0];
        }

        #endregion

        #region Save & Load Game Data

        /// <summary>
        /// Load all data from files
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Daniell/Save System/Load")]
#endif
        public static void Load()
        {
            // Get all scene IDs
            List<int> loadedSceneIDs = new List<int>();

            List<DataSaver> globalDataSavers = new List<DataSaver>();

            for (int i = 0; i < _registeredDataSavers.Count; i++)
            {
                var dataSaver = _registeredDataSavers[i];

                if (dataSaver.IsGlobal)
                {
                    globalDataSavers.Add(dataSaver);
                }
                else
                {
                    var sceneID = dataSaver.SceneID;
                    if (!loadedSceneIDs.Contains(sceneID))
                    {
                        loadedSceneIDs.Add(sceneID);
                    }
                }
            }

            // Load all ID files
            for (int i = 0; i < loadedSceneIDs.Count; i++)
            {
                var loadedSceneID = loadedSceneIDs[i];
                var saveFileName = GetSaveFileName(loadedSceneID);
                var gameData = OpenSaveFile(saveFileName, SelectedSlot);

                if (gameData.Length > 0)
                {
                    var dataAsDictionary = GetDataSaverDictionary(gameData);

                    for (int j = 0; j < _registeredDataSavers.Count; j++)
                    {
                        var dataSaver = _registeredDataSavers[j];

                        if (!dataSaver.IsGlobal && dataSaver.SceneID == loadedSceneID)
                        {
                            // Load data
                            dataSaver.Load(dataAsDictionary[dataSaver.GUID]);
                        }
                    }
                }
            }

            // Load global file
            if (globalDataSavers.Count > 0)
            {
                var gameData = OpenSaveFile(GLOBAL_SAVE_FILE_NAME, SelectedSlot);

                if (gameData.Length > 0)
                {
                    var dataAsDictionary = GetDataSaverDictionary(gameData);

                    for (int i = 0; i < globalDataSavers.Count; i++)
                    {
                        // Load data
                        var dataSaver = globalDataSavers[i];
                        dataSaver.Load(dataAsDictionary[dataSaver.GUID]);
                    }
                }
            }

            Dictionary<string, SaveDataContainer[]> GetDataSaverDictionary(DataSaverContent[] gameData)
            {
                Dictionary<string, SaveDataContainer[]> dataSaverContents = new Dictionary<string, SaveDataContainer[]>();

                for (int j = 0; j < gameData.Length; j++)
                {
                    DataSaverContent data = gameData[j];
                    dataSaverContents.Add(data.guid, data.saveDataContainers);
                }

                return dataSaverContents;
            }
        }

        /// <summary>
        /// Save the current data to a file
        /// </summary>
#if UNITY_EDITOR
        [MenuItem("Daniell/Save System/Save")]
#endif
        public static void Save()
        {
            // Retrieve registered DataSavers content
            Dictionary<int, List<DataSaverContent>> dataSaverContentByScene = new Dictionary<int, List<DataSaverContent>>();

            List<DataSaverContent> globalDataSavers = new List<DataSaverContent>();

            for (int i = 0; i < _registeredDataSavers.Count; i++)
            {
                var dataSaver = _registeredDataSavers[i];

                var listOfContainers = dataSaver.Save();
                var objectSceneID = dataSaver.SceneID;

                var dataSaverContent = new DataSaverContent() { guid = dataSaver.GUID, saveDataContainers = listOfContainers };

                if (dataSaver.IsGlobal)
                {
                    globalDataSavers.Add(dataSaverContent);
                }
                else
                {
                    // If the scene hasn't already been added
                    if (!dataSaverContentByScene.ContainsKey(objectSceneID))
                    {
                        // Create a new slot for the list
                        dataSaverContentByScene.Add(objectSceneID, new List<DataSaverContent>());
                    }

                    dataSaverContentByScene[objectSceneID].Add(dataSaverContent);
                }
            }

            // Create save files for each scene
            foreach (var dataSaverContent in dataSaverContentByScene)
            {
                var saveFileName = GetSaveFileName(dataSaverContent.Key);
                var saveFileData = dataSaverContent.Value.ToArray();
                CreateSaveFile(saveFileName, SelectedSlot, saveFileData);
            }

            // Create the global save file if there is global data to save
            if (globalDataSavers.Count > 0)
            {
                CreateSaveFile(GLOBAL_SAVE_FILE_NAME, SelectedSlot, globalDataSavers.ToArray());
            }
        }

        #endregion

        #region Save & Load Game Settings

        public static void SetGameSetting<T>(string settingID, T value)
        {

        }

        public static T GetGameSetting<T>(string settingID)
        {
            return default;
        }

        #endregion

#if UNITY_EDITOR
        [MenuItem("Daniell/Save System/Open Save Data Folder...")]
        public static void OpenSaveDataFolder()
        {
            string path = GetSaveFolderPath();
            path = path.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        [MenuItem("Daniell/Save System/Clear Save Data Folder")]
        public static void ClearData()
        {
            var directory = GetSaveFolderPath();
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }
#endif
    }
}