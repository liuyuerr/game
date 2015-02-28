using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Utility
{
	public static void RecursiveSetLayer (GameObject obj, int layer)
	{
		Transform trans = obj.transform;
		for (int i = 0; i < trans.childCount; i++) {
			Transform childtrans = trans.GetChild (i);
			RecursiveSetLayer (childtrans.gameObject, layer);
		}
		obj.layer = layer;
	}	

	public static GameObject GetGameObjectByPath(GameObject root, string path){
		Transform tr = root.transform.Find(path);
		return tr ? tr.gameObject : null;
	}

	public GameObject CreateGameObject (string strName, GameObject objParent)
	{
		GameObject obj = new GameObject (strName);
		if (obj != null && objParent != null) {
			obj.transform.parent = objParent.transform;
		}
		return obj;
	}
	
	public static T CreateGameObjectComponent<T> (string objName, GameObject objParent) where T : MonoBehaviour
	{
		GameObject obj = new GameObject (objName);
		T tClass = obj.AddComponent<T> ();
		if (objParent != null) {
			obj.transform.parent = objParent.transform;
		}
		return tClass;
	}

	public static void DictModify (Dictionary<string, object> dict, string s, object b)
	{
		if (dict.ContainsKey (s)) {
			dict [s] = b;
		} else {
			dict.Add (s, b);
		}
	}

	public static float GetRandomValue(float min, float max)
	{
		float range = max - min;
		return UnityEngine.Random.value * range + min;
	}

	public static bool IsInteger(Type type)
	{         
		TypeCode value = Type.GetTypeCode(type);
		return (value == TypeCode.SByte || value == TypeCode.Int16 || value == TypeCode.Int32
		        || value == TypeCode.Int64 || value == TypeCode.Byte || value == TypeCode.UInt16 
		        || value == TypeCode.UInt32 || value == TypeCode.UInt64 ); 
	}
	
	public static bool IsFloat(Type type) 
	{       
		TypeCode value = Type.GetTypeCode(type);
		return (value == TypeCode.Single | value == TypeCode.Double | value == TypeCode.Decimal);
	}

	public static object ChangeType (object value, Type conversionType)
	{
		//没有什么好的方法可以快速转类型，
		//目前空字符串转数字类型会报错， 暂作放错处理， 以后优化.
		if(IsNumeric(conversionType) && value.GetType().Equals(typeof(string)) ){
			if(string.IsNullOrEmpty((string)value)) value = 0;
		}
		return Convert.ChangeType(value, conversionType);
	}

	public static bool IsNumeric( Type type )
	{
		if (type == null)
		{
			return false;
		}
		
		switch (Type.GetTypeCode(type))
		{
			case TypeCode.Byte:
			case TypeCode.Decimal:
			case TypeCode.Double:
			case TypeCode.Int16:
			case TypeCode.Int32:
			case TypeCode.Int64:
			case TypeCode.SByte:
			case TypeCode.Single:
			case TypeCode.UInt16:
			case TypeCode.UInt32:
			case TypeCode.UInt64:
				return true;
			case TypeCode.Object:
				if ( type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
				{
					return IsNumeric(Nullable.GetUnderlyingType(type));
				}
				return false;
		}
		return false;
	}
}