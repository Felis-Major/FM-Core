using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FM.Runtime.Core.Coroutines;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace FM.Runtime.Core.DataManagement
{
	/// <summary>
	/// Handles saving/loading data at runtime
	/// </summary>
	public static class DataManager
	{
		/// <summary>
		/// Key type for the dictionary to sort by files and keys
		/// </summary>
		private class DataKey
		{
			/* ==========================
			 * > Properties
			 * -------------------------- */

			/// <summary>
			/// ID of the file
			/// </summary>
			public string FileID { get; }

			/// <summary>
			/// Key of the data
			/// </summary>
			public string Key { get; }


			/* ==========================
			 * > Constructors
			 * -------------------------- */

			public DataKey(string fileID, string key)
			{
				FileID = fileID;
				Key = key;
			}


			/* ==========================
			 * > Methods
			 * -------------------------- */

			/// <summary>
			/// <inheritdoc/>
			/// </summary>
			public override bool Equals(object obj)
			{
				return obj is DataKey dK && dK.FileID == FileID && dK.Key == Key;
			}

			/// <summary>
			/// <inheritdoc/>
			/// </summary>
			public override int GetHashCode()
			{
				return FileID.GetHashCode() ^ Key.GetHashCode();
			}
		}


		/* ==========================
		 * > Constants
		 * -------------------------- */

		/// <summary>
		/// Extension of the save file
		/// </summary>
		private const string SaveFileExtension = ".json";

		/// <summary>
		/// Name for the locations file
		/// </summary>
		private const string LocationFileName = "_Locations" + SaveFileExtension;

		/// <summary>
		/// Name for the global save file
		/// </summary>
		private const string GlobalSaveFileName = "Global" + SaveFileExtension;

		/// <summary>
		/// Path relative to the persistent data path where the DataManager saves files
		/// </summary>
		private const string DataPath = "Data";

		/// <summary>
		/// Name of the default user slot
		/// </summary>
		private const string DefaultUserSlot = "_Default";


		/* ==========================
		 * > Properties
		 * -------------------------- */

		public static string UserSlot { get; set; }


		/* ==========================
		 * > Private Fields
		 * -------------------------- */

		private static Dictionary<DataKey, object> _savedData = new();  // Dictionary containing all the saved data
		private static readonly string _persistentDataFilePath;         // Path of the persistent data folder


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
		 * > Constructor
		 * -------------------------- */

		static DataManager()
		{
			// Cache persistent data path to use in async operations
			_persistentDataFilePath = Application.persistentDataPath;
			UserSlot = DefaultUserSlot;
		}


		/* ==========================
		 * > Methods
		 * -------------------------- */

		#region Save & Load

		/// <summary>
		/// Save data to a file
		/// </summary>	
		/// <remarks>This method runs and wait for <see cref="SaveAsync"/> to finish. Consider using <see cref="SaveAsync"/> or <see cref="DoSave"/> directly.</remarks>
		public static void Save()
		{
			var saveTask = Task.Run(SaveAsync);
			saveTask.Wait();
		}

		/// <summary>
		/// Save data to a file
		/// </summary>
		/// <remarks>This method waits for the <see cref="SaveAsync"/> method to finish using a <see cref="WaitForTask"/> yield instruction</remarks>
		/// <returns><see cref="IEnumerator"/> to be used as a <see cref="Coroutine"/></returns>
		public static IEnumerator DoSave()
		{
			yield return new WaitForTask(SaveAsync());
		}

		/// <summary>
		/// Asynchronously save data to a file
		/// </summary>
		public static async Task SaveAsync()
		{
			// Find all possible locations & store them inside of a lookup object
			var locations = new List<string>();
			ILookup<string, KeyValuePair<DataKey, object>> dataLookUp = _savedData.ToLookup(item =>
			{
				string fileID = item.Key.FileID;
				if (!locations.Contains(fileID))
				{
					locations.Add(fileID);
				}

				return fileID;
			});

			// Create the locations file
			string locationFilePath = BuildPath(LocationFileName, out string locationFileDirectory);
			string serializedLocations = Serialize(locations);

			// Create the directory if there isn't one 
			if (!Directory.Exists(locationFileDirectory))
			{
				Directory.CreateDirectory(locationFileDirectory);
			}

			await File.WriteAllTextAsync(locationFilePath, serializedLocations);

			// Create each file
			for (int i = 0; i < locations.Count; i++)
			{
				string location = locations[i];
				var fileContent = new Dictionary<string, object>();

				// Add data to the file
				foreach (KeyValuePair<DataKey, object> data in dataLookUp[location])
				{
					fileContent.Add(data.Key.Key, data.Value);
				}

				// Create the path of the file to be saved
				string filePath = BuildPath(location, out string dataFileDirectory);

				// Create the directory if there isn't one 
				if (!Directory.Exists(dataFileDirectory))
				{
					Directory.CreateDirectory(dataFileDirectory);
				}

				// Serialize data for that file only
				string jsonData = Serialize(fileContent);

				// Write to the file
				await File.WriteAllTextAsync(filePath, jsonData);
			}

			OnDataSaved?.Invoke();
		}

		/// <summary>
		/// Load data from a file
		/// </summary>
		/// <remarks>This method runs and wait for <see cref="LoadAsync"/> to finish. Consider using <see cref="LoadAsync"/> or <see cref="DoLoad"/> directly.</remarks>
		public static void Load()
		{
			var loadTask = Task.Run(LoadAsync);
			loadTask.Wait();
		}

		/// <summary>
		/// Load data from a file
		/// </summary>
		/// <remarks>This method waits for the <see cref="LoadAsync"/> method to finish using a <see cref="WaitForTask"/> yield instruction</remarks>
		/// <returns><see cref="IEnumerator"/> to be used as a <see cref="Coroutine"/></returns>
		public static IEnumerator DoLoad()
		{
			yield return new WaitForTask(LoadAsync());
		}

		/// <summary>
		/// Asynchronously load data from a file
		/// </summary>
		public static async Task LoadAsync()
		{
			_savedData.Clear();

			// Find all the locations	
			string locationFilePath = BuildPath(LocationFileName, out string _);
			if (File.Exists(locationFilePath))
			{
				string serializedLocations = await File.ReadAllTextAsync(locationFilePath);
				string[] locations = Deserialize<string[]>(serializedLocations);

				for (int i = 0; i < locations.Length; i++)
				{
					string location = locations[i];

					// Load data from the file
					string filePath = BuildPath(location, out string _);

					if (File.Exists(filePath))
					{
						string fileContent = await File.ReadAllTextAsync(filePath);
						Dictionary<string, object> fileData = Deserialize<Dictionary<string, object>>(fileContent);

						// Add each keys to the current saved data
						foreach (KeyValuePair<string, object> data in fileData)
						{
							var dataKey = new DataKey(location, data.Key);
							_savedData.Add(dataKey, data.Value);
						}
					}
				}
			}

			OnDataLoaded?.Invoke();
		}

		#endregion


		#region Runtime Data

		/// <summary>
		/// Set/Add a new key to the runtime saved data inside the global data file
		/// </summary>
		/// <param name="key">Unique value identifier</param>
		/// <param name="value">Value to save</param>
		public static void SetValue(string key, object value)
		{
			SetValue(GlobalSaveFileName, key, value);
		}

		/// <summary>
		/// Set/Add a new key to the runtime saved data
		/// </summary>
		/// <param name="fileID">ID of the file</param>
		/// <param name="key">Unique value identifier</param>
		/// <param name="value">Value to save</param>
		public static void SetValue(string fileID, string key, object value)
		{
			var dataKey = new DataKey(fileID, key);
			_savedData[dataKey] = value;
		}

		/// <summary>
		/// Retrieves a value from the runtime saved data
		/// </summary>
		/// <typeparam name="T">Type of the value to cast to</typeparam>
		/// <param name="key">Key of the value to get</param>
		/// <param name="value">Found value as <typeparamref name="T"/></param>
		/// <returns>True if a value was found for the <paramref name="key"/></returns>
		/// <remarks>Will throw an exception if the type it was deserialized into is not either <see cref="IConvertible"/> or <see cref="JToken"/></remarks>
		/// <exception cref="ArgumentException"></exception>
		public static bool GetValue<T>(string key, out T value)
		{
			return GetValue(GlobalSaveFileName, key, out value);
		}

		/// <summary>
		/// Retrieves a value from the runtime saved data
		/// </summary>
		/// <typeparam name="T">Type of the value to cast to</typeparam>
		/// <param name="fileID">ID of the file</param>
		/// <param name="key">Key of the value to get</param>
		/// <param name="value">Found value as <typeparamref name="T"/></param>
		/// <returns>True if a value was found for the <paramref name="key"/></returns>
		/// <remarks>Will throw an exception if the type it was deserialized into is not either <see cref="IConvertible"/> or <see cref="JToken"/></remarks>
		/// <exception cref="ArgumentException"></exception>
		public static bool GetValue<T>(string fileID, string key, out T value)
		{
			var dataKey = new DataKey(fileID, key);
			if (_savedData.TryGetValue(dataKey, out object savedValue))
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
		/// Removes a value from the runtime saved data inside the global save file
		/// </summary>
		/// <param name="key">ID of the value to remove</param>
		public static void RemoveValue(string key)
		{
			RemoveValue(GlobalSaveFileName, key);
		}

		/// <summary>
		/// Removes a value from the runtime saved data inside the global save file
		/// </summary>
		/// <param name="fileID">ID of the file</param>
		/// <param name="key">ID of the value to remove</param>
		public static void RemoveValue(string fileID, string key)
		{
			var dataKey = new DataKey(fileID, key);
			_savedData.Remove(dataKey);
		}

		/// <summary>
		/// Clear the content of the runtime data
		/// </summary>
		public static void ClearValues()
		{
			_savedData.Clear();
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

		/// <summary>
		/// Builds a path to a given file
		/// </summary>
		/// <param name="fileName">Name of the file to get the path for</param>
		/// <returns>Path of the file</returns>
		private static string BuildPath(string fileName, out string directory)
		{
			string file = fileName + SaveFileExtension;
			directory = Path.Combine(_persistentDataFilePath, DataPath, UserSlot);
			string path = Path.Combine(directory, file);
			return path;
		}

		#endregion
	}
}
