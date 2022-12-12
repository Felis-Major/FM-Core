using System.Collections;
using UnityEngine;

namespace FM.Runtime.Core.DataManagement
{
	public class SaveSystemTest : MonoBehaviour
	{
		[ContextMenu("test")]
		private void Test()
		{
			DataManager.UserSlot = "new user";
			DataManager.ClearValues();
			DataManager.SetValue("global", "a", "global content");
			DataManager.SetValue("settings", "b", "settings content");
			DataManager.SetValue("dialogues", "c", "dialogues content");
			DataManager.SetValue("dialogues", "d", "a content");
			DataManager.SetValue("dialogues", "e", "aaa content");
			DataManager.SetValue("dialogues", "f", "aaaaa content");

			StartCoroutine(Coroutine());
		}

		private IEnumerator Coroutine()
		{
			yield return DataManager.DoSave();
			yield return DataManager.DoLoad();

			DataManager.GetValue("settings", "b", out string settings);
			Debug.Log(settings);

			DataManager.GetValue("dialogues", "c", out string dialogues);
			Debug.Log(dialogues);

			DataManager.GetValue("dialogues", "d", out string d);
			Debug.Log(d);

			DataManager.GetValue("dialogues", "e", out string e);
			Debug.Log(e);

			DataManager.GetValue("dialogues", "f", out string f);
			Debug.Log(f);
		}
	}
}