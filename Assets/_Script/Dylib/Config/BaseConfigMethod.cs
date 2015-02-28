using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public interface IConfigMethod {
		void Parse(object info = null);
		
		IConfig owner{get;set;}
		string nodeType{get;set;}
		//string id{get;set;}
		string name{get;set;}
		string parent{get;set;}
		//string refVal{get;set;}

		List<IConfigArgument> arguments{get;set;}

	}

	public class BaseConfigMethod : IConfigMethod {
		private IConfig _owner;
		private string _nodeType = ConfigNodeType.METHOD;
//		private string _id = string.Empty;
		private string _name = string.Empty;
		private string _parent = string.Empty;
//		private string _ref = string.Empty;

		protected List<IConfigArgument> _arguments;

		public BaseConfigMethod(object info = null, IConfig o = null){
			if(o!=null) owner = o;
			if(info!=null) Parse(info);
		}
		
		public virtual void Parse(object info = null){

		}

		protected virtual void UpdateArguments(object info = null){
			_arguments = new List<IConfigArgument>();
		}


		public IConfig owner{
			get{ return _owner;}
			set{ _owner = value;}
		}
		public string nodeType{
			get{ return _nodeType;}
			set{ _nodeType = value;}
		}
//		public string id{
//			get{ return _id;}
//			set{ _id = value;}
//		}
		public string name{
			get{ return _name;}
			set{ _name = value;}
		}
		public string parent{
			get{ return _parent;}
			set{ _parent = value;}
		}
//		public string refVal{
//			get{ return _ref;}
//			set{ _ref = value;}
//		}

		public List<IConfigArgument> arguments{
			get{
				if(_arguments == null){
					UpdateArguments();
				}
				return _arguments;
			}
			set{_arguments = value;}
		}



	}
}

