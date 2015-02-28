using UnityEngine;
using System;
using System.Collections;
using UnityEngine.Events;
using Dylib.File;

namespace Dylib
{
	public class UIAware : ApplicationContextAware, IApplicationListener {

		public override void SetApplicationContext (IApplicationContext ac)
		{
			base.SetApplicationContext (ac);
		}

		public void OnApplicationEvent(ApplicationEvent e){
			if(e is NavigateEvent){
				tryDisplay((NavigateEvent)e);
			}
		}

		protected void tryDisplay(NavigateEvent e){
			string status = e.navStatus;
			//ResMgr.instance.LoadResAsync(ResType.UI, status, OnGameObjectCreate);

			//GameObject go = AddUI("UI/Prefab/UI_LOTTERY");
		}

		public void OnGameObjectCreate(string id, GameObject go){
			string reference = ResConfig.GetObjectClass(id);
			DealWithComponent(go, reference);
		}

		protected GameObject AddUI(string path){
			GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(path));
			return go;
		}

//		protected void DealWithScript(GameObject go, string status){
//			if(go != null){
//				Logger.Log("给对象{0} 绑定脚本{1}",go,status);
//				IScriptHolder holder = go.AddComponent<ScriptHolder>();
//				holder.AddScript(status);
//				//ScriptEnv.main.Run("UILottery", go);
//			}
//		}

		protected void DealWithComponent(GameObject go, string cls){
			if(go != null){
				Logger.Log("给对象{0} 绑定组件{1}",go,cls);
				Type c = Type.GetType(cls);
				if(c != null){
					go.AddComponent(c);
				}
			}
		}

//		protected void DealWithComponentTest(GameObject go, string cls){
//			if(go != null){
//				Logger.Log("给对象{0} 绑定组件{1}",go,cls);
//				ResMgr.instance.AddScriptByCodeDll(cls, go);
//			}
//		}

	}
}

