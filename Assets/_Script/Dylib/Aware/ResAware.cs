using UnityEngine;
using System;
using System.Collections;
using Dylib.File;


namespace Dylib
{
	public class ResAware {
		protected static ResAware _instance;
		public static ResAware instance{
			get{
				return _instance;
			}
		}

		public delegate void ResLoadCompleteDelgate(string id, GameObject go);


		public ResAware(){
			_instance = this;
		}

//		public void LoadRes(string resId, ResLoadCompleteDelgate delgate){
//			string uipath = GetUrlById(resId);
//			IFile a = new ABFile(resId, uipath);
//			FileReadyDelegate frd = (IFile file)=>{
//				ABFile abfile = file as ABFile;
//				GameObject o = abfile.GetObjectInAB(abfile.id);
//				if(o != null){
//					delgate.Invoke(abfile.id, o);
//				}else{
//					Debug.LogError("can't not find ui ! id:"+ abfile.id + "\n" + abfile.GetUrl());
//				}
//				abfile.Clear();
//			};
//			a.GetContentWhenReady(frd);
//		}

		//	public void AddScriptByCodeDll(string type, GameObject go){
		//		string dllpath = "file://" + Application.streamingAssetsPath + "/dll/code.assetbundle";
		//		IFile a = new ABFile("code", dllpath);
		//		FileReadyDelegate frd = (IFile file)=>{
		//			ABFile abfile = file as ABFile;
		//			Type t = abfile.GetScript(type);
		//			if(t!=null){
		//				go.AddComponent(t);
		//			}else{
		//				Debug.LogError("get Class error " + type);
		//			}
		//		};
		//		a.GetContentWhenReady(frd);
		//	}

		public string GetUrlById(string id){
			string ret = "";
			string locate = ResConfig.GetObjectLocate(id);
			string folder = ResConfig.GetObjectType(id);
			ret = "file://" + Application.streamingAssetsPath;
			if(!string.IsNullOrEmpty(folder)){
				string.Concat(ret, "/", folder, "/");
			}
			string.Concat(locate,".assetbundle");
			return ret;
		}

	}

}
