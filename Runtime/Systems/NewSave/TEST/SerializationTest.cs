using System;
using System.Collections;
using System.Collections.Generic;
using FM.Runtime.Helpers.DataStructures;
using UnityEngine;

public class SerializationTest : MonoBehaviour
{
	[Serializable]
	private struct Test
	{
		public string name;
		public string description;
		public string type;
		public int price;

		public Test(string name, string description, string type, int price)
		{
			this.name = name;
			this.description = description;
			this.type = type;
			this.price = price;
		}
	}

	[Serializable]
	private struct DataWrapper
	{
		public string test;
	}

	[Serializable]
	public class Data
	{
		public List<object> _tests = new() { new Test("", "", "", 50000), new DataWrapper() { test = "555" } };
		public List<int> _aaa = new() { 1, 2, 3, 4, 5, 6 };
	}


	public NewSDitct<string, int> d = new();


	[ContextMenu("TEst")]
	public void Test2()
	{
		d.Add("50", 50);
		d.Add("aaaa", 50);
		d.Add("2222", 50);
		d.Add("4", 50);
	}


	// Start is called before the first frame update
	private void Start()
	{
		var json = JsonUtility.ToJson(new Data());
		Debug.Log(json);

		//var t = new Test("A", "B", "C", 5897);
		//SerializeObject(t);
		//Serialize<Test>(t);
	}

	private void SerializeObject(object data)
	{
		var json = JsonUtility.ToJson(data);
		Debug.Log(json);
		Test d = JsonUtility.FromJson<Test>(json);
		Debug.Log(d.price);
	}

	private void Serialize<T>(T data)
	{
		var json = JsonUtility.ToJson(data);
		Debug.Log(json);
		Test d = JsonUtility.FromJson<Test>(json);
		Debug.Log(d.price);
	}
}
