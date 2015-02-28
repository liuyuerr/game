using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dylib;

public class Game : MonoBehaviour {

	// Use this for initialization
	protected GamePlat _plat;
	bool _alloc = false;

	void Awake() {
		if(GamePlat.inst == null){
//			GameObject ui = new GameObject("UI");
//			DontDestroyOnLoad(ui);
			_plat = new GamePlat(gameObject);
			DontDestroyOnLoad(gameObject);
		}
	}

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!_alloc && _plat != null && _plat.IsAlloc()) {
			_alloc = true;
			InitGameConfig();
		}
	}

	void InitGameConfig() {
		string[] tmp = Setting.CONFIGS;
		for (int i = 0; i < tmp.Length; i++) {
			tmp[i] = "file:///" + Application.streamingAssetsPath + "/conf/" + tmp[i];
		}
		_plat.LoadConfigs(tmp);
	}

//	IEnumerator LoadConfig(string url, bool local = false) {
//		if(local) url = "file:///" + url;
//		WWW w = new WWW(url);
//		yield return w;
//		if (string.IsNullOrEmpty(w.error))  
//		{
//			XMLParser parser=new XMLParser();
//			XMLNode root = parser.Parse(w.text);
//			XMLNodeList a = root.GetNodeList("configs>0>object");
//			XMLNode node = a.Pop();
//			string id = node.GetValue("@id");
//			string cls = node.GetValue("@class");
//		} else {
//			Debug.Log(w.error);
//		}
//	}




}
