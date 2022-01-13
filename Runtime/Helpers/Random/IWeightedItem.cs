using UnityEngine;

namespace Daniell.Runtime.Helpers.Random
{
    /// <summary>
    /// Interface for an item that can be randomly picked using weight
    /// </summary>
    /// <typeparam name="T">Value of the item</typeparam>
    public interface IWeightedItem<T>
    {
        /// <summary>
        /// Value of the item
        /// </summary>
        T Value { get; }

        /// <summary>
        /// Weight of the item
        /// </summary>
        int Weight { get; }
    }
}