using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleReader : MonoBehaviour
{
	public int test;
	public string test2;

	public JsonDataBundle bundle;

	private void Start()
	{
		bundle.SetData("testInt", test);
		bundle.SetData("testString", test2);
	}
}
