using UnityEngine;
using System.Collections;

public class SceneControllerBootstrap : SceneController_Base
{
	#region inspector hooks

	public UnityEngine.UI.Text versionText;
	public float delay = 2f;

	#endregion inspector hooks

	private void MoveOn( )
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.Main );
	}

	#region event handlers


	#endregion event handlers

	#region SceneController_Base

	override public SceneManager.EScene Scene( )
	{
		return SceneManager.EScene.Bootstrap;
	}

	override protected void PostStart( )
	{
		SceneManager.CreateInstance( );

		Application.targetFrameRate = 60;

		StartCoroutine( CopyBundlesFromStreamingAssetsCR( ) );
	}

	#endregion SceneController_Base

	#region bundle copying


	IEnumerator CopyBundlesFromStreamingAssetsCR( )
	{
		foreach (string abName in AppManager.abNames)
		{
			Debug.Log( "Start Copying bundle '" + abName + "'" );

			string bundleFileStreaminAssetsPath = AppManager.streamingAssetsBundlesPath + abName;

			WWW wwwFile = new WWW( bundleFileStreaminAssetsPath );
			yield return wwwFile;
			if (!string.IsNullOrEmpty( wwwFile.error ))
			{
				Debug.LogError( "Failed to load bundle file: " + bundleFileStreaminAssetsPath + " with error \n" + wwwFile.error );
			}
			else
			{
				yield return StartCoroutine( SaveBundleCR( abName, wwwFile.bytes ) );
			}
		}
		yield return StartCoroutine(LoadAllBundles());
	}

	IEnumerator SaveBundleCR( string abName, byte[] bytes )
	{
		Debug.Log( "Saving bundle '" + abName + "' (" + bytes.Length + " bytes)" );

		System.IO.DirectoryInfo bundleDirInfo = new System.IO.DirectoryInfo( AppManager.persistentBundlePath );
		if (!bundleDirInfo.Exists)
		{
			Debug.Log( "Creating persistent bundle dir " + AppManager.persistentBundlePath );
			bundleDirInfo.Create( );
			bundleDirInfo = new System.IO.DirectoryInfo( AppManager.persistentBundlePath );
			if (!bundleDirInfo.Exists)
			{
				Debug.Log( "Failed to create persistent bundle dir " );
				yield break;
			}
		}

		string saveBundleFilename = AppManager.saveBundleFilename( abName );

		System.IO.FileInfo bundleFileInfo = new System.IO.FileInfo( saveBundleFilename );
		if (bundleFileInfo.Exists)
		{
			Debug.LogWarning( "Bundle File " + saveBundleFilename + " exists, will overwrite" );
		}
		else
		{
			Debug.Log( "Creating File " + saveBundleFilename );
		}

		System.IO.File.WriteAllBytes( saveBundleFilename, bytes );
		Debug.Log( "Created File " + saveBundleFilename );

		yield return null;
	}

	#endregion bundlecopying

	#region bundle loading

	System.Text.StringBuilder sb = new System.Text.StringBuilder( );

	IEnumerator LoadAllBundles( )
	{
		foreach (string abName in AppManager.abNames)
		{
			yield return StartCoroutine( LoadBundle( abName ) );
		}
		MoveOn( );
	}

	IEnumerator LoadBundle(string abName)
	{
		Debug.Log( "Start Loading bundle '" + abName + "'" );

		string saveBundleFilename = AppManager.saveBundleFilename( abName );

		System.IO.FileInfo bundleFileInfo = new System.IO.FileInfo( saveBundleFilename );
		if (!bundleFileInfo.Exists)
		{
			Debug.LogError( "Bundle file does not exist: " + saveBundleFilename );
			yield break;
		}
		Debug.Log( "Bundle file exists: " + saveBundleFilename );

		WWW www = new WWW( "file:///" +saveBundleFilename );
		yield return www;

		if (!string.IsNullOrEmpty(www.error))
		{
			Debug.LogError( "Failed to load bundle file " + saveBundleFilename +" with error \n"+www.error );
		}
		else
		{
			AssetBundle bundle = www.assetBundle;
			if (bundle == null)
			{
				Debug.LogError( "No bundle in file " + saveBundleFilename );
			}
			else
			{
				AssetBundleRequest request = bundle.LoadAllAssetsAsync< Object>( );
				yield return request;

				Object[] objects = request.allAssets;
				sb.Length = 0;
				sb.Append( "Loaded AB from "+saveBundleFilename+" with " + objects.Length + " objects:" );
				for (int i = 0; i < objects.Length; i++)
				{
					sb.Append( "\n " + objects[i].name + " - " + objects[i].GetType( ) );
				}

				Debug.Log( sb.ToString( ) );

				bundle.Unload( false );

			}
		}
		www.Dispose( );
		
		yield return null;
	}
	#endregion bundle loading

}
