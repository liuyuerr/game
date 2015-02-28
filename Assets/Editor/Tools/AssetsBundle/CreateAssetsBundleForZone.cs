using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class CreateAssetsBundleForZone
{
	//场景打包
	//按章节，模块打包。 同一目录下开启依赖打包
	//保存目录 StreamingAssets/res/zone/
	//命名: z_{name}
	public static void Execute(BuildTarget bt) {
		BuildAssetBundleOptions buildOp = 
		BuildAssetBundleOptions.CompleteAssets |  
		BuildAssetBundleOptions.CollectDependencies | 
		BuildAssetBundleOptions.DeterministicAssetBundle;
		//common

		string path = Application.dataPath + "/Resources/Assets/Zone/";
		string copyPath = path + "Copy/";
		string savePath = Application.streamingAssetsPath + "/res/zone/{0}.assetbundle";

		//打包副本章节
		//遍历副本章节目录 Zone/Copy/
		string[] zones = Directory.GetDirectories(copyPath, "*.*", SearchOption.TopDirectoryOnly);
		foreach (string zone in zones) {
			DirectoryInfo dinfo = new DirectoryInfo(zone);
			//章节依赖入栈
			BuildPipeline.PushAssetDependencies();
			//打包章节公共包
			string zCommonPath = zone + "/Common";
			string zCommonName = string.Format(savePath, "z_"+dinfo.Name+"_common");
			if(PackageUtils.Menu(zCommonPath, zCommonName, buildOp, bt)){
				Debug.Log("场景公共包 " + dinfo.Name + "打包成功");
			}else{
				Debug.Log("场景公共包 " + dinfo.Name + "打包失败");
			}
			//打包章节的子包
			DirectoryInfo[] subInfos = dinfo.GetDirectories("*.*", SearchOption.TopDirectoryOnly);
			foreach (DirectoryInfo sub in subInfos) {
				if(sub.Name.Contains("Common")) continue;//排除公共目录
				string zName = "z_"+dinfo.Name+"_"+sub.Name;
				string zSavePath = string.Format(savePath, zName);
				if(PackageUtils.Menu(sub.FullName, zSavePath, buildOp, bt)){
					Debug.Log("场景 " + zName + "打包成功");
				}else{
					Debug.Log("场景 " + zName + "打包失败");
					break;
				}
			}
			//章节依赖出栈
			BuildPipeline.PopAssetDependencies();
		}

		AssetDatabase.Refresh();

	}
		
}


