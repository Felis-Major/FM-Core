using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace FM.Runtime.Core.DataManagement
{
	public class SaveSystemTest : MonoBehaviour
	{
		public class TestClass
		{
			public string ohWow = "aaaaa";
			public int wwwww = 81;

			public override string ToString()
			{
				return ohWow.ToString() + " " + wwwww.ToString();
			}
		}

		[ContextMenu("test")]
		private void Test()
		{
			object[] sourceData = new object[] { "test", 54, new string[] { "hello", "222" }, new TestClass() };
			var types = new Type[] { typeof(string), typeof(int), typeof(string[]), typeof(TestClass) };

			string json = JsonConvert.SerializeObject(sourceData);
			print(json);

			object deserializedData = JsonConvert.DeserializeObject<object[]>(json);

			object[] array = deserializedData as object[];

			for (int i = 0; i < array.Length; i++)
			{
				object data = array[i];

				// If the data can be converted directly
				object convertedData = data switch
				{
					IConvertible => Convert.ChangeType(data, types[i]),
					JToken jToken => jToken.ToObject(types[i]),
					_ => default,
				};

				Debug.Log(convertedData.GetType() + " " + convertedData.ToString());
			}
		}
	}
}