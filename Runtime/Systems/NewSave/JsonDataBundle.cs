using System.Collections.Generic;
using System.IO;
using FM.Runtime.Helpers.DataStructures;
using UnityEngine;

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
		ClearData();
		var json = File.ReadAllText(filePath);
		List<DataBlock> data = JsonUtility.FromJson<ValueWrapper<List<DataBlock>>>(json).value;

		for (var i = 0; i < data.Count; i++)
		{
			DataBlock dataBlock = data[i];
			SetData(dataBlock.Guid, DeserializeData<string>(dataBlock.SerializedData));
		}
	}

	/// <summary>
	/// <inheritdoc/>
	/// </summary>
	public override void Save(string filePath)
	{
		var json = JsonUtility.ToJson(new ValueWrapper<List<DataBlock>>(_data), true);
		File.WriteAllText(filePath, json);
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