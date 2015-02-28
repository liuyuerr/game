using UnityEngine;
using System.Collections;

public interface IObjectAware {
	object OnObjectBeforeInit(object o, string name);
	object OnObjectAfterInit(object o, string name);
}

public abstract class ObjectAware : IObjectAware {

	public abstract object OnObjectBeforeInit(object o, string name);
	public abstract object OnObjectAfterInit(object o, string name);
}
