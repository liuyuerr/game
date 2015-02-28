using UnityEngine;
using System;
using System.Collections;

public class GameLoader : MonoBehaviour{

	protected string _url;
	protected WWW _www;
	protected float _version;

	public void Load(string path, string fileName, float ver = 1){
		_url = Setting.FILEPATH + "/" + path + "/" + fileName;	
		_version = ver;
		StartCoroutine(OnLoading());	
	}

	protected IEnumerator OnLoading(){
		_www = new WWW(_url);
		yield return _www;
		if(_www.error != null){
			OnFail();
		}else{
			OnReady();
		}
	}

	protected void OnReady(){
		AssetBundle _ab = _www.assetBundle;
		#if UNITY_5_0
		TextAsset txt = _www.assetBundle.LoadAsset("Assembly-CSharp") as TextAsset;
		#else
		TextAsset txt = _www.assetBundle.Load("Assembly-CSharp") as TextAsset;
		#endif
		System.Reflection.Assembly assembly = System.Reflection.Assembly.Load(txt.bytes);
		Type t = assembly.GetType("Game");
		if(t!=null){
			gameObject.AddComponent(t);
		}else{
			Debug.LogError("can't find Game in Dll");
		}
	}

	protected void OnFail(){
		Debug.LogError("can't load game code..");
	}


}
