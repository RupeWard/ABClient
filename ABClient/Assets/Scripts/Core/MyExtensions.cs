using UnityEngine;
using System.Collections;

public static class MyExtensions
{
	/*
	static public string DebugDescribe<T>( this T t) where T : IDebugDescribable
	{
		System.Text.StringBuilder sb = new System.Text.StringBuilder ();
		t.DebugDescribe (sb);
		return sb.ToString ();
	}*/

	public static string GetPathInHierarchy( this Transform transform )
	{
		string path = transform.name;
		while (transform.parent != null)
		{
			transform = transform.parent;
			path = transform.name + "/" + path;
		}
		return path;
	}
}
