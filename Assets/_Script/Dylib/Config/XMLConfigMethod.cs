using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public class XMLConfigMethod : BaseConfigMethod {

		protected XMLNode _xml;

		public XMLConfigMethod(object info = null , IConfig o = null):base(info,o){
			
		}

		public override void Parse (object info)
		{
			XMLNode xml = (XMLNode)info;
			if(xml!=null){
				_xml = xml;
				nodeType = xml.name;
				name = xml.GetValue("@name");
				parent = xml.GetValue("@parent");
				//refVal = xml.GetValue("@ref");
				arguments = null;
			}
		}

		protected override void UpdateArguments (object info = null)
		{
			if(info == null) info = _xml;
			XMLNode xml = (XMLNode)info;
			if(xml != null){
				_arguments = new List<IConfigArgument>();
				XMLNodeList attrs = xml.GetNodeList(ConfigNodeType.METHOD_ARG);
				foreach (XMLNode argXml in attrs) {
					XMLConfigArgument tmp = new XMLConfigArgument(argXml);
					_arguments.Add(tmp);
				}
			}
		}

	}

}
