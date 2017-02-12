using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AppManager : RJWard.Core.Singleton.SingletonApplicationLifetime< AppManager >
{
	public List<GameObject> assetBundleGameObjects = new List<GameObject>( );
	public bool AddAssetBundleGameObject(GameObject newGo)
	{
		foreach( GameObject go in assetBundleGameObjects)
		{
			if (go.name == newGo.name)
			{
				Debug.LogError( "AppManager is alreadty managing an assetbundle go called " + newGo.name );
				return false;
			}
		}
		assetBundleGameObjects.Add( newGo );
		Debug.Log( "AppManager is now managing an assetbundle go called " + newGo.name );
		return true;
	}

	static public List<string> abNames = new List<string>( )
	{
		"stage01"
	};

	private static string streamingAssetsPath
	{
		get
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return "jar:file://" + Application.dataPath + "!/assets";
			}
			else
			{
				return "file://" + Application.streamingAssetsPath;
			}
		}
	}

	public static string streamingAssetsDataPath
	{
		get
		{
			return streamingAssetsPath + "/Data/";
		}
	}

	public static string streamingAssetsBundlesPath
	{
		get
		{
			return streamingAssetsPath + "/Bundles/";
		}
	}

	public static string persistentBundlePath
	{
		get
		{
			return Application.persistentDataPath + "/Bundles/";
		}
	}

	public static string saveBundleFilename( string abName )
	{
		return AppManager.persistentBundlePath + abName;
	}

}

