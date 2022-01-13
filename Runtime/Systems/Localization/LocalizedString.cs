using UnityEngine;

namespace Daniell.Runtime.Systems.Localization
{
    /// <summary>
    /// String container that will return its content in the current language.
    /// </summary>
    [System.Serializable]
    public class LocalizedString
    {
        /// <summary>
        /// Key of the translated string.
        /// </summary>
        public string Key => _key;

        /// <summary>
        /// Table in which to look for the key.
        /// </summary>
        public LocalizationTable Table => _table;

        [SerializeField]
        [Tooltip("Key of the translated string")]
        private string _key;

        [SerializeField]
        [Tooltip("Table in which to look for the key")]
        private LocalizationTable _table;

        #region Conversion

        public static implicit operator string(LocalizedString localizedString)
        {
            return LocalizationManager.GetData(localizedString.Table, localizedString.Key);
        }

        public override string ToString() => this;

        #endregion
    }
}
