namespace FM.Runtime.Systems.Save
{
    /// <summary>
    /// Object that can be saved
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Load currently saved data
        /// </summary>
        /// <param name="saveDataContainer">Container for the data</param>
        void Load(SaveDataContainer saveDataContainer);

        /// <summary>
        /// Save currently loaded data
        /// </summary>
        /// <param name="saveDataContainer">Container for the data</param>
        void Save(SaveDataContainer saveDataContainer);
    }
}