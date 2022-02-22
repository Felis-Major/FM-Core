using Daniell.Runtime.Helpers.GUIDSystem;
using Daniell.Runtime.Systems.Save;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveTest : MonoBehaviour, ISaveable
{
    public string GUID => _guid;

    [GUID]
    public string _guid;


    public int a;


    public void Load(SaveDataContainer saveDataContainer)
    {
        a = saveDataContainer.Get<int>("a");
    }

    public void Save(SaveDataContainer saveDataContainer)
    {
        saveDataContainer.Set("a", a);
    }
}
