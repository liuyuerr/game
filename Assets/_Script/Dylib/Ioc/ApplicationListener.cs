using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public interface IApplicationListener {
		//int[] ListEventInterests();
		void OnApplicationEvent(ApplicationEvent e);
	}

	public class ApplicationListener : IApplicationListener{

//		public virtual int[] ListEventInterests() {
//			return new int[]{};
//		}

		public virtual void OnApplicationEvent(ApplicationEvent e){

		}

	}
}
