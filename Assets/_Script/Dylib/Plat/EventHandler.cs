using UnityEngine;
using System.Collections;

namespace Dylib
{
	public interface IEventHandler {
		void OnEventHandler(ApplicationEvent e);
	}

	public class EventHandler : ApplicationContextAware, IEventHandler{

		public virtual void OnEventHandler(ApplicationEvent e){

		}
	}

}
