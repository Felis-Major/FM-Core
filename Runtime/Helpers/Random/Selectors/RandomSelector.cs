using System.Collections.Generic;

namespace FM.Runtime.Helpers.Random
{
	/// <summary>
	/// Base class for a random selection calculator
	/// </summary>
	/// <typeparam name="T">Type of data to be selected</typeparam>
	public abstract class RandomSelector<T>
	{
		/* ==========================
		* > Private Fields
		* -------------------------- */

		protected List<IWeightedItem<T>> _items = new();


		/* ==========================
         * > Methods
         * -------------------------- */

		#region Add & Remove

		/// <summary>
		/// Add a new <see cref="IWeightedItem{T}"/> to the <see cref="RandomSelector{T}"/>
		/// </summary>
		/// <param name="entry">Weighted Item</param>
		public void AddEntry(IWeightedItem<T> entry)
		{
			_items.Add(entry);
		}

		/// <summary>
		/// Add multiple <see cref="IWeightedItem{T}"/> to the <see cref="RandomSelector{T}"/>
		/// </summary>
		/// <param name="entries">Weighted Items</param>
		public void AddEntries(IWeightedItem<T>[] entries)
		{
			_items.AddRange(entries);
		}

		/// <summary>
		/// Remove a <see cref="IWeightedItem{T}"/> from the <see cref="RandomSelector{T}"/>
		/// </summary>
		/// <param name="entry">Weighted Item</param>
		public void RemoveEntry(IWeightedItem<T> entry)
		{
			_items.Remove(entry);
		}

		/// <summary>
		/// Clear the <see cref="RandomSelector{T}"/>
		/// </summary>
		public void Clear()
		{
			_items.Clear();
		}

		#endregion

		/// <summary>
		/// Return a randomly picked array of <typeparamref name="T"/> from the selector
		/// </summary>
		/// <returns>Randomly picked array of <typeparamref name="T"/></returns>
		public abstract T[] GetRandomItems();
	}
}

