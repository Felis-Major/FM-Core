using FM.Runtime.Helpers.GUIDSystem;
using UnityEngine;

public class DatabaseItem
{
	public string GUID => _guid;

	[GUID]
	[SerializeField]
	private string _guid;
}