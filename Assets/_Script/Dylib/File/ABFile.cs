using UnityEngine;
using System;
using System.Collections;
using System.Reflection;
using System.IO;

namespace Dylib.File {
	public class ABFile : WWWFile {
		protected AssetBundle _ab;
		protected Assembly _assembly;
		protected bool _autoUnload;

		protected UnityEngine.Object[] allAsset;

		public ABFile(object data = null, bool autoUnload = false) : base(data){
			_autoUnload = autoUnload;
		}

		public override void Clear (){
			_ab.Unload(false);
			_ab = null;
			allAsset = null;
			base.Clear ();
		}
		
		protected override void Parse() {
			_content = _www.assetBundle;
			_ab = _www.assetBundle;
		}

		public T GetComponent<T>() where T : MonoBehaviour{
			if (_ab != null && _ab.mainAsset != null)
			{
				GameObject asset = _ab.mainAsset as GameObject;
				if (asset != null) return asset.GetComponent<T>();
			}

			return null;
		}

		public UnityEngine.Object[] LoadAllAssets(){
			if (_ab != null && allAsset == null){
				#if UNITY_5_0
				allAsset = _ab.LoadAllAssets();
				#else
				allAsset = _ab.LoadAll();
				#endif
			}
			return allAsset;
		}
		
		public UnityEngine.Object GetObjectInAB(string obj){
			if(_ab != null){
				#if UNITY_5_0
				var sth = _ab.LoadAsset(obj);
				#else
				var sth = _ab.Load(obj);
				#endif
				if(sth != null){
					return UnityEngine.Object.Instantiate(sth);
				}
			}
			return null;
		}

		protected override object DoLoading() {
			string url = GetUrl();
			return _www = WWW.LoadFromCacheOrDownload(url, 1);
		}

		public override void OnReady ()
		{
			base.OnReady ();
			if(_autoUnload){
				Clear();
			}
		}

		public bool Save(string path) {
			Byte[] bytes = _www.bytes;
			Stream sw = null;
			string dir = path.Substring(0, path.LastIndexOf("/"));
			bool suc = false;
			try{
				DirectoryInfo dirInfo = new DirectoryInfo(dir);
				if (!dirInfo.Exists) {
					dirInfo.Create();
				}
				if (System.IO.File.Exists(path)){
					System.IO.File.Delete(path);
				}
				FileInfo t = new FileInfo(path);
				sw = new FileStream(t.FullName, FileMode.OpenOrCreate, FileAccess.Write);
				sw.Write(bytes, 0, bytes.Length);
				suc = true;
			}catch (IOException e){
				Debug.Log(e.Message);
			}finally{
				if (sw != null){
					sw.Close();
					sw.Dispose();
				}
			}
			return suc;
		}


	}
}


