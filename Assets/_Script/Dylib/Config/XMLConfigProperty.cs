using UnityEngine;
using System.Collections;

namespace Dylib
{
	public class XMLConfigProperty : BaseConfigProperty {
		
		public XMLConfigProperty(object info = null , IConfig o = null):base(info,o){
			
		}

		public override void Parse (object info)
		{
			XMLNode xml = (XMLNode)info;
			if(xml!=null){
				nodeType = xml.name;
				name = xml.GetValue("@name");
				type = xml.GetValue("@type");
				value = xml.GetValue("@value");
				refVal = xml.GetValue("@ref");
			}
		}
	}
}

