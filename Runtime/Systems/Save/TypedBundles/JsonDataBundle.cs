using System.Collections.Generic;
using System.IO;
using FM.Runtime.Helpers.DataStructures;
using UnityEngine;

namespace FM.Runtime.Systems.Save
{
	/// <summary>
	/// Bind and serialize data to a Json file
	/// </summary>
	[CreateAssetMenu]
	public class JsonDataBundle : DataBundle<string>
	{
		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Load(string filePath)
		{
			// Clear all the currently active data
			ClearData();

			// Read the whole file as text and convert from JSON
			var streamReader = new StreamReader(filePath);
			var rawJson = streamReader.ReadToEnd();
			streamReader.Close();

			// Check if the JSON file is not null, empty or whitespace
			if (!string.IsNullOrWhiteSpace(rawJson))
			{
				// Deserialize json into a value wrapper
				ValueWrapper<List<DataBlock>> rawDataBlock = JsonUtility.FromJson<ValueWrapper<List<DataBlock>>>(rawJson);

				// Get data from the wrapper
				List<DataBlock> data = rawDataBlock.value;

				// Fill the data bundle with data
				for (var i = 0; i < data.Count; i++)
				{
					DataBlock dataBlock = data[i];
					SetData(dataBlock.ID, DeserializeData<string>(dataBlock.SerializedData));
				}
			}
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public override void Save(string filePath)
		{
			// Convert current data to json
			var json = JsonUtility.ToJson(new ValueWrapper<List<DataBlock>>(_data), true);

			// Write the whole json content
			var streamWriter = new StreamWriter(filePath);
			streamWriter.Write(json);
			streamWriter.Close();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override T DeserializeData<T>(string serializedData)
		{
			ValueWrapper<T> data = JsonUtility.FromJson<ValueWrapper<T>>(serializedData);
			return data.value;
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		protected override string SerializeData<T>(T deserializedData)
		{
			var json = JsonUtility.ToJson(new ValueWrapper<T>(deserializedData));
			return json;
		}
	}
}