using UnityEngine;
using System.Collections;

namespace Dylib {
	public class XMLConfigArgument :  BaseConfigArgument {
		
		public XMLConfigArgument(object info = null , IConfig o = null):base(info,o){
			
		}

		public override void Parse (object info = null)
		{
			XMLNode xml = (XMLNode)info;
			if(xml != null){
				nodeType = xml.name;
				name = xml.GetValue("@name");
				index = xml.GetValue("@index");
				type = xml.GetValue("@type");
				value = xml.GetValue("@value");
				refVal = xml.GetValue("@ref");
			}
		}

	}

}
