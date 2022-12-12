using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

#if UNITY_EDITOR
#endif

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


		/* ==========================
		 * > Private Fields
		 * -------------------------- */

		private static Dictionary<DataKey, object> _savedData = new();  // Dictionary containing all the saved data
		private static readonly string persistentDataFilePath;          // Path of the persistent data folder


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
			// Cache persistent data path to use in other threads
			persistentDataFilePath = Application.persistentDataPath;
		}


		/* ==========================
		 * > Methods
		 * -------------------------- */

		#region Save & Load

		/// <summary>
		/// Save data to a file
		/// </summary>
		public static void Save()
		{
			// Find all possible locations
			_savedData.GroupBy(item => item.Key.FileID);









			// Sort dictionary by file ID
			KeyValuePair<DataKey, object>[] sortedSaveData = _savedData.OrderBy(item => item.Key.FileID).ToArray();

			// Initialize file parameters
			var locations = new List<string>();
			string currentFileID = string.Empty;
			var currentFileContent = new Dictionary<string, object>();

			// todo list all the possible locations, instantiate new dictionaries and populate with data + 




			// Iterate over each data in order
			for (int i = 0; i < sortedSaveData.Length; i++)
			{
				KeyValuePair<DataKey, object> dataToSave = sortedSaveData[i];
				string fileID = dataToSave.Key.FileID;
				string key = dataToSave.Key.Key;

				// Do not support location name 
				if (fileID == LocationFileName)
				{
					throw new ArgumentException($"The name {LocationFileName} cannot be used.");
				}

				// Add the data to the file content
				currentFileContent.Add(key, dataToSave.Value);

				// If we're iterating over a new type of file, or it's the last iteration
				bool isNewFile = fileID != currentFileID;
				bool isLastIteration = i == sortedSaveData.Length - 1;
				if (isNewFile || isLastIteration)
				{
					// If there is content to save
					if (currentFileContent.Count > 0)
					{
						Debug.Log(fileID + " File Content: ");
						foreach (KeyValuePair<string, object> item in currentFileContent)
						{
							Debug.Log(item.Key);
						}

						// Create the path of the file to be saved
						string filePath = BuildPath(fileID);

						// Serialize data for that file only
						string jsonData = Serialize(currentFileContent);

						// Write to the file
						File.WriteAllText(filePath, jsonData);

						// Clear file content
						currentFileContent.Clear();
					}

					// Set to the new file ID
					currentFileID = fileID;
					locations.Add(fileID);
				}
			}

			// Create the locations file
			string serializedLocations = Serialize(locations);
			string locationFilePath = BuildPath(LocationFileName);
			File.WriteAllText(locationFilePath, serializedLocations);
		}

		/// <summary>
		/// Asynchronously save data to a file
		/// </summary>
		public static async void SaveAsync()
		{

		}

		/// <summary>
		/// Load data from a file
		/// </summary>
		public static void Load()
		{
			_savedData.Clear();

			// Find all the locations	
			string locationFilePath = BuildPath(LocationFileName);
			if (File.Exists(locationFilePath))
			{
				string serializedLocations = File.ReadAllText(locationFilePath);
				string[] locations = Deserialize<string[]>(serializedLocations);

				for (int i = 0; i < locations.Length; i++)
				{
					string location = locations[i];

					// Load data from the file
					string filePath = BuildPath(location);

					if (File.Exists(filePath))
					{
						string fileContent = File.ReadAllText(filePath);
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
		}

		/// <summary>
		/// Asynchronously load data from a file
		/// </summary>
		public static async void LoadAsync()
		{

		}

		#endregion


		#region Runtime Data

		public static void Clear()
		{
			_savedData.Clear();
		}

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
		public static void ClearValue(string key)
		{
			ClearValue(GlobalSaveFileName, key);
		}

		/// <summary>
		/// Removes a value from the runtime saved data inside the global save file
		/// </summary>
		/// <param name="fileID">ID of the file</param>
		/// <param name="key">ID of the value to remove</param>
		public static void ClearValue(string fileID, string key)
		{
			var dataKey = new DataKey(fileID, key);
			_savedData.Remove(dataKey);
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
		private static string BuildPath(string fileName)
		{
			return Path.Combine(persistentDataFilePath, fileName + SaveFileExtension);
		}

		#endregion
	}
}
