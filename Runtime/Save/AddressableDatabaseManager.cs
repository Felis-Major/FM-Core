using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace FM.Runtime.Core.DataManagement
{
	[Serializable]
	public struct AddressableInfo
	{
		public string tag;
		public string name;
	}

	public static class AddressableDatabaseManager
	{
		private static Dictionary<string, UnityEngine.Object[]> _addressableDatabase = new();

		public static async void InitializeTag(string tag)
		{
			if (!_addressableDatabase.ContainsKey(tag))
			{
				// Load all assets asynchronously
				AsyncOperationHandle<IList<UnityEngine.Object>> assets = Addressables.LoadAssetsAsync<UnityEngine.Object>(tag, x => { });

				// Hang the main thread while retrieving assets
				IList<UnityEngine.Object> storedData = await assets.Task;

				// Convert the list to array once retrieved
				var dataArray = new UnityEngine.Object[storedData.Count];
				for (int i = 0; i < dataArray.Length && i < storedData.Count; i++)
				{
					dataArray[i] = storedData[i];
				}

				_addressableDatabase.Add(tag, storedData.ToArray());
			}
		}

		public static bool TryGetItem<T>(AddressableInfo addressableInfo, out T item) where T : UnityEngine.Object
		{
			if (_addressableDatabase.TryGetValue(addressableInfo.tag, out UnityEngine.Object[] objects))
			{
				foreach (UnityEngine.Object v in objects)
				{
					if (v.name == addressableInfo.name)
					{
						item = (T)v;
						return true;
					}
				}
			}

			item = null;
			return false;
		}
	}
}
