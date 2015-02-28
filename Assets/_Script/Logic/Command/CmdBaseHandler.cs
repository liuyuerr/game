using UnityEngine;
using System.Collections;
using Dylib;

public class CmdBaseHandler : EventHandler {
	protected TheGameModel _model;

	public override void SetApplicationContext (IApplicationContext ac)
	{
		base.SetApplicationContext (ac);
		_model = _ac.GetObject(GameIds.MODEL) as TheGameModel;
	}



}
