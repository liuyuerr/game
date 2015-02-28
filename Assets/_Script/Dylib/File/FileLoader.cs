using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Dylib.File{
	public class FileLoader : MonoBehaviour {

		#region Instance
		// --------------------------------------------------------------
		// Instance
		// --------------------------------------------------------------
		private static FileLoader s_Instance = null;
		public static FileLoader instance
		{
			get { return s_Instance; }
		}
		
		public static bool IsNull()
		{
			return (s_Instance == null);
		}
		#endregion

		protected Queue<IFile> _queueRequest = new Queue<IFile>();
		protected List<IFile> _loadingList = new List<IFile>();

		const int COROUTINE_LOAD_LIMIT = 5;		//max load Num
		// Use this for initialization
		void Awake () {
			s_Instance = this;
		}

		// Update is called once per frame
		void Update () {

			if(_loadingList.Count > 0){
	//			foreach(IFile file in _loadingList){
	//				if(file.IsReady()) _loadingList.Remove(file);
	//			}
				if(_loadingList.Count > 0){
					for (int i = _loadingList.Count - 1; i >= 0; i--) {
						if(_loadingList[i].IsReady() || _loadingList[i].IsFail()){
							_loadingList.RemoveAt(i);
						}
					}
				}
			}

			while (!IsQueueEmpty() && GetLoadingCount() < COROUTINE_LOAD_LIMIT)
			{
				SyncLoad(DequeueRequestFile());
			}
		}

		public bool IsQueueEmpty() {
			return _queueRequest.Count == 0;
		}

		public int GetLoadingCount(){
			return _loadingList.Count;
		}

		public IFile DequeueRequestFile() {
			return _queueRequest.Dequeue();
		}

		public void AsyncLoad(IFile file) {
			_queueRequest.Enqueue(file);
		}

		public void SyncLoad(IFile file) {
			_loadingList.Add(file);
			StartCoroutine(file.Loading());
		}

	}
}
