using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public interface IConfig
	{
		void Parse(object info);
		void InitConfigs();
		List<IConfigNode> ListConfigs();
		List<string> ListObjectIds();
		IConfigNode GetConfig(string id);
		void AddConfigNode(string id, IConfigNode a);
		void RemoveConfigNode(string id);
	}
	
	public class BaseConfig : IConfig {
		public string[] _subList = {"arguments","properties","methods"};
		public bool cacheRelateLocateIds = true;
		public bool cacheRelateObjectIds = true;
		public Dictionary<string, List<string>> _relateLocateCache = new Dictionary<string, List<string>>();
		public Dictionary<string, List<string>> _relateObjectCache = new Dictionary<string, List<string>>();

		protected Dictionary<string, IConfigNode> _config = new Dictionary<string, IConfigNode>();
		
		public BaseConfig(object info = null){
			if(info != null) Parse(info);
		}
		
		public virtual void Parse(object info) {
			
		}
		
		public void InitConfigs() {
			foreach (KeyValuePair<string, IConfigNode> item in _config) {
				item.Value.UpdataArguments();
				item.Value.UpdateProperties();
				item.Value.UpdateMethod();
			}
		}

		public void Refresh() {

		}
		
		public List<IConfigNode> ListConfigs() {
			List<IConfigNode> a = new List<IConfigNode>();
			foreach (KeyValuePair<string, IConfigNode> item in _config) {
				a.Add(item.Value);
			}
			return a;
		}
		
		public List<string> ListObjectIds() {
			List<string> a = new List<string>();
			foreach (KeyValuePair<string, IConfigNode> item in _config) {
				a.Add(item.Key);
			}
			return a;
		}
		
		public virtual IConfigNode GetConfig(string id) {
			return _config.ContainsKey(id) ? _config[id] : null;
		}
		
		public virtual void AddConfigNode(string id, IConfigNode a) {
			_config.Add(id, a);
		}
		
		public virtual void RemoveConfigNode(string id) {
			_config.Remove(id);
		}
		
		public virtual void RemoveAllNode() {
			foreach (KeyValuePair<string, IConfigNode> item in _config) {
				RemoveConfigNode(item.Key);
			}
		}

	}
}

