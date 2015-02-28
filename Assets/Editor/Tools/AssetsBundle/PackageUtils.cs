using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class PackageUtils : MonoBehaviour {

	public static string Format(string path){
		return path.Replace("\\", "/");
	}

	public static string FileUrlToAssetUrl(string path){
		return Format(path.Replace(Application.dataPath, "Assets"));
	}

	public static string AssetUrlToFileUrl(string path){
		return Format(Application.dataPath + path.Substring("Assets".Length));
	}

	public static bool Menu(string path, string savePath, BuildAssetBundleOptions opts, BuildTarget bt){
		return Menu(path,savePath, "*.*", opts, bt);
	}

	public static bool Menu(string path, string savePath, string f, BuildAssetBundleOptions opts, BuildTarget bt){
		List<Object> tmpList = ErgodicDirectory(path, f);
		return Build(tmpList.ToArray(),savePath, opts, bt);
	}

	public static bool MultiMenu(string[] paths, string savePath, BuildAssetBundleOptions opts, BuildTarget bt){
		return MultiMenu(paths, savePath, "*.*", opts, bt);
	}

	public static bool MultiMenu(string[] paths, string savePath, string f, BuildAssetBundleOptions opts, BuildTarget bt){
		List<Object> tmpList = new List<Object>();
		foreach (string path in paths) {
			List<Object> tmp = ErgodicDirectory(path, f);
			tmpList.AddRange(tmp);
		}
		return Build(tmpList.ToArray(),savePath, opts, bt);
	}

	public static List<Object> ErgodicDirectory(string path, string f="*.*", SearchOption opt = SearchOption.AllDirectories){
		List<Object> tmpList = new List<Object>();
		string[] a = Directory.GetFiles(path, f, SearchOption.AllDirectories);
		foreach (string item in a) {
			if(item.Contains(".meta") || item.Contains(".svn")) continue;
			string file = PackageUtils.FileUrlToAssetUrl(item);
			Object obj = AssetDatabase.LoadMainAssetAtPath(file);
			tmpList.Add(obj);
		}
		return tmpList;
	}

	//build for assets with names.
	public static bool Build(Object[] sth, string[] names, string path, BuildAssetBundleOptions opts, BuildTarget bt){
		return BuildPipeline.BuildAssetBundleExplicitAssetNames (sth, names, path, opts, bt);
	}

	//build for assets without names.
	public static bool Build(Object[] sth, string path, BuildAssetBundleOptions opts, BuildTarget bt){
		#if !UNITY_5_0
		return BuildPipeline.BuildAssetBundle(null, sth, path, opts, bt);
		#else
		List<string> name = new List<string>();
		foreach (Object o in sth) {
			name.Add(Format(AssetDatabase.GetAssetPath(o)));
		}
		return Build(sth, name.ToArray(), path, opts, bt);
		#endif
	}

	//build for mianAsset
	public static bool Build(Object main, string path, BuildAssetBundleOptions opts, BuildTarget bt){
		return BuildPipeline.BuildAssetBundle(main, null, path, opts, bt);
	}

}
