using Dylib;
using UnityEngine;
using System;
using System.IO;
using System.Collections;

public class DictMgr : MonoBehaviour {

	#region Instance
	// --------------------------------------------------------------
	// Instance
	// --------------------------------------------------------------
	private static DictMgr s_Instance = null;
	public static DictMgr instance
	{
		get { return s_Instance; }
	}
	
	public static bool IsNull()
	{
		return (s_Instance == null);
	}
	#endregion

	void Awake()
	{
		s_Instance = this;
	}

	#region CustonFunction
	public virtual void InitDicts(){
		//roleDict = initDictWithCsv ("Assets/csv/role", typeof(BaseDict), typeof(RoleInfo));
		//initDictWithPb("cs_role.dat",typeof(BaseDict), typeof(RoleInfo));
	}

	//init dict with csv Path. type of dict, type of info at dict
	public IDict initDictWithCsv(string path,Type dictType, Type infoType)
	{
//		TextAsset tmp = Resources.LoadAssetAtPath (path,typeof(TextAsset)) as TextAsset;
		TextAsset tmp = (TextAsset)ResourceMgr.instance.LoadResource(path);
		if (tmp) {
			return (IDict)DealWithFormatCsv(dictType, tmp.text, infoType);
		}else{
			LogMgr.instance.DebugLog(string.Format("dict load Error:{0}", path));
		}
		return null;
	}


	private object DealWithFormatCsv(Type dictType, string content, Type infoType){
		object[] param = {infoType,content.Trim()};
		return Activator.CreateInstance(dictType,param);
		//return new BaseDict(Type.GetType(infoType),content.Trim());
	}

	//ex:
	//"cs_role.dat", "role_config_rows" ,typeof(SanguoConfig.cs_role),typeof(BaseDict), typeof(RoleInfo)
//	private IDict initDictWithPb(string path,Type pbClass, string field, Type dictType, Type infoType)
//	{
//		IExtensible cfg = (IExtensible)Activator.CreateInstance(pbClass);
//		using (var file = File.OpenRead(Config.FILEPATH + "/cgi/" + path))
//		{
//			cfg = (IExtensible)RuntimeTypeModel.Default.Deserialize(file,null,pbClass);
//			object content = cfg.GetType().GetProperty(field).GetValue(cfg,null);
//			object[] param = {infoType,content};
//			return (IDict)Activator.CreateInstance(dictType,param);
//		}
//		return null;
//	}
	
	public IInfo GetInfoByDict(IDict dict, int key)
	{
		if (dict != null) {
			return (IInfo)dict.GetInfoByKey(key);
		}
		return null;
	}
	#endregion

	#region dicts
//	public IDict roleDict = null;
//	public IDict roleDict2 = null;
//	public IDict skillDict = null;
//	public IDict mapDict = null;
//	public IDict actionInfoDict = null;
//	public IDict animInfoDict = null;
	#endregion

}
