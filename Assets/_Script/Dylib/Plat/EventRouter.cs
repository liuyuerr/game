using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dylib {

	public interface IEventRouter {
		void AddEventRoute(int type, IEventHandler handler);
		void RemoveEventRoute(int type);
		bool HasEventRoute(int type);
	}

	public class EventRouter : ApplicationContextAware, IEventRouter, IEventHandler, IApplicationListener{
		protected Dictionary<int, List<IEventHandler>> _dict = new Dictionary<int, List<IEventHandler>>();


		public virtual void AddEventRoute(int type, IEventHandler handler){
			if(!HasEventRoute(type)){
				_dict.Add(type,new List<IEventHandler>());
			}
			_dict[type].Add(handler);
		}

		public virtual void RemoveEventRoute(int type){
			if(_dict.ContainsKey(type)){
				_dict.Remove(type);
			}
		}

		public virtual bool HasEventRoute(int type){
			return _dict.ContainsKey(type) && _dict[type].Count > 0;
		}

		public void OnApplicationEvent(ApplicationEvent e){
			OnEventHandler(e);
		}

		public void OnEventHandler(ApplicationEvent e){
			if(_dict.ContainsKey(e.type)){
				OnDetailEventHandler(_dict[e.type], e);
			}
		}

		public void OnDetailEventHandler(List<IEventHandler> list, ApplicationEvent e){
			for (int i = 0; i < list.Count; i++) {
				list[i].OnEventHandler(e);
			}
		}


	}
}

