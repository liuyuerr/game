using UnityEngine;
using System;
using System.Collections;
using Dylib.File;


public class TestUI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ResMgr resMgr = new ResMgr();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	string resId = "1006";
	string modelPath = "assets/resources/assets/models/1006/prefabs/1006.prefab";
	string effectPath = "assets/resources/assets/models/1006/effect/51.prefab";

	string ui = "ui_login";
	string uiPath = "assets/resources/ui/prefab/ui_login.prefab";


	void OnGUI ()
	{
		if (GUI.Button (new Rect (5, 150, 120, 60), "test1")) {
			Caching.CleanCache();
		}

		if (GUI.Button (new Rect (155, 150, 120, 60), "resLoad")) {
			ResourceLoad();
		}

		if (GUI.Button (new Rect (5, 230, 120, 60), "common")) {
			string commonPath = Setting.FILEPATH + "/res/model/m_common.assetbundle";
			ResMgr.instance.LoadCommon(commonPath);
		}

		if (GUI.Button (new Rect (155, 230, 120, 60), "异步"+resId)) {
			//异步:
			Logger.Watch();
			ResMgr.instance.LoadResAsync(ResType.MODEL, resId, modelPath, onCreate);//<30ms
		}

		if (GUI.Button (new Rect (305, 230, 120, 60), "同步"+resId)) {
			//同步1:
			Logger.Watch();
			ResMgr.instance.PreLoadRes(ResType.MODEL, "1006", onLoadReady);
		}

		if (GUI.Button (new Rect (5, 300, 120, 60), "uicommon")) {
			string commonPath = Setting.FILEPATH + "/res/ui/ui_common.assetbundle";
			ResMgr.instance.LoadCommon(commonPath);
		}

		if (GUI.Button (new Rect (155, 300, 120, 60), "ui")) {
			Logger.Watch();
			addUI(ui);
		}

		GUI.Label(new Rect(500,10, 100, 30), Logger.useMemory);
	}

	//同步2:
	void onLoadReady(IFile file){
		GameObject go = (file as ABFile).GetObjectInAB(modelPath) as GameObject;
		ScreenLog.Log("1","同步 模型:" + Logger.useTime);
		GameObject effectGo = (file as ABFile).GetObjectInAB(effectPath) as GameObject;
		ScreenLog.Log("2","同步 特效:" + Logger.useTime);
		effectGo.transform.SetParent(go.transform);
	}

	//异步
	void onCreate(string id, GameObject go){
		ScreenLog.Log("1","异步 模型:" + Logger.useTime);
		ResMgr.instance.LoadResAsync(ResType.MODEL, resId, effectPath, onCreate2);//<10ms
	}

	void onCreate2(string id, GameObject go){
		ScreenLog.Log("2","异步 特效:" + Logger.useTime);
	}

	void addUI(string id){
		//todo:这个实现应该是放进UImgr里的
		//UImgr来读取UI配置 并绑定脚本

//		uiPath = 读取UI配置。。。
		string uiClass = "UILogin"; //读取UI配置

		ResMgr.ResLoadCompleteDelgate d = (string resId, GameObject go) =>{
			ScreenLog.Log("ui0","ui 加载:" + Logger.useTime);
			Type t = Type.GetType(uiClass);
			if(t != null){
				go.AddComponent(t);
			}else{
				Debug.LogError(string.Format("出错了 UI{0} 绑定的脚本{1} 不存在", go, uiClass));
			}
		};
		ResMgr.instance.LoadResAsync(ResType.UI, ui, uiPath, d);
	}


	void ResourceLoad(){
		Logger.Watch();
		GameObject go = Instantiate(Resources.Load("Assets/models/1006/prefabs/1006")) as GameObject;
		ScreenLog.Log("0","Resource: " + Logger.useTime);
	}




	//
	void AddGoByAb(){
		//		string commonPath = Application.streamingAssetsPath + "/res/model/common.assetbundle";
		//		AssetBundle common = AssetBundle.CreateFromFile(commonPath);
		//		Debug.Log(common);

		//testing...
		StartCoroutine(LoadAb());
	}

	IEnumerator LoadAb(){
		string path = Setting.FILEPATH + "/res/model/m_1006.assetbundle";
		WWW www = new WWW(path);
		//path = path.Replace("file:///", "").Replace("jar:file:///", "");

		ScreenLog.Log("0", path);
		yield return www;
		ScreenLog.Log("1", www.error);
		AssetBundle ab = www.assetBundle;
		ScreenLog.Log("2", "ab is ready " + (ab!=null).ToString() );
		ab.LoadAllAssets();
		ScreenLog.Log("3", "ab load all");
		//		Debug.Log(ab.LoadAsset("Assets/Resources/Assets/Models/1003/1003.prefab"));
		//		Debug.Log(ab.LoadAsset("1003"));
		//		Debug.Log(ab.LoadAsset("1003.prefab"));

		UnityEngine.Object o = ab.LoadAsset("Assets/Resources/Assets/Models/1006/Prefabs/1006.prefab");
		ScreenLog.Log("4", "go: - > " + o);
		GameObject go = Instantiate(o) as GameObject;
		ab.Unload(false);
		www.Dispose();
		www = null;
	}
}
