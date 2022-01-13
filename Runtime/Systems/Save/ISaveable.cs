namespace Daniell.Runtime.Systems.Save
{
    /// <summary>
    /// Object that can be saved
    /// </summary>
    public interface ISaveable
    {
        /// <summary>
        /// Unique ID for this object
        /// </summary>
        string GUID { get; }

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