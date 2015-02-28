using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;

public class CreateAssetsBundle
{
	//打包选择内容
	public static void CreateAssetBunldesSelected (BuildTarget bt, string name)
	{
		//获取在Project视图中选择的所有游戏对象
		BuildAssetBundleOptions opts = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle;
		Object[] SelectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		string targetPath = Application.dataPath + "/StreamingAssets/" + name + ".assetbundle";
		if (BuildPipeline.BuildAssetBundle (Selection.activeObject, SelectedAsset, targetPath, opts, bt)) {
			Debug.Log(name +"资源打包成功");
		}else {
			Debug.Log(name +"资源打包失败");
		}

		//刷新编辑器
		AssetDatabase.Refresh ();
	}

}


