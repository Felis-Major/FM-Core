namespace FM.Runtime.Helpers.Random
{
    /// <summary>
    /// Item that can be randomly picked using its weight
    /// </summary>
    /// <typeparam name="T">Value type of the item</typeparam>
    public class WeightedItem<T>
    {
        /* ==========================
         * > Properties
         * -------------------------- */

        /// <summary>
        /// Weight of the item
        /// </summary>
        public int Weight { get; }

        /// <summary>
        /// Value of the item
        /// </summary>
        public T Value { get; }


        /* ==========================
         * > Constructor
         * -------------------------- */

        public WeightedItem(int weight, T value)
        {
            Weight = weight;
            Value = value;
        }
    }
}