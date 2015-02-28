using UnityEngine;
using System.Collections;

namespace Dylib
{
	public class ConfigNodeType {
		static public readonly string OBJECT = "object";
		static public readonly string CONSTRUCTOR_ARG = "constructor_arg";
		static public readonly string PROPERTY = "property";
		static public readonly string METHOD = "method";
		static public readonly string METHOD_ARG = "method_arg";
	}

	public class ConfigScope {
		static public readonly string STATIC = "static";
		static public readonly string SINGLETON = "singleton";
		static public readonly string PROTOTYPE = "prototype";
	}
}