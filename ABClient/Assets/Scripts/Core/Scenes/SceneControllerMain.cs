using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class SceneControllerMain: SceneController_Base 
{

	#region SceneController_Base

	override public SceneManager.EScene Scene ()
	{
		return SceneManager.EScene.Main;
	}

	protected override void PostAwake( )
	{
	}

	protected override void PostStart( )
	{
		Debug.Log( "SceneMain: start" );

		foreach (GameObject go in AppManager.Instance.assetBundleGameObjects)
		{
			GameObject newGo = Instantiate<GameObject>( go );
			Debug.Log( "Instantiated " + go.name + " as " + newGo.transform.GetPathInHierarchy( ) );
		}
	}

	#endregion SceneController_Base

}

