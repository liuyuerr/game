using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public class XMLConfigNode : BaseConfigNode {

		protected XMLNode _xml;

		public XMLConfigNode(object info = null, IConfig o = null):base(info,o) {

		}

		public override void parse (object info)
		{
			//base.parse (info);
			XMLNode xml = (XMLNode)info;
			if(xml != null){
				_xml = xml;
				id = xml.GetValue("@id");
				parent = xml.GetValue("@parent");
				scope = xml.GetValue("@scope");
				lazyInitStr = xml.GetValue("@lazy-init");
				abstractStr = xml.GetValue("@abstract");
				locate = xml.GetValue("@locate");
				className = xml.GetValue("@class");
				initMethod = xml.GetValue("@initMethod");
				destroyMethod = xml.GetValue("@destroyMethod");

				arguments = null;
				properties = null;
				methods = null;
			}
		}
		protected void ClearXml(){
			if(_xml != null && _arguments != null && _properties != null && _methods != null){
				_xml = null;
			}
		}

		public override void UpdataArguments (object info = null)
		{
			if(info == null) info = _xml;
			XMLNode xml = _xml;
			if(xml != null){
				_arguments = parentNode == null ? new List<IConfigArgument>() : parentNode.arguments;
				XMLNodeList xmlArgs = xml.GetNodeList(ConfigNodeType.CONSTRUCTOR_ARG);
				foreach(XMLNode xmlArg in xmlArgs){
					XMLConfigArgument tmp = new XMLConfigArgument(xmlArg, owner);
					if(string.IsNullOrEmpty(tmp.index)){
						//new constructor.
						_arguments.Add(tmp);
					}else if(parentNode != null){
						//override parent construct argument.
						int index;
						int.TryParse(tmp.index, out index);
						_arguments[index - 1] = tmp;
					}
				}
			}
			ClearXml();
		}

		public override void UpdateProperties (object info = null)
		{
			if(info == null) info = _xml;
			XMLNode xml = _xml;
			if(xml != null){
				_properties = parentNode == null ? new List<IConfigProperty>() : parentNode.properties;
				XMLNodeList xmlProperty = xml.GetNodeList(ConfigNodeType.PROPERTY);
				bool isOverride = false;
				foreach(XMLNode prop in xmlProperty){
					isOverride = false;
					XMLConfigProperty tmp = new XMLConfigProperty(prop, owner);
					if(parentNode != null){
						for (int i = 0; i < _properties.Count; i++) {
							if(_properties[i].name == tmp.name){
								isOverride = true;
								_properties[i] = tmp;
								break;
							}
						}
					}
					if(!isOverride){
						_properties.Add(tmp);
					}
				}
			}
			ClearXml();
		}

		public override void UpdateMethod (object info = null)
		{
			if(info == null) info = _xml;
			XMLNode xml = _xml;
			if(xml != null){
				_methods = parentNode == null ? new List<IConfigMethod>() : parentNode.methods;
				XMLNodeList xmlMethods = xml.GetNodeList(ConfigNodeType.METHOD);
				foreach(XMLNode xmlMethod in xmlMethods){
					XMLConfigMethod tmp = new XMLConfigMethod(xmlMethod, owner);
					_methods.Add(tmp);
				}
			}
			ClearXml();
		}

	}
}

