using UnityEngine;
using System;
using System.Reflection;
using System.Collections;

namespace Dylib
{
	public class BaseObject:object
	{
		private static BindingFlags flag = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
		public bool HasField (string fieldName)
		{
			return GetField(fieldName) != null;
		}
		
		public FieldInfo GetField(string fieldName)
		{
			return this.GetType ().GetField (fieldName,flag);
		}
		
		public Type GetFieldType (string fieldName)
		{
			return GetField(fieldName).FieldType;
		}
		
		public object GetFieldValue (string fieldName)
		{
			return GetField(fieldName).GetValue (this);
		}
		
		public void SetFieldValue (string field, object val)
		{
			//Type Ts = this.GetType ();
			FieldInfo fs = GetField(field);
			if(fs!=null){
				if (val.GetType () != fs.FieldType) {
					val = ChangeType (val, fs.FieldType);
					//NullableConverter converter = new NullableConverter(Ts.GetField(field).FieldType);
					//val = converter.ConvertFrom(val);
				}
				fs.SetValue (this, val);
			}
		}
		
		public virtual void Update (object data)
		{
			if (data is BaseObject) {
				BaseObject target = (BaseObject)data;
				FieldInfo[] fs = this.GetType ().GetFields(flag);
				//FieldInfo[] tfs = data.GetType().GetFields();
				foreach (FieldInfo f in fs) {
					if (target.HasField (f.Name) && target.GetFieldType (f.Name) == this.GetFieldType (f.Name)) {
						this.SetFieldValue (f.Name, target.GetFieldValue (f.Name));
					}
				}
			}else{
				foreach (var item in data.GetType().GetProperties()) {
					SetFieldValue(item.Name, item.GetValue(data,null));
				}
			}
		}
		
		public object ChangeType (object value, Type conversionType)
		{

			if (value.GetType ().Equals (typeof(string))) {
				if (Utility.IsNumeric (conversionType)) {
					if (string.IsNullOrEmpty ((string)value))
						value = 0;
				}
			}
			//nullable fix
			if (conversionType.IsGenericType &&
			    conversionType.GetGenericTypeDefinition ().Equals (typeof(Nullable<>))) {
				
				if (value == null)
					return null;
				
				System.ComponentModel.NullableConverter nullableConverter
					= new System.ComponentModel.NullableConverter (conversionType);
				
				conversionType = nullableConverter.UnderlyingType;
			}
			return Convert.ChangeType (value, conversionType);
		}
		
		
	}
}

