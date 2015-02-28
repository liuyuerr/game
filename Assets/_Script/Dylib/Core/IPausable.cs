using UnityEngine;
using System.Collections;

namespace Dylib {
	public interface IPausable {

		bool isPause{get;}
		void Pause();
		void Resume();

	}
}
