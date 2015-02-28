using UnityEngine;
using System.Collections;
using Dylib;
using Dylib.File;

namespace Dylib{
	public class GamePlat : ApplicationContext {

		public static GamePlat inst{
			get;
			private set;
		}
		
		public static IApplicationContext ac{
			get{
				return inst;
			}
		}
		
		public GamePlat(GameObject go):base(go){
			inst = this;
			//系统底层加载接口的实现
			Utility.CreateGameObjectComponent<FileLoader>("FileLoader", go);
			//Utility.CreateGameObjectComponent<ScriptMain>("ScriptMain", go);
		}
		
		public virtual bool IsAlloc() {
			return !FileLoader.IsNull();
		}

		
	}
}



