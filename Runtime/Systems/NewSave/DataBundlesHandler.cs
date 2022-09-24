using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

public static class DataBundlesHandler
{
	public const string SaveableAssetLocation = "Saveables";
	public const string FileExtension = "txt";

	private static DataBundle[] GetAllDataBundles()
	{
		object[] objects = Resources.LoadAll<Object>(SaveableAssetLocation);
		List<DataBundle> dataBundles = new();

		for (var i = 0; i < objects.Length; i++)
		{
			var obj = objects[i];
			if (obj is DataBundle saveableAsset)
			{
				dataBundles.Add(saveableAsset);
			}
		}

		return dataBundles.ToArray();
	}

	private static string GetFilePath(string fileName)
	{
		return $"{Application.persistentDataPath}/{SaveableAssetLocation}/{fileName}.{FileExtension}";
	}

	[MenuItem("Felis Major/Load")]
	public static void Load()
	{
		// Load all scriptables from resources
		DataBundle[] dataBundles = GetAllDataBundles();

		for (var i = 0; i < dataBundles.Length; i++)
		{
			// Build filepath
			var filePath = GetFilePath(dataBundles[i].FileName);
			dataBundles[i].Load(filePath);
		}
	}

	[MenuItem("Felis Major/Save")]
	public static void Save()
	{
		// save all scriptables in resources
		DataBundle[] dataBundles = GetAllDataBundles();

		for (var i = 0; i < dataBundles.Length; i++)
		{
			// Build filepath
			var filePath = GetFilePath(dataBundles[i].FileName);
			dataBundles[i].Save(filePath);
		}
	}
}
