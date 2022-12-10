using UnityEngine;

namespace FM.Runtime.Core.DataManagement
{
	public class SaveSystemTest : MonoBehaviour
	{
		public int test;

		private void Awake()
		{
			DataManager.SetValue("1", test);
		}

		private void OnEnable()
		{
			DataManager.OnDataLoaded += OnDataLoaded;
		}

		private void OnDisable()
		{
			DataManager.OnDataLoaded -= OnDataLoaded;
		}

		private void OnDataLoaded()
		{
			DataManager.GetValue("1", out int _1);
			test = _1;
		}
	}
}
