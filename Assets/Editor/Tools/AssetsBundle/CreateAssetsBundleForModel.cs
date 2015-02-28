using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class CreateAssetsBundleForModel
{
	//角色打包
	//按目录打包。 公共目录Controller,Shader打公共包
	//保存目录 StreamingAssets/res/model/
	//命名: m_{name}
	public static void Execute(BuildTarget bt) {
		BuildAssetBundleOptions buildOp = 
		BuildAssetBundleOptions.CompleteAssets |  BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle;
		//common
		BuildPipeline.PushAssetDependencies();

		string path = Application.dataPath + "/Resources/Assets/Models/";
		string controlPath = path + "Controller/";
		string shaderPath = path + "Shader/";
		string savePath = Application.streamingAssetsPath + "/res/model/{0}.assetbundle";
		string commonAB = string.Format(savePath, "m_common");

		if(PackageUtils.MultiMenu(new string[]{controlPath, shaderPath}, commonAB, buildOp, bt)){
			Debug.Log("角色公共资源打包成功");
			string md5 = MD5Helper.md5file(commonAB);
			Debug.Log(md5);
		}else {
			Debug.Log("角色公共资源打包失败");
			return;
		}

		string[] models = Directory.GetDirectories(path);
		foreach (var item in models) {
			DirectoryInfo info = new DirectoryInfo(item);
			int ret;
			int.TryParse(info.Name,out ret);
			if(ret>0){
				BuildPipeline.PushAssetDependencies();
				string fileName = string.Format(savePath, "m_"+info.Name);
				if(PackageUtils.Menu(item, fileName, buildOp, bt)){
					Debug.Log(info.Name + "打包成功");
				}else{
					Debug.Log("-----" + info.Name + "打包失败");
					break;
				}
				BuildPipeline.PopAssetDependencies();
			}
		}

		BuildPipeline.PopAssetDependencies();

		AssetDatabase.Refresh();

	}
		
}


