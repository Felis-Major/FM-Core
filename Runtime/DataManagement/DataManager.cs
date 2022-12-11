using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

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
		 * > Properties
		 * -------------------------- */

		public static string PersistentDataFilePath => Application.persistentDataPath + "/save.json";


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

		#region Save & Load

		/// <summary>
		/// Save data to a file
		/// </summary>
		public static void Save()
		{
			string jsonData = Serialize(_savedKeys);
			File.WriteAllText(PersistentDataFilePath, jsonData);
		}

		/// <summary>
		/// Asynchronously save data to a file
		/// </summary>
#if UNITY_EDITOR
		[MenuItem("Felis Major/Data Management/Save")]
#endif
		public static async void SaveAsync()
		{
			string jsonData = Serialize(_savedKeys);
			await File.WriteAllTextAsync(PersistentDataFilePath, jsonData);
			OnDataSaved?.Invoke();
		}

		/// <summary>
		/// Load data from a file
		/// </summary>
		public static void Load()
		{
			if (File.Exists(PersistentDataFilePath))
			{
				string json = File.ReadAllText(PersistentDataFilePath);
				_savedKeys = Deserialize<Dictionary<string, object>>(json);
			}
			else
			{
				_savedKeys = new Dictionary<string, object>();
			}
		}

		/// <summary>
		/// Asynchronously load data from a file
		/// </summary>
#if UNITY_EDITOR
		[MenuItem("Felis Major/Data Management/Load")]
#endif
		public static async void LoadAsync()
		{
			if (File.Exists(PersistentDataFilePath))
			{
				string json = await File.ReadAllTextAsync(PersistentDataFilePath);
				_savedKeys = Deserialize<Dictionary<string, object>>(json);
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


		#region	Helpers

		/// <summary>
		/// Serialize data using the default <see cref="JsonConverter"/>
		/// </summary>
		/// <typeparam name="T">Type of data to serialize</typeparam>
		/// <param name="data">Data instance to serialize</param>
		/// <returns>Serialized data as json <see cref="string"/></returns>
		private static string Serialize<T>(T data)
		{
			return JsonConvert.SerializeObject(data, Formatting.Indented);
		}

		/// <summary>
		/// Deserialize a json <see cref="string"/> into <typeparamref name="T"/>
		/// </summary>
		/// <typeparam name="T">Type of the serialized data</typeparam>
		/// <param name="serializedData">Json <see cref="string"/> to deserialize</param>
		/// <returns>Instance of the deserialized data as <typeparamref name="T"/></returns>
		private static T Deserialize<T>(string serializedData)
		{
			return JsonConvert.DeserializeObject<T>(serializedData);
		}

		#endregion
	}
}
