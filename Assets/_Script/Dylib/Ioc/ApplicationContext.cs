using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dylib.File;


namespace Dylib
{
	public interface IApplicationContext : IObjectFactory {
		void LoadConfigs(string[] configUrl, Type ConfigClass = null);
		void LoadConfig(string url);
		object GetObjectWhenReady(string id, ObjectReadyDelegate callback);
		void DispatchApplicationEvent(ApplicationEvent e);
	}

	public delegate void ObjectReadyDelegate(object obj);

	public class ApplicationContext : ObjectFactory,IApplicationContext {

		readonly Type DEFAULT_CONFIG_CLASS = typeof(XMLConfig);
		protected Type _configClass = null;
		protected int _configLoad = 0;
		protected int _configTotal = 0;

		protected List<IApplicationListener> _listener = new List<IApplicationListener>();

		public ApplicationContext(GameObject root){

		}

		public bool CanGetObject(string id){
			if(!HasObject(id)){
				IConfigNode config = _config.GetConfig(id);
				return !config.isAbstract;
			}else{
				return true;
			}
		}


		public object GetObjectWhenReady(string id, ObjectReadyDelegate callback){
			object ret = null;
			if(CanGetObject(id)){
				ret = GetObject(id);
				if(ret!=null){
					callback(ret);
				}
			}else{
				PreloadObject(id);
			}
			return ret;
		}

		public void PreloadObject(string id){
			if(!CanGetObject(id)){
				
			}
		}

		public void LoadConfigs(string[] configUrl, Type ConfigClass = null) {
			//only one Config class in system. 
			_configClass = ConfigClass == null ? DEFAULT_CONFIG_CLASS : ConfigClass;
			_configLoad = 0;
			_configTotal = configUrl.Length;
			Logger.Watch();
			for (int i = 0; i < _configTotal; i++) {
				LoadConfig(configUrl[i]);
			}
		}

		public void LoadConfig(string url) {
			TextFile file = new TextFile(url);
			file.GetContentWhenReady(OnConfigFileLoadComplete);
		}

		public void OnConfigFileLoadComplete(IFile file) {
			if(GetConfig() == null){
				SetConfig((IConfig)DoClassConsturct(_configClass));
			}
			_configLoad++;
			object data = file.GetContent();
			if(data == null){
				Debug.LogError("file content is null. url: " + file.GetUrl() );
				return;
			}
			IConfig config = (IConfig)DoClassConsturct(_configClass, data);
			AppendConfig(config);
			if(_configLoad>=_configTotal){
				OnAllConfigFilesLoadComplete();
			}
		}

		public void OnAllConfigFilesLoadComplete(){
			Logger.Log("loading config using time: {0}", Logger.useTime);
			Logger.Watch();
			ParseConfig(GetConfig());
			Logger.Log("create config using time: {0}", Logger.useTime);
			DispatchApplicationEvent(new ApplicationEvent(EventDef.EVENT_CONFIG_INIT_COMPLETE));
		}

		public void DispatchApplicationEvent(ApplicationEvent e) {
			//todo: 根据移动端性能表现修改事件机制。 
			//目前是全局通知，如果不行需要改成观察者，订阅模式
			for (int i = 0; i < _listener.Count; i++) {
//				int[] eIds = _listener[i].ListEventInterests();
//				if(eIds.Length < 1 || Array.IndexOf(eIds,e.type) < 0){
//					continue;
//				}
				_listener[i].OnApplicationEvent(e);
			}
		}

		//为所有监听IApplicationContextAware的对象实现SetApplicationContext.
		protected override void DoObjectAfterInit (string id, IConfigNode config, object obj)
		{
			base.DoObjectAfterInit (id, config, obj);
			if(obj is IApplicationContextAware){
				//Logger.Log("set ac to obj:{0}", obj);
				(obj as IApplicationContextAware).SetApplicationContext(this);
			}
			if(obj is IApplicationListener){
				//Logger.Log("addListener at obj:{0}", obj);
				_listener.Add((IApplicationListener)obj);
			}
			if(obj is IFile){
				//Logger.Log("load Ifile at url:{0}", (obj as IFile).GetUrl());
				//(obj as IFile).GetContentWhenReady(null);
			}
		}


		
	}
}
