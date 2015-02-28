using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public interface IInfo
	{
		String Key { get;}
		object GetFieldValue (string propertyName);
		//void SetFieldValue (string field, object val)
	}
	
	
	
	public class BaseInfo:BaseObject,IInfo{
		
		public int id = 0;
		public virtual String Key{
			get{return "id";}
		}

		public BaseInfo(object data = null)
		{
			if(data!=null) Update(data);
		}
		
		public override void Update(object data)
		{
			if (data is IList) {
				UpdateList ((IList)data);
			} else if(data is IDictionary){
				UpdateDict((IDictionary)data);
			} else{
				base.Update(data);
			}
		}
		void UpdateList(IList list)
		{
			
		}
		
		void UpdateDict(IDictionary data)
		{
			foreach (string key in data.Keys)
			{
				//Debug.Log(string.Format("key:{0} value:{1}",key,data[key]));
				if (this.HasField(key)){
					this.SetFieldValue(key,data[key]);
				}
			}
		}
		
		
		
		
	}
}

