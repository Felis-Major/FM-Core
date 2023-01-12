using System.Collections.Generic;

namespace FM.Runtime.Helpers.Random
{
	/// <summary>
	/// Handles selecting a random <see cref="WeightedItem{T}"/> from a list using chances
	/// </summary>
	/// <typeparam name="T">Value of the item</typeparam>
	public class ProbabilisticRandomSelector<T> : RandomSelector<T>
	{
		/* ==========================
		 * > Private Fields
		 * -------------------------- */

		private readonly int _scale;    // Scale at which to read probabilities


		/* ==========================
		 * > Constructors
		 * -------------------------- */

		public ProbabilisticRandomSelector()
		{
			_scale = 100;
		}

		public ProbabilisticRandomSelector(int scale)
		{
			_scale = scale;
		}


		/* ==========================
		 * > Methods
		 * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override T[] GetRandomItems()
		{
			var selectedItems = new List<T>();

			for (int i = 0; i < _items.Count; i++)
			{
				float randomChance = UnityEngine.Random.value * _scale;
				WeightedItem<T> item = _items[i];
				if (randomChance <= item.Weight)
				{
					selectedItems.Add(item.Value);
				}
			}

			return selectedItems.ToArray();
		}
	}
}