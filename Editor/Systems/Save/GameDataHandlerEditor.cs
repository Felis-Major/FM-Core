using FM.Runtime.Systems.Save;
using UnityEditor;

namespace FM.Editor.Systems.Save
{
    /// <summary>
    /// List of methods used by the Game Data Handler to save and load data
    /// </summary>
    public static class GameDataHandlerEditor
    {
        /// <summary>
        /// Save Game Data in permanent file (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Save/Permanent")]
        public static async void SavePermanent()
        {
            await GameDataHandler.Save(GameDataHandler.DataPersistency.Permanent);
        }

        /// <summary>
        /// Save Game Data in temporary file (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Save/Temporary")]
        public static async void SaveTemporary()
        {
            await GameDataHandler.Save(GameDataHandler.DataPersistency.Temporary);
        }

        /// <summary>
        /// Load Game Data from permanent file (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Load/Permanent")]
        public static async void LoadPermanent()
        {
            await GameDataHandler.Load(GameDataHandler.DataPersistency.Permanent);
        }

        /// <summary>
        /// Load Game Data from temporary file (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Load/Temporary")]
        public static async void LoadTemporary()
        {
            await GameDataHandler.Load(GameDataHandler.DataPersistency.Temporary);
        }

        /// <summary>
        /// Make Temporary Data Permanent (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Make Temp Data Permanent")]
        public static async void SaveTempData()
        {
            await GameDataHandler.MakeTemporaryDataPermanent();
        }

        /// <summary>
        /// Clear Temp Data (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Clear Temp data")]
        public static async void ClearTempData()
        {
            await GameDataHandler.FlushTempData();
        }

        /// <summary>
        /// Open the save data folder (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Open Save Data Folder...")]
        public static void OpenSaveDataFolder()
        {
            string path = GameDataHandler.BaseSaveDataPath;
            path = path.Replace(@"/", @"\");
            System.Diagnostics.Process.Start("explorer.exe", path);
        }

        /// <summary>
        /// Clear the saved data (editor only)
        /// </summary>
        [MenuItem("Felis Major/Save System/Clear Save Data Folder")]
        public static async void ClearData()
        {
            await GameDataHandler.ClearAllSavedData();
        }
    }
}