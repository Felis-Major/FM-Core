namespace Daniell.Runtime.Helpers.DataStructures
{
    /// <summary>
    /// Wrapper for a single value to enable JSON conversion
    /// </summary>
    /// <typeparam name="T">Type of the value</typeparam>
    [System.Serializable]
    public struct ValueWrapper<T>
    {
        /// <summary>
        /// Value of the Wrapper
        /// </summary>
        public T value;

        public ValueWrapper(T value)
        {
            this.value = value;
        }
    }
}