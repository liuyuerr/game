using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Dylib
{
	public interface IDict
	{
		int Count { get;}
		object GetInfoByKey(int key);
		void SetInfoByKey(int key, IInfo info);
		List<int> ListKeys();
		List<IInfo> ListInfo();
	}
	
	public class BaseDict:BaseObject,IDict{
		
		protected Dictionary<int, IInfo> _dict = new Dictionary<int, IInfo>();
		protected Type _infoType = null;
		public BaseDict(Type t)
		{
			_infoType = t;
		}
		public BaseDict(object data)
		{
			_infoType = typeof(BaseInfo);
			Update(data);
		}
		public BaseDict(Type t, object data)
		{
			_infoType = t;
			Update(data);
		}
		
		public virtual object GetInfoByKey(int key)
		{
			IInfo info;
			if(_dict.TryGetValue(key,out info)){
				return info;
			}
			return null;
		}
		
		public virtual void SetInfoByKey(int key, IInfo info)
		{
			_dict.Add(key,info);
		}
		
		public int Count{
			get{return _dict.Count;}
		}
		
		public List<int> ListKeys(){
			List<int> list = new List<int>();
			foreach (KeyValuePair<int, IInfo> pair in _dict) {
				list.Add(pair.Key);
			}
			return list;
		}
		
		public List<IInfo> ListInfo(){
			List<IInfo> list = new List<IInfo>();
			foreach (KeyValuePair<int, IInfo> pair in _dict) {
				list.Add(pair.Value);
			}
			return list;
		}
		
		public override void Update(object data)
		{
			_dict.Clear ();
			if (data is IList) {
				UpdateList((IList)data);
			}else if(data is String){
				UpdateCsv((String)data);
			}else{
				base.Update(data);
			}
		}
		
		void UpdateList(IList data)
		{
			for (int i = 0; i<data.Count; i++) {
				IInfo info = BuildInfo(data[i]);
				SetInfo(info);
			}
		}
		
		void UpdateCsv(String data)
		{					
			string [] tmp = data.Split (new char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);		//按行分割出数组
			string [] content;
			Dictionary<String, object> obj = new Dictionary<String,object>();				//构建临时数据对象
			string [] keys = tmp.GetValue(0).ToString().Split(","[0]);
			for (int i = 1; i<tmp.Length; i++) {
				obj.Clear ();
				content = tmp.GetValue(i).ToString().Split(","[0]);							//分割内容数组
				if(content.Length <= 0) break;
				for(int j=0;j<content.Length;j++){
					obj.Add(keys[j],content[j]);											//填充临时数据对象供info进行update
				}
				IInfo info = BuildInfo(obj);
				SetInfo(info);
			}
			obj = null;
		}
		
		protected IInfo BuildInfo(object o){
			return (IInfo)Activator.CreateInstance(_infoType,o);
		}
		
		protected void SetInfo(IInfo info)
		{
			int keyVal = int.Parse(info.GetFieldValue(info.Key).ToString());
			SetInfoByKey(keyVal, info);			//以info的主键存储在dict里
		}
		
		
		
	}
}

