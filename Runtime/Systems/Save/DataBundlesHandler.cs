using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FM.Runtime.Systems.Save
{
	public static class DataBundlesHandler
	{
		public const string SaveableAssetLocation = "Saveables";

		private static DataBundle[] GetAllDataBundles()
		{
			// Load all objects from the resources folder
			object[] objects = Resources.LoadAll<Object>(SaveableAssetLocation);
			List<DataBundle> dataBundles = new();

			// Find all the objects that are databundles and cache them into a list
			for (int i = 0; i < objects.Length; i++)
			{
				object obj = objects[i];
				if (obj is DataBundle saveableAsset)
				{
					dataBundles.Add(saveableAsset);
				}
			}

			// Return the list as array for convenience
			return dataBundles.ToArray();
		}

		/// <summary>
		/// Find the path of the file that will be created
		/// </summary>
		/// <param name="fileName">Name of the file to be created</param>
		/// <param name="filePath">Complete path of the file</param>
		/// <remarks>Will create a new file/directory if it doesn't already exist</remarks>
		/// <returns>True if the file already exists</returns>
		private static void GetFilePath(string fileName, out string filePath, out bool wasFileCreated)
		{
			// Initialize output
			wasFileCreated = false;

			// Find the path of the file
			string fileDirectory = $"{Application.persistentDataPath}/{SaveableAssetLocation}/";
			filePath = Path.Combine(fileDirectory, fileName);

			// Evaluate directories
			bool doesDirectoryExist = Directory.Exists(fileDirectory);
			bool doesFileExist = File.Exists($"{fileDirectory}/{fileName}");
			bool shouldCreateFile = !(doesDirectoryExist && doesFileExist);

			// Create path if it doesn't already exist
			if (shouldCreateFile)
			{
				Directory.CreateDirectory(fileDirectory);
				FileStream fileStream = File.Create(filePath);
				fileStream.Close();
				wasFileCreated = true;
			}
		}

#if UNITY_EDITOR
		[MenuItem("Felis Major/Load")]
#endif
		public static void Load()
		{
			// Load all scriptables from resources
			DataBundle[] dataBundles = GetAllDataBundles();

			for (int i = 0; i < dataBundles.Length; i++)
			{
				GetFilePath(dataBundles[i].FileName, out string filePath, out _);
				dataBundles[i].Load(filePath);
			}
		}

#if UNITY_EDITOR
		[MenuItem("Felis Major/Save")]
#endif
		public static void Save()
		{
			// save all scriptables in resources
			DataBundle[] dataBundles = GetAllDataBundles();

			for (int i = 0; i < dataBundles.Length; i++)
			{
				GetFilePath(dataBundles[i].FileName, out string filePath, out _);
				dataBundles[i].Save(filePath);
			}
		}

#if UNITY_EDITOR
		[MenuItem("Felis Major/Open persistent data path...")]
		public static void OpenPersistentDataPath()
		{
			// fix this
			string saveDirectory = $"{Application.persistentDataPath}/{SaveableAssetLocation}/";
			UnityEngine.Debug.Log(saveDirectory);
			Process.Start("explorer.exe", saveDirectory);
		}
#endif
	}
}