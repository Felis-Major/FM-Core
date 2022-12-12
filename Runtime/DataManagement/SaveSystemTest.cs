using UnityEngine;

namespace FM.Runtime.Core.DataManagement
{
	public class SaveSystemTest : MonoBehaviour
	{
		[ContextMenu("test")]
		private void Test()
		{
			DataManager.Clear();
			DataManager.SetValue("global", "a", "global");
			DataManager.SetValue("settings", "b", "settings");
			DataManager.SetValue("dialogues", "c", "dialogues");
			DataManager.SetValue("dialogues", "d", "a");
			DataManager.SetValue("dialogues", "e", "aaa");
			DataManager.SetValue("dialogues", "f", "aaaaa");

			DataManager.Save();

			DataManager.Load();

			DataManager.GetValue("global", "a", out string global);
			Debug.Log(global);

			DataManager.GetValue("settings", "b", out string settings);
			Debug.Log(settings);

			DataManager.GetValue("dialogues", "c", out string dialogues);
			Debug.Log(dialogues);
		}
	}
}