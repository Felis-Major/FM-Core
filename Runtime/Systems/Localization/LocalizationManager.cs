using System;

namespace Daniell.Runtime.Systems.Localization
{
    /// <summary>
    /// Static helper to manage current language.
    /// </summary>
    public static class LocalizationManager
    {
        /// <summary>
        /// Current active language.
        /// </summary>
        public static string CurrentLanguage
        {
            get => _currentLanguage;
            set
            {
                _currentLanguage = value;
                OnLanguageChanged?.Invoke();
            }
        }

        // Private fields
        private static string _currentLanguage;

        /// <summary>
        /// Triggered when a new language has been set.
        /// </summary>
        public static event Action OnLanguageChanged;

        /// <summary>
        /// Get Data from table and key for the current language.
        /// </summary>
        /// <param name="table">Table to look into</param>
        /// <param name="key">Key to look for</param>
        /// <returns>Data at key</returns>
        public static string GetData(LocalizationTable table, string key)
        {
            return table[key, CurrentLanguage];
        }
    }
}
