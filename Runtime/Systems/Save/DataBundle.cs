using System;
using System.Collections.Generic;
using UnityEngine;

namespace FM.Runtime.Systems.Save
{
	/// <summary>
	/// Base class for <see cref="DataBundle{TSerializedType}"/>
	/// </summary>
	public abstract class DataBundle : ScriptableObject
	{
		/* ==========================
		 * > Properties
		 * -------------------------- */

		/// <summary>
		/// Name of the file to be saved
		/// </summary>
		public virtual string FileName => $"{name}.json";


		/* ==========================
		 * > Methods
		 * -------------------------- */

		#region	Save / Load

		/// <summary>
		/// Save this <see cref="DataBundle"/> to a file
		/// </summary>
		public abstract void Save(string filePath);

		/// <summary>
		/// Load this <see cref="DataBundle"/> from a file
		/// </summary>
		public abstract void Load(string filePath);

		#endregion
	}

	/// <summary>
	/// Contains a list of <see cref="DataBlock"/> and saves it to a file
	/// <typeparam name="TSerializedType">Data once serialized</typeparam>
	/// </summary>
	public abstract class DataBundle<TSerializedType> : DataBundle
	{
		/* ==========================
		 * > Data Structures
		 * -------------------------- */

		/// <summary>
		/// Link between a <see cref="TSerializedType"/> and an id (<see cref="string"/>)
		/// </summary>
		[Serializable]
		protected class DataBlock
		{
			/* ==========================
			 * > Properties
			 * -------------------------- */

			/// <summary>
			/// ID for this <see cref="DataBlock"/>
			/// </summary>
			public string ID => _id;

			/// <summary>
			/// <see cref="TSerializedType"/> for this <see cref="DataBlock"/>
			/// </summary>
			public TSerializedType SerializedData => _serializedData;


			/* ==========================
			 * > Private Serialized Fields
			 * -------------------------- */

			[SerializeField]
			//[HideInInspector]
			private string _id;

			[SerializeField]
			//[HideInInspector]
			private TSerializedType _serializedData;


			/* ==========================
			 * > Constructors
			 * -------------------------- */

			public DataBlock(string id, TSerializedType serializedData)
			{
				_id = id;
				_serializedData = serializedData;
			}
		}


		/* ==========================
		 * > Private Serialized Fields
		 * -------------------------- */

		[SerializeField]
		protected List<DataBlock> _data = new();


		/* ==========================
		 * > Methods
		 * -------------------------- */

		#region Data Management

		/// <summary>
		/// Add/Update an entry in the <see cref="DataBlock"/> list
		/// </summary>
		/// <typeparam name="T">Type of data to be added</typeparam>
		/// <param name="id">ID of the data</param>
		/// <param name="data">Raw data</param>
		public void SetData<T>(string id, T data)
		{
			TSerializedType serializedData = SerializeData<T>(data);
			var newData = new DataBlock(id, serializedData);
			DataBlock existingData = GetDataByID(id, out var i);

			if (existingData == null)
			{
				_data.Add(newData);
			}
			else
			{
				_data[i] = newData;
			}
		}

		/// <summary>
		/// Find data by id
		/// </summary>
		/// <typeparam name="T">Type of data to be return</typeparam>
		/// <param name="id">ID of the data</param>
		/// <param name="wasDataFound">True if data was found</param>
		/// <returns>Data as <typeparamref name="T"/></returns>
		public T GetData<T>(string id, out bool wasDataFound)
		{
			DataBlock data = GetDataByID(id, out _);

			if (data != null)
			{
				T value = DeserializeData<T>(data.SerializedData);
				wasDataFound = true;
				return value;
			}

			wasDataFound = false;
			return default;
		}

		/// <summary>
		/// Remove data from this <see cref="DataBundle"/>
		/// </summary>
		/// <param name="id">ID of the <see cref="DataBlock"/> to be removed</param>
		/// <returns>True if data was removed</returns>
		public bool RemoveData(string id)
		{
			DataBlock data = GetDataByID(id, out _);

			// Remove data if it exists
			if (data != null)
			{
				_data.Remove(data);
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// Clear all the data
		/// </summary>
		public void ClearData()
		{
			_data.Clear();
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Serialize data into a <typeparamref name="TSerializedType"/>
		/// </summary>
		/// <typeparam name="T">Type to serialize</typeparam>
		/// <param name="deserializedData">Raw data to serialize</param>
		/// <returns>Serialized Data</returns>
		protected abstract TSerializedType SerializeData<T>(T deserializedData);

		/// <summary>
		/// Deserialize data from <typeparamref name="TSerializedType"/>
		/// </summary>
		/// <typeparam name="T">Type to deserialize</typeparam>
		/// <param name="serializedData"></param>
		/// <returns></returns>
		protected abstract T DeserializeData<T>(TSerializedType serializedData);

		#endregion

		#region Helpers

		/// <summary>
		/// Get a <see cref="DataBlock"/> by its id
		/// </summary>
		/// <param name="id">id to get the <see cref="DataBlock"/> from</param>
		/// <param name="index">Index of the found <see cref="DataBlock"/>. Will return -1 if nothing was found</param>
		/// <returns><see cref="DataBlock.Empty"/> if no <see cref="DataBlock"/> was found</returns>
		private DataBlock GetDataByID(string id, out int index)
		{
			for (var i = 0; i < _data.Count; i++)
			{
				DataBlock data = _data[i];
				if (data.ID == id)
				{
					index = i;
					return data;
				}
			}

			index = -1;
			return null;
		}

		#endregion
	}
}