using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dylib.File;

public enum ResType
{
	NONE = 0,
	MODEL,
	UI,
	ZONE
}

public class ResMgr {
	protected static ResMgr _instance;
	public static ResMgr instance{
		get{
			return _instance;
		}
	}

	public delegate void ResLoadCompleteDelgate(string id, GameObject go);

	protected Dictionary<string, IFile> _common = new Dictionary<string, IFile>(); 
	protected Dictionary<string, IFile> _cache = new Dictionary<string, IFile>();

	public ResMgr(){
		_instance = this;
		Logger.Log("ResMgr is init .....");
	}

	//创建ABFile
	public IFile CreateAB(string locate, bool autoClean = false){
		IFile a = new ABFile(locate,autoClean);
		_cache.Add(locate, a);
		return a;
	}

	//根据类型，资源id生成locate.
	public string GetLocate(ResType type, string resId){
		string path = Setting.FILEPATH;
		if(type == ResType.UI){
			return path + "/res/ui/ui_all.assetbundle";
			//return string.Format(path + "/res/ui/{0}.assetbundle", ResConfig.GetObjectLocate(resId));
		}else if(type == ResType.MODEL){
			return string.Format(path + "/res/model/m_{0}.assetbundle", resId);
		}else if(type == ResType.ZONE){
			return string.Format(path + "/res/zone/z_{0}.assetbundle", resId);
		}else{
			return path + "/" + resId + ".assetbundle";
		}
	}

	//预加载资源
	//type:资源类型
	//resId:资源id
	public void PreLoadRes(ResType type, string resId, FileReadyDelegate delgate){
		string locate = GetLocate(type, resId);
		FileReadyDelegate tmp = (IFile file) => {
			ScreenLog.Log("PreLoadRes", "预加载AB耗时:::" +  Logger.useTime);
			delgate.Invoke(file);
		};
		PreLoadAssetBundle(locate, tmp);
	}

	//预加载AssetBundle
	public void PreLoadAssetBundle(string locate, FileReadyDelegate delgate){
		IFile a;
		if(_cache.ContainsKey(locate)){
			a = _cache[locate];
		}else{
			a = CreateAB(locate);
		}
		a.GetContentWhenReady(delgate);
	}

	//同步加载对象
	//locate:已准备的AB  
	//resId:资源Id
	public UnityEngine.Object LoadObject(string locate, string resPath){
		if(_cache.ContainsKey(locate)){
			IFile a = _cache[locate];
			if(a != null && a.IsReady() && a is ABFile){
				ABFile file = a as ABFile;
				return file.GetObjectInAB(resPath);
			}else{
				Debug.LogError("can't load " + resPath + " in " + locate);
			}
		}else{
			Debug.LogError("file "+ locate + " does not loaded" );
		}
		return null;
	}

	//异步加载对象
	//type:读取类型
	//resId:资源id
	//resPath:资源路径
	//delgate:完成执行委托
	public void LoadResAsync(ResType type, string resId, string resPath = "", ResLoadCompleteDelgate delgate = null){
		string locate = GetLocate(type, resId);
		IFile a;
		FileReadyDelegate frd = (IFile file)=>{
			ScreenLog.Log("LoadResAsync" , "异步加载AB耗时::: " + Logger.useTime);
			ABFile abfile = file as ABFile;
			GameObject o = abfile.GetObjectInAB(resPath) as GameObject;
			if(o != null){
				if(delgate != null) delgate.Invoke(resPath, o);
			}else{
				Debug.LogError("can't not find object ! resId: " + resId + "  resPath: "+ resPath + "\n" + abfile.GetUrl());
			}
		};
		if(_cache.ContainsKey(locate)){
			a = _cache[locate];
		}else{
			a = CreateAB(locate);
		}
		a.GetContentWhenReady(frd);
	}

	//加载公共依赖包
	public void LoadCommon(string path){
		if(!_common.ContainsKey(path)){
			FileReadyDelegate frd = (IFile file)=>{
				Debug.Log("common -> " + file.GetUrl() + " load all...");
				(file as ABFile).LoadAllAssets();
			};
			IFile a = CreateAB(path);
			a.GetContentWhenReady(frd);
			_common.Add(path, a);

		}
	}

	public void CleanAll(){
		foreach (KeyValuePair<string, IFile> v in _cache) {
			v.Value.Clear();
		}
		_cache.Clear();
	}

}
