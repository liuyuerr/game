using UnityEngine;
using System.Collections;

namespace Dylib
{
	public interface IState{

	}

	public class State : IPausable {
		protected bool _isPause = false;

		public virtual bool isPause{
			get{return _isPause;}
		}

		public virtual void Pause(){
			_isPause = true;
		}

		public virtual void Resume(){
			_isPause = false;
		}

	}
}

