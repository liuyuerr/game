using UnityEngine;
using System;
using System.Collections;

public class GameLoading : MonoBehaviour {

	// Use this for initialization
	void Start () {
		//GameObject go = new GameObject("Root");
		GameLoader gloader = gameObject.AddComponent<GameLoader>();
		gloader.Load("code", "Assembly-CSharp.assetbundle");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
