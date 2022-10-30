namespace FM.Runtime.Helpers.Random
{
	/// <summary>
	/// Handles selecting a random <see cref="IWeightedItem{T}"/> from a list using weights
	/// </summary>
	/// <typeparam name="T">Value of the item</typeparam>
	public class WeightedRandomSelector<T> : RandomSelector<T>
	{
		/* ==========================
		 * > Methods
		 * -------------------------- */

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override T[] GetRandomItems()
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
					return new T[1] { item.Value };
				}
			}

			return new T[0];
		}
	}
}

