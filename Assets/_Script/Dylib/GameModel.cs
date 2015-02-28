using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public class GameModel:ApplicationContextAware{
		
		public Dictionary<int, iUserData> _userDict = new Dictionary<int, iUserData>();
		public iUserData myInfo;
		protected bool _vInit = false;

		public GameModel(){
			InitModel();
		}
		public iUserData GetMyInfo(){
			return myInfo;
		}
		
		public void SetUserInfo(int id, iUserData data){
			_userDict[id] = data;
		}
		
		public iUserData GetUserInfo(int id){
			iUserData tmp;
			return _userDict.TryGetValue(id,out tmp) ? tmp : null;
		}
		
		public virtual bool ready{
			get{return _vInit;}
		}
		
		public virtual void InitModel(){
			myInfo = _ac.GetObject(GameIds.USERDATA) as iUserData;
			_vInit = true;
		}
	}
}

