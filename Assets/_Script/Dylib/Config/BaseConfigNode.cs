using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Dylib
{
	public interface IConfigNode
	{
		void UpdataArguments(object info = null);
		void UpdateProperties(object info = null);
		void UpdateMethod(object info = null);

		List<IConfigArgument> arguments {get;set;}
		List<IConfigProperty> properties {get;set;}
		List<IConfigMethod> methods {get;set;}

		IConfig owner{get;set;}
		string id{get; set;}
		string parent{get; set;}
		string scope{get; set;}
		string locate{get; set;}
		string className{get; set;}
		bool isLazyInit{get;}
		string lazyInitStr{get; set;}
		bool isAbstract{get;}
		string abstractStr{get; set;}
		string initMethod{get; set;}
		string destroyMethod{get; set;}
	}
	
	
	public class BaseConfigNode : IConfigNode{
		private IConfig _owner;
		private IConfigNode _parentNode;
		private string _nodeType = ConfigNodeType.OBJECT;
		private string _scope = ConfigNodeType.PROPERTY;

		protected List<IConfigArgument> _arguments;
		protected List<IConfigProperty> _properties;
		protected List<IConfigMethod> _methods;

		private string _id = "";
		private string _parent = "";
		private string _locate = "";
		private string _className = "";
		private string _lazyInit = "";
		private string _abstract = "";
		private string _initMethod = "";
		private string _destroyMethod = "";

		public BaseConfigNode(object info = null, IConfig o = null) {
			if(o!=null) _owner = o;
			if(info!=null) parse(info);
		}

		public virtual void parse(object info) {

		}

		public virtual void UpdataArguments(object info = null) {
			_arguments = new List<IConfigArgument>();
			if(parentNode!=null){
				_arguments.AddRange(parentNode.arguments);
			}
		}

		public virtual void UpdateProperties(object info = null) {
			_properties = new List<IConfigProperty>();
			if(parentNode!=null){
				_properties.AddRange(parentNode.properties);
			}
		}

		public virtual void UpdateMethod(object info = null) {
			_methods = new List<IConfigMethod>();
			if(parentNode!=null){
				_methods.AddRange(parentNode.methods);
			}
		}

		public IConfig owner{
			get {return _owner;}
			set {
				_owner = value;

			}
		}
		public IConfigNode parentNode{
			get {return _parentNode;}
			set {
				if(value != null){
					_parentNode = value;
				}else if(_owner!=null && !string.IsNullOrEmpty(_parent)){
					_parentNode = _owner.GetConfig(_parent);
				}
			}
		}
		public string nodeType{
			get {return _nodeType;}
			set {_nodeType = value;}
		}
		public List<IConfigArgument> arguments{
			get {
				if(_arguments == null){
					UpdataArguments();
				}
				return _arguments;
			}
			set {_arguments = value;}
		}
		public List<IConfigProperty> properties{
			get {
				if(_properties == null){
					UpdateProperties();
				}
				return _properties;
			}
			set {_properties = value;}
		}
		public List<IConfigMethod> methods{
			get {
				if(_methods == null){
					UpdateMethod();
				}
				return _methods;
			}
			set {_methods = value;}
		}

		public string id{
			get {return _id;}
			set {_id = value;}
		}
		public string parent{
			get {return _parent;}
			set {
				_parent = value;
				parentNode = null;
			}
		}
		public string scope{
			get {return (string.IsNullOrEmpty(_scope) && parentNode != null) ? parentNode.scope : _scope;}
			set {_scope = value;}
		}
		public string locate{
			get {return (string.IsNullOrEmpty(_locate) && parentNode != null) ? parentNode.locate : _locate;}
			set {_locate = value;}
		}
		public string className{
			get {return (string.IsNullOrEmpty(_className) && parentNode != null) ? parentNode.className : _className;}
			set {_className = value;}
		}
		public bool isLazyInit{
			get {return _lazyInit.Equals("true");}
		}
		public string lazyInitStr{
			get {return (string.IsNullOrEmpty(_lazyInit) && parentNode != null) ? parentNode.lazyInitStr : _lazyInit;}
			set {_lazyInit = value;}
		}
		public bool isAbstract{
			get {return _abstract.Equals("true");}
		}
		public string abstractStr{
			get {return (string.IsNullOrEmpty(_abstract) && parentNode != null) ? parentNode.abstractStr : _abstract;}
			set {_abstract = value;}
		}
		public string initMethod{
			get{ return _initMethod;}
			set{ _initMethod = value;}
		}
		public string destroyMethod{
			get{ return _destroyMethod;}
			set{ _destroyMethod = value;}
		}
	}
}

