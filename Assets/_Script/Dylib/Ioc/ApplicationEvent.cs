using UnityEngine;
using System.Collections;

namespace Dylib
{
	public interface IApplicationEvent {

	}

	public class ApplicationEvent : IApplicationEvent{
		int _type;
		object _relatedObject;

		public int type{
			get{return _type;}
			private set{_type = value;}
		}

		public object relatedObject{
			get{return _relatedObject;}
			private set{_relatedObject = value;}
		}

		public ApplicationEvent(int t, object o = null) {
			_type = t;
			_relatedObject = o;
		}
	}

	public class NavigateEvent : ApplicationEvent{
		private string _navStatus;

		public string navStatus{
			get{return _navStatus;}
			private set{_navStatus = value;}
		}

		public NavigateEvent(string s, int t, object o):base(t,o){
			_navStatus = s;
		}
	}

}