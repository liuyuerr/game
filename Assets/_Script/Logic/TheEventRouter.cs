using UnityEngine;
using System.Collections;
using Dylib;

public class TheEventRouter : EventRouter {

	public override void SetApplicationContext (IApplicationContext ac)
	{
		base.SetApplicationContext (ac);
		InitRouter();
	}

	protected void InitRouter(){

	}

	protected object GetHandler<T>() where T : new(){
		object o = new T();
		_ac.DealWithObject(o);
		return o;
	}

}
