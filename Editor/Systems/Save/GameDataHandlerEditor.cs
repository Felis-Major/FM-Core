using Daniell.Runtime.Helpers.Logging;
using Daniell.Runtime.Systems.Save;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Daniell.Editor.Systems.Save
{
    /// <summary>
    /// List of methods used by the Game Data Handler to save and load data
    /// </summary>
    public static class GameDataHandlerEditor
    {
        /// <summary>
        /// Save Game Data in permanent file (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Save/Permanent")]
        public static void SavePermanent()
        {
            GameDataHandler.Save(GameDataHandler.DataPersistency.Permanent);
        }

        /// <summary>
        /// Save Game Data in temporary file (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Save/Temporary")]
        public static void SaveTemporary()
        {
            GameDataHandler.Save(GameDataHandler.DataPersistency.Temporary);
        }

        /// <summary>
        /// Load Game Data from permanent file (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Load/Permanent")]
        public static void LoadPermanent()
        {
            GameDataHandler.Load(GameDataHandler.DataPersistency.Permanent);
        }

        /// <summary>
        /// Load Game Data from temporary file (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Load/Temporary")]
        public static void LoadTemporary()
        {
            GameDataHandler.Load(GameDataHandler.DataPersistency.Temporary);
        }

        /// <summary>
        /// Make Temporary Data Permanent (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Make Temp Data Permanent")]
        public static void SaveTempData()
        {
            GameDataHandler.MakeTemporaryDataPermanent();
        }

        /// <summary>
        /// Clear Temp Data (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Clear Temp data")]
        public static void ClearTempData()
        {
            GameDataHandler.FlushTempData();
        }

        /// <summary>
        /// Open the save data folder (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Open Save Data Folder...")]
        public static void OpenSaveDataFolder()
        {
            string path = GameDataHandler.BaseSaveDataPath;
            path = path.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        /// <summary>
        /// Clear the saved data (editor only)
        /// </summary>
        [MenuItem("Daniell/Save System/Clear Save Data Folder")]
        public static void ClearData()
        {
            var directory = GameDataHandler.BaseSaveDataPath;
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
                ConsoleLogger.Log("Game Data erased", Color.red, ConsoleLogger.LogType.Important);
            }
        }
    }
}