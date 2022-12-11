using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace FM.Runtime.Core.DataManagement
{
	/// <summary>
	/// Handles saving/loading data at runtime
	/// </summary>
	public static class DataManager
	{
		/* ==========================
		 * > Private Fields
		 * -------------------------- */

		private static Dictionary<string, object> _savedKeys = new();   // Dictionary containing all the saved data


		/* ==========================
		 * > Events
		 * -------------------------- */

		/// <summary>
		/// Called when the data is saved
		/// </summary>
		/// <remarks>Is called when the async <see cref="SaveAsync"/> method is done</remarks>
		public static event Action OnDataSaved;

		/// <summary>
		/// Called when the data is loaded
		/// </summary>
		/// <remarks>Is called from the async <see cref="LoadAsync"/> method is done</remarks>
		public static event Action OnDataLoaded;


		/* ==========================
		 * > Methods
		 * -------------------------- */

		#region File IO

		/// <summary>
		/// Asynchronously save data to a file
		/// </summary>
#if UNITY_EDITOR
		[MenuItem("Felis Major/Data Management/Save")]
#endif
		public static async void SaveAsync()
		{
			string jsonData = JsonConvert.SerializeObject(_savedKeys, Formatting.Indented);

			// Write to file
			await File.WriteAllTextAsync(Application.persistentDataPath + "/save.json", jsonData);

			OnDataSaved?.Invoke();
		}

		/// <summary>
		/// Asynchronously load data from a file
		/// </summary>
#if UNITY_EDITOR
		[MenuItem("Felis Major/Data Management/Load")]
#endif
		public static async void LoadAsync()
		{
			// Load from file
			if (File.Exists(Application.persistentDataPath + "/save.json"))
			{
				string json = await File.ReadAllTextAsync(Application.persistentDataPath + "/save.json");

				_savedKeys = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
			}
			else
			{
				_savedKeys = new Dictionary<string, object>();
			}

			OnDataLoaded?.Invoke();
		}

		#endregion

		#region Runtime Data

		/// <summary>
		/// Set/Add a new key to the runtime saved data
		/// </summary>
		/// <param name="key">Unique value identifier</param>
		/// <param name="value">Value to save</param>
		public static void SetValue(string key, object value)
		{
			_savedKeys[key] = value;
		}

		/// <summary>
		/// Retrieves a value from the runtime saved data
		/// </summary>
		/// <typeparam name="T">Type of the value to cast to</typeparam>
		/// <param name="key">Key of the value to get</param>
		/// <param name="value">Found value as <typeparamref name="T"/></param>
		/// <returns>True if a value was found for the <paramref name="key"/></returns>
		public static bool GetValue<T>(string key, out T value)
		{
			if (_savedKeys.TryGetValue(key, out object savedValue))
			{
				value = savedValue switch
				{
					// Convert immediately if it implements IConvertible
					IConvertible => (T)Convert.ChangeType(savedValue, typeof(T)),

					// Convert to object if it's a JToken
					JToken jToken => jToken.ToObject<T>(),

					// Don't handle other types
					_ => throw new ArgumentException($"Key {key} was deserialized as an unsupported type: {savedValue.GetType()}."),
				};

				return true;
			}

			value = default;
			return false;
		}

		/// <summary>
		/// Removes a value from the runtime saved data
		/// </summary>
		/// <param name="key">ID of the value to remove</param>
		public static void ClearValue(string key)
		{
			_savedKeys.Remove(key);
		}

		#endregion
	}
}
