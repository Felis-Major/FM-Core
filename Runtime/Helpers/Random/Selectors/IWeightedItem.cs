namespace FM.Runtime.Helpers.Random
{
    /// <summary>
    /// Base class for a weighted item
    /// </summary>
    public interface IWeightedItem
    {
        /// <summary>
        /// Weight of the item
        /// </summary>
        int Weight { get; }
    }

    /// <summary>
    /// Interface for an item that can be randomly picked using weight
    /// </summary>
    /// <typeparam name="T">Value of the item</typeparam>
    public interface IWeightedItem<T> : IWeightedItem
    {
        /// <summary>
        /// Value of the item
        /// </summary>
        T Value { get; }
    }
}