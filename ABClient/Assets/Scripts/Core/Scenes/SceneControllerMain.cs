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
	}

	#endregion SceneController_Base

}

