using UnityEngine;
using System.Collections;

namespace Dylib
{
	public class GamePlatAware : ApplicationContextAware, IObjectAware, IApplicationListener {

		public object OnObjectBeforeInit(object o, string name){
			return o;
		}

		public object OnObjectAfterInit(object o, string name){
			return o;
		}

		public override void SetApplicationContext(IApplicationContext ac){
			base.SetApplicationContext(ac);
			DoInitGameEnv();
		}

		public virtual void OnApplicationEvent(ApplicationEvent e){

		}

		public void UpdateStatus(string status){
			Debug.Log("update status to ---> " + status);
		}

		protected void DoInitGameEnv(){
			_ac.GetObject(GameIds.MODEL);
			_ac.GetObject(GameIds.UIAWARE);
			_ac.GetObject(GameIds.RESAWARE);

			_ac.DispatchApplicationEvent(new ApplicationEvent(EventDef.EVENT_GAME_READY));
		}

		
	}
}

