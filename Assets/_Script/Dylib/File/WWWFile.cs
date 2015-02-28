using UnityEngine;
using System.Collections;

namespace Dylib.File
{
	public class WWWFile : File {
		protected WWW _www;
		public WWWFile(object data = null) : base(data){
			
		}
		
		public override void Clear (){
			_www.Dispose();
			_www = null;
			base.Clear ();
		}
		
		protected override void Preload(object data){
			if(data is string){
				_url = (string)data;
			}
		}

		public override IEnumerator Loading (){
			yield return DoLoading();
			if(_www.error != null){
				OnError();
			}else{
				Parse();
				OnReady();
			}
		}

		protected virtual object DoLoading() {
			string url = GetUrl();
			return _www = new WWW(url);
		}

		protected override void OnError() {
			Debug.LogError(_www.error);
			base.OnError();
		}

	}
}

