using UnityEngine;
using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public interface IObjectFactory {
		Type GetClass(string name);
		Type GetObjectClass(string id);
		bool HasObject(string id);
		object GetObject(string id);

		object DealWithObject(object obj, string id = "", IConfigNode config = null);
	}

	public class ObjectFactory : IObjectFactory{

		protected IConfig _config;

		protected Dictionary<string, object> _objects = new Dictionary<string, object>();

		protected Dictionary<string, Type> _objectClasses = new Dictionary<string, Type>();

		protected Dictionary<string, IObjectAware> _iObjectAwares = new Dictionary<string, IObjectAware>();

		public ObjectFactory() {

		}

		#region config
		public void SetConfig(IConfig c) {
			_config = c;
		}

		public IConfig GetConfig() {
			return _config;
		}

		public void AppendConfig(IConfig tConfig) {
			foreach (IConfigNode node in tConfig.ListConfigs()) {
				_config.AddConfigNode(node.id, node);
			}
		}

		public virtual void ParseConfig(IConfig config) {
			IConfigNode[] tConfigNodes = config.ListConfigs().ToArray();
			DoConfigInitAwareObjects(tConfigNodes);
			DoConfigInitSingleton(tConfigNodes);
		}

		protected void DoConfigInitAwareObjects(IConfigNode[] a) {
			foreach(IConfigNode node in a){
				if(!string.IsNullOrEmpty(node.className) && !node.isAbstract){
					Type t = GetObjectClass(node.id);
					//Debug.Log(string.Format("id:{0} type:{1}, isIObjectAware:{2}", node.id,t,typeof(IObjectAware).IsAssignableFrom(t)));
					if(typeof(IObjectAware).IsAssignableFrom(t)){
						GetObject(node.id);
					}
				}
			}
		}

		protected void DoConfigInitSingleton(IConfigNode[] a) {
			foreach(IConfigNode node in a){
				//Logger.Log("id:{0} - parent:{1}, scope:{2}",node.id, node.parent, node.scope);
				if(node.scope == ConfigScope.STATIC){
					GetObject(node.id);
				}else if(node.scope == ConfigScope.SINGLETON && !node.isLazyInit && !node.isAbstract){
					GetObject(node.id);
				}
			}
		}
		#endregion

		#region public interface
		public Type GetClass(string name){
			return Type.GetType(name,true,true);
			//return null;
		}

		public Type GetObjectClass(string id){
			Type t;
			_objectClasses.TryGetValue(id, out t);
			if(t != null) return t;
			IConfigNode config = _config.GetConfig(id);
			if(config != null){
				t = GetClass(config.className);
				_objectClasses[id] = t;
				return t;
			}
			return null;
		}

		public bool HasObject(string id) {
			return _objects.ContainsKey(id);
		}

		public object GetObject(string id) {
			if(HasObject(id)) return _objects[id];
			return Create(id);
		}

		public object DealWithObject(object obj, string id = "", IConfigNode config = null){
			if(obj == null) return null;
			if(!string.IsNullOrEmpty(id)){
				DoObjectSaveAs(id, config, obj);
			}
			DoObjectBeforeInit(id, config, obj);
			DoObjectInitMethod(id, config, obj);
			DoObjectSetProperties(id, config, obj);
			DoObjectRunMethod(id, config, obj);
			DoObjectAfterInit(id, config, obj);
			return obj;
		}
		#endregion

		protected object Create(string id, IConfigNode config = null) {
			object obj = null;
			if(config == null){
				config = _config.GetConfig(id);
			}
			if(config != null){
				if(config.isAbstract){
					Debug.LogError("can't create intance on a abstract class - " + id);
				}else if(string.IsNullOrEmpty(config.className)){
					Debug.LogError("can't create intance with empty class name - " + id);
				}
				Type t = GetObjectClass(id);
				if(t == null){
					Debug.LogError("can't find the class at id - " + id);
				}
				//Logger.Watch();
				obj = DoCreate(id, config, t);
				//Logger.Log("{0} -  using time: {1}", id, Logger.useTime);
			}else{
				Debug.LogError("can't find object config - " + id);
			}
			DealWithObject(obj, id, config);
			return obj;
		}

		protected object DoCreate(string id, IConfigNode config, Type t){
			if(config.scope == ConfigScope.STATIC){
				return t;
			}else{
				if(config.arguments.Count > 0){
					object[] args = GetConstructArgs(id,config);
					return DoClassConsturct(t, args);
				}else{
					return DoClassConsturct(t, null);
				}

			}
		}

		protected object[] GetConstructArgs(string id, IConfigNode config){
			List<IConfigArgument> tArg = config.arguments;
			List<object> args = new List<object>();
			for (int i = 0; i < tArg.Count; i++) {
				args.Add(GetArgumentsValue(tArg[i]));
			}
			return args.ToArray();
		}

		protected object GetArgumentsValue(object obj){
			if(typeof(IConfigProperty).IsAssignableFrom(obj.GetType())){
				IConfigProperty config = obj as IConfigProperty;
				object tValue = null;
				if(!string.IsNullOrEmpty(config.refVal)){
					//value of reference
					tValue = GetObject(config.refVal);
				}else{
					//value of type.
					Type type = null;
					if(!string.IsNullOrEmpty(config.type)){
						type = GetClass(config.type);
					}
					if(!string.IsNullOrEmpty(config.value)){
						//default type set to string.
						type = type ?? typeof(string);
						//convert object to type
						tValue = Utility.ChangeType(config.value, type);
					}
				}
				return tValue;
			}
			return null;
		}

		protected object DoClassConsturct(Type t, object par){
			return DoClassConsturct(t, new object[]{par});
		}
		protected object DoClassConsturct(Type t, object[] pars = null){
			//return t.GetConstructor(new Type[]{}).Invoke(pars);
			return Activator.CreateInstance(t,pars);
		}

		protected void DoObjectSaveAs(string id, IConfigNode config, object obj){
			if(obj != null && config != null){
				if(config.scope != ConfigScope.PROTOTYPE){
					_objects[id] = obj;
				}
			}
			if(obj is IObjectAware){
				_iObjectAwares[id] = (IObjectAware)obj;
			}
		}

		protected void DoObjectInitMethod(string id, IConfigNode config, object obj){
			if(config !=null && obj != null ){
				Type t = obj.GetType();
				if(!string.IsNullOrEmpty(config.initMethod)){
					MethodInfo mInfo = t.GetMethod(config.initMethod);
					if(mInfo != null) mInfo.Invoke(obj, null);
				}
			}
		}

		protected void DoObjectSetProperties(string id, IConfigNode config, object obj){
			if(config !=null && obj != null ){
				List<IConfigProperty> properties = config.properties;
				Type t = obj.GetType();
				for (int i = 0; i < properties.Count; i++) {
					IConfigProperty prop = properties[i];
					object val = GetArgumentsValue(prop);
					PropertyInfo pInfo = t.GetProperty(prop.name);
					if(pInfo != null){
						pInfo.SetValue(obj, val, null);
					}
				}
			}
		}

		protected void DoObjectRunMethod(string id, IConfigNode config, object obj){
			if(config !=null && obj != null ){
				Type t = obj.GetType();
				List<IConfigMethod> methods = config.methods;
				for (int i = 0; i < methods.Count; i++) {
					IConfigMethod method = methods[i];
					MethodInfo mInfo = t.GetMethod(method.name);
					if(mInfo != null){
						//build object params from IconfigArguments. 
						List<IConfigArgument> methodArgs = method.arguments;
						object[] ps = null;
						if(methodArgs.Count>0){
							List<object> objs = new List<object>();
							for (int j = 0; j < methodArgs.Count; j++) {
								objs.Add(GetArgumentsValue(methodArgs[j]));
							}
							ps = objs.ToArray();
						}
						//run method with params
						mInfo.Invoke(obj, ps);
					}
				}
			}
		}

		protected virtual void DoObjectBeforeInit(string id, IConfigNode config, object obj){
			foreach (KeyValuePair<string, IObjectAware> item in _iObjectAwares) {
				item.Value.OnObjectBeforeInit(obj, id);
			}
		}

		protected virtual void DoObjectAfterInit(string id, IConfigNode config, object obj){
			foreach (KeyValuePair<string, IObjectAware> item in _iObjectAwares) {
				item.Value.OnObjectAfterInit(obj, id);
			}
		}
	}
}

