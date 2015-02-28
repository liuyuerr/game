using UnityEngine;
using System.Collections;

namespace Dylib.File{
	public interface IFile {
		IEnumerator Loading();
		bool IsReady();
		bool IsFail();
		string GetUrl();
		void Clear();
		void LoadSync();
		void Load();
		object GetContent();
		void GetContentWhenReady(FileReadyDelegate callBack, FileReadyDelegate fail = null);
	}
	
	public delegate void FileReadyDelegate(IFile file);
	
	public class File : IFile {
		protected bool _ready = false;
		protected bool _fail = false;
		protected object _content = null;
		protected string _url = string.Empty;
		protected FileReadyDelegate _onReady = null;
		protected FileReadyDelegate _onFail = null;

		public File(object data = null) {
			Preload(data);
			if(!string.IsNullOrEmpty(GetUrl())) {
				Load ();
			}
		}
		
		protected virtual void Preload(object data) {
			
		}
		
		public virtual void Clear(){
			
		}
		
		public virtual string GetUrl() {
			return _url;
		}
		
		public object GetContent() {
			return _content;
		}
		
		public void GetContentWhenReady(FileReadyDelegate callBack, FileReadyDelegate fail = null) {
			_onReady += callBack;
			if(fail!=null) _onFail += fail;
			if(IsReady()){
				DoLoadComplete();
			}
		}
		
		public void LoadSync (){
			DoLoad(false);
		}
		
		public void Load() {
			DoLoad();
		}
		
		protected void DoLoad(bool Async = true) {
			_ready = false;
			if(FileLoader.IsNull()){
				Debug.LogError("please init FileLoader before loading File.");
				return;
			}
			if(Async){
				FileLoader.instance.AsyncLoad(this);
			}else{
				FileLoader.instance.SyncLoad(this);
			}
		}
		
		public bool IsReady() {
			return _ready;
		}

		public bool IsFail() {
			return _fail;
		}
		
		public virtual IEnumerator Loading() {
			yield return null;
			Parse();
			OnReady();
		}


		protected virtual void OnError(){
			_fail = true;
			if(_onFail != null){
				//_onFail.Invoke(this);
				FileReadyDelegate tmp = _onFail;
				_onFail = null;
				tmp.Invoke(this);
			}
			_onReady = null;
		}

		protected virtual void Parse() {
			
		}

		public virtual void OnReady() {
			_ready = true;
			DoLoadComplete();
		}
		
		protected virtual void DoLoadComplete() {

			if(_onReady != null){
				//_onReady.Invoke(this);
				FileReadyDelegate tmp = _onReady;
				_onReady = null;
				tmp.Invoke(this);
			}
			_onFail = null;
		}
		
	}
}

