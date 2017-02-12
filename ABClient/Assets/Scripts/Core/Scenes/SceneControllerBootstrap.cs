using UnityEngine;
using System.Collections;

public class SceneControllerBootstrap : SceneController_Base 
{
#region inspector hooks

	public UnityEngine.UI.Text versionText;
	public float delay = 2f;

#endregion inspector hooks

	private void MoveOn()
	{
		SceneManager.Instance.SwitchScene( SceneManager.EScene.Main);
	}

#region event handlers


#endregion event handlers

#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.Bootstrap;
	}

	override protected void PostStart()
	{
		Application.targetFrameRate = 60;

		StartCoroutine( CopyBundlesFromStreamingAssetsCR() );
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
			if (!string.IsNullOrEmpty( wwwFile.error))
			{
				Debug.LogError( "Failed to load bundle file: " + bundleFileStreaminAssetsPath +" with error \n"+wwwFile.error);
			}
			else
			{
				yield return StartCoroutine( SaveBundleCR( abName, wwwFile.bytes ));
			}
		}
		yield return null;
		MoveOn( );
	}

	IEnumerator SaveBundleCR( string abName, byte[] bytes)
	{
		Debug.Log( "Saving bundle '" + abName + "' ("+bytes.Length+ " bytes)" );

		System.IO.DirectoryInfo bundleDirInfo = new System.IO.DirectoryInfo( AppManager.persistentBundlePath );
		if (!bundleDirInfo.Exists)
		{
			Debug.Log( "Creating persistent bundle dir " + AppManager.persistentBundlePath );
			bundleDirInfo.Create( );
			bundleDirInfo = new System.IO.DirectoryInfo( AppManager.persistentBundlePath );
			if (!bundleDirInfo.Exists)
			{
				Debug.Log( "Failed to create persistent bundle dir ");
				yield break;
			}
		}

		string saveBundleFilename = AppManager.persistentBundlePath + abName;

		System.IO.FileInfo bundleFileInfo = new System.IO.FileInfo( saveBundleFilename );
		if (bundleFileInfo.Exists)
		{
			Debug.LogWarning( "Bundle File " + saveBundleFilename + " exists, will overwrite" );
		}
		else
		{
			Debug.Log( "Creating File " + saveBundleFilename);
		}

		System.IO.File.WriteAllBytes( saveBundleFilename, bytes );
		Debug.Log( "Created File " + saveBundleFilename );

		yield return null;
	}

	#endregion bundlecopying

}
