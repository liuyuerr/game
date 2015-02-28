using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CreateAssetsBundleForUI
{
	//打包ui资源 所有预设都打包进一个包里。

	public static void ExecuteForAll(BuildTarget bt){
		BuildAssetBundleOptions buildOp = 
			BuildAssetBundleOptions.CompleteAssets |  
//			BuildAssetBundleOptions.CollectDependencies | 
			BuildAssetBundleOptions.DeterministicAssetBundle |
			BuildAssetBundleOptions.UncompressedAssetBundle;
		string path = Application.dataPath + "/Resources/UI/";
		string atlasPath = path + "Atlas/";
		string effectPath = path + "Effect/";
		string prefabPath = path + "Prefab/";

		string savePath = Application.streamingAssetsPath + "/res/ui/{0}.assetbundle";
		string commonAB = string.Format(savePath, "ui_common");
		string prefabAB = string.Format(savePath, "ui_all");
		BuildPipeline.PushAssetDependencies();
		if(PackageUtils.MultiMenu(new string[]{atlasPath, effectPath}, commonAB, buildOp, bt)){
			Debug.Log("UI公共资源打包成功");
		}else {
			Debug.Log("UI公共资源打包失败");
			return;
		}

		if(PackageUtils.Menu(prefabPath, prefabAB, "*.*", buildOp, bt)){
			Debug.Log("UI打包成功");
		}else{
			Debug.Log("UI打包失败");
		}
		BuildPipeline.PopAssetDependencies();

		AssetDatabase.Refresh();

	}





	//角色ui资源
	//按目录打包。 读取/conf/res.xml 根据配置id 打包进配置的locate里
	//保存目录 StreamingAssets/res/ui/
	//命名: ui_{name}
	public static void Execute (BuildTarget bt)
	{
		BuildAssetBundleOptions buildOp = 
			BuildAssetBundleOptions.CompleteAssets |  BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle;

		string xmlPath = Application.streamingAssetsPath + "/conf/res.xml";
		string uiPath = Application.dataPath + "/Resources/UI/Prefab/";
		string savePath = Application.streamingAssetsPath + "/res/ui/";
		//<locate, objs>
		Dictionary<string, List<Object>> locate = new Dictionary<string, List<Object>>();
		//<id,locate>
		Dictionary<string, string> cache = new Dictionary<string, string>();

		//读取xml配置 载入配置缓存locate字典
		XmlDocument xml = new XmlDocument();
		xml.Load(xmlPath);
		XmlNodeList objs = xml.SelectNodes("configs/object");
		foreach (XmlNode node in objs) {
			string uiLocate = node.Attributes["locate"].Value;
			if(!string.IsNullOrEmpty(uiLocate)){
				string uiName = node.Attributes["id"].Value;
				cache.Add(uiName,uiLocate);
			}
		}
		//遍历UI目录所有的预设
		string[] prefabs = System.IO.Directory.GetFiles (uiPath, "*.prefab", System.IO.SearchOption.AllDirectories);
		for (int i = 0; i < prefabs.Length; i++) {
			System.IO.FileInfo info = new System.IO.FileInfo(prefabs[i]);
			string fileId = info.Name.Replace(info.Extension,"");
			if(cache.ContainsKey(fileId)){
				string prefabLocate = PackageUtils.FileUrlToAssetUrl(info.FullName);//info.FullName.Replace(Application.dataPath,"Assets");
				Object obj = AssetDatabase.LoadMainAssetAtPath(prefabLocate);
				string locateKey = cache[fileId];
				if(!locate.ContainsKey(locateKey)){
					locate[locateKey] = new List<Object>();
				}
				List<Object> current = locate[locateKey];
				current.Add(obj);
			}
		}

		//公共依赖打包
		bool buildCommon = false;
		if(locate.ContainsKey("ui_common")){
			buildCommon = true;
			BuildPipeline.PushAssetDependencies();
			BuildList("ui_common", locate["ui_common"], savePath, buildOp, bt);
			locate.Remove("ui_common");
		}
		//打包其他AB
		foreach (KeyValuePair<string, List<Object>> item in locate) {
			BuildList(item.Key, item.Value, savePath, buildOp, bt);
		}
		//pop公共依赖打包
		if(buildCommon){
			BuildPipeline.PopAssetDependencies();
		}

		AssetDatabase.Refresh ();
	}


	public static void BuildList(string key, List<Object> list, string path, BuildAssetBundleOptions opts, BuildTarget bt){
		if(list.Count > 0){
			Debug.Log("正在打包" + key);
			for (int i = 0; i < list.Count; i++) {
				Debug.Log(i+": "+list[i]);
			}
			string filename = path + key + ".assetbundle";
//			if(System.IO.File.Exists(filename)){
//				System.IO.File.Delete(filename);
//			}
			if (BuildPipeline.BuildAssetBundle (null, list.ToArray(), filename, opts, bt)) {
				Debug.Log(key +"资源打包成功");
			} else {
				Debug.Log(key +"资源打包失败");
			}
		}else{
			Debug.LogError("some object locate error : " + key);
		}
	}



}


