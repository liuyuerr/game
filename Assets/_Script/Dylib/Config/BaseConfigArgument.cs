using UnityEngine;
using System.Collections;

namespace Dylib
{
	public interface IConfigArgument : IConfigProperty {
//		void Parse(object info = null);
//
//		IConfig owner{get;set;}
//		string nodeType{get;set;}
//		string name{get;set;}
		string index{get;set;}
//		string type{get;set;}
//		string value{get;set;}
//		string refVal{get;set;}
	}

	public class BaseConfigArgument : IConfigArgument {
		private IConfig _owner;
		private string _nodeType = ConfigNodeType.PROPERTY;
		private string _name = string.Empty;
		private string _index = string.Empty;
		private string _type = string.Empty;
		private string _value = string.Empty;
		private string _ref = string.Empty;

		public BaseConfigArgument(object info = null , IConfig o = null){
			if(o!=null) owner = o;
			if(info!=null) Parse(info);
		}

		public virtual void Parse(object info = null){

		}

		public IConfig owner{
			get{ return _owner;}
			set{ _owner = value;}
		}
		public string nodeType{
			get{ return _nodeType;}
			set{ _nodeType = value;}
		}
		public string name{
			get{ return _name;}
			set{ _name = value;}
		}
		public string index{
			get{ return _index;}
			set{ _index = value;}
		}
		public string type{
			get{ return _type;}
			set{ _type = value;}
		}
		public string value{
			get{ return _value;}
			set{ _value = value;}
		}
		public string refVal{
			get{ return _ref;}
			set{ _ref = value;}
		}
	}
}

