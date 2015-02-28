using UnityEngine;
using System.Collections;

namespace Dylib {
	public class XMLConfig : BaseConfig {

		public XMLConfig(){}

		public XMLConfig(object info = null) : base (info) 
		{

		}

		public override void Parse (object info)
		{
			XMLParser parser=new XMLParser();
			XMLNode root = parser.Parse(info.ToString());
			XMLNodeList a = (XMLNodeList)root.GetNode("configs").GetNodeList("object");//.GetObject("configs>0>object");
			//XMLNode tmp = (XMLNode)a[0];
			foreach (XMLNode item in a) {
				if(item.name.Equals(ConfigNodeType.OBJECT)){
					XMLConfigNode node = new XMLConfigNode(item, this);
					AddConfigNode(node.id,node);
				}
			}
			base.Parse (info);


		}

	}


}

