using System.Collections.Generic;
using UnityEngine;

namespace Daniell.Runtime.Helpers.Random
{
    /// <summary>
    /// Handles selecting a random item from a list using weighted probabilities
    /// </summary>
    /// <typeparam name="T">Value of the item</typeparam>
    public class WeightedRandomSelector<T>
    {
        /* ==========================
         * > Private Fields
         * -------------------------- */

        private List<IWeightedItem<T>> _items = new List<IWeightedItem<T>>();
      
        
        /* ==========================
         * > Methods
         * -------------------------- */

        /// <summary>
        /// Add a new Weighted Item to the selector
        /// </summary>
        /// <param name="entry">Weighted Item</param>
        public void AddEntry(IWeightedItem<T> entry)
        {
            _items.Add(entry);
        }

        /// <summary>
        /// Add multiple Weighted Items to the selector
        /// </summary>
        /// <param name="entries">Weighted Items</param>
        public void AddEntries(IWeightedItem<T>[] entries)
        {
            _items.AddRange(entries);
        }

        /// <summary>
        /// Remove a Weighted Item from the selector
        /// </summary>
        /// <param name="entry">Weighted Item</param>
        public void RemoveEntry(IWeightedItem<T> entry)
        {
            _items.Remove(entry);
        }

        /// <summary>
        /// Clear the selector
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Return a random Item from the selector
        /// </summary>
        /// <returns>Randomly picked Item</returns>
        public T GetRandomItem()
        {
            // Add all the weights
            int totalWeight = 0;

            for (int i = 0; i < _items.Count; i++)
            {
                totalWeight += _items[i].Weight;
            }

            // Pick a random index
            int randomIndex = UnityEngine.Random.Range(0, totalWeight);

            int j = 0;

            // Find the item that was selected
            for (int i = 0; i < _items.Count; i++)
            {
                IWeightedItem<T> item = _items[i];
                j += item.Weight;

                if (j > randomIndex)
                {
                    return item.Value;
                }
            }

            return default;
        }
    }
}

