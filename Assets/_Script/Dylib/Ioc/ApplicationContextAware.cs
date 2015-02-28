using UnityEngine;
using System.Collections;

namespace Dylib
{
	public interface IApplicationContextAware
	{
		void SetApplicationContext (IApplicationContext ac);
	}


	public class ApplicationContextAware : IApplicationContextAware
	{
		protected IApplicationContext _ac;

		public virtual void SetApplicationContext (IApplicationContext ac)
		{
			_ac = ac;
		}
	}

}
