using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class CreateAssetsBundleForCode
{
	//打包代码
	public static void Execute(BuildTarget bt){
		bool autoCopy = false;
		string path = Application.dataPath + "/Resources/Assembly-CSharp.bytes";
		Object code = AssetDatabase.LoadMainAssetAtPath(PackageUtils.FileUrlToAssetUrl(path));
		if(code == null){
			autoCopy = true;
			string projectPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf('/'));
			string orgPath = projectPath + "/Library/ScriptAssemblies/Assembly-CSharp.dll";
			System.IO.File.Copy(orgPath, path, true);
			AssetDatabase.Refresh();
			code = AssetDatabase.LoadMainAssetAtPath(PackageUtils.FileUrlToAssetUrl(path));
		}
		BuildAssetBundleOptions opts = BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle;
		string targetPath = Application.dataPath + "/StreamingAssets/dll/Assembly-CSharp.assetbundle";
		if (PackageUtils.Build(code, targetPath, opts, bt)) {
			Debug.Log("打包代码成功");
		}else{
			Debug.Log("打包代码失败");
		}
		if(autoCopy){
			System.IO.File.Delete(path);
		}
		AssetDatabase.Refresh();
		return ;
	}


}


