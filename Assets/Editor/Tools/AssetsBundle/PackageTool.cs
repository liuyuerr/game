using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class PackageTool :EditorWindow {

	BuildTarget _plat = EditorUserBuildSettings.activeBuildTarget;

	// Add menu named "My Window" to the Window menu
	[MenuItem ("Tools/Package")]
	static void Init () {
		// Get existing open window or if none, make a new one:
		PackageTool window = (PackageTool)EditorWindow.GetWindow (typeof (PackageTool));
	}

	void OnGUI () {
		GUILayout.Label ("资源打包", EditorStyles.boldLabel);
		_plat = (BuildTarget)EditorGUILayout.EnumPopup("选择发布平台: ", _plat);

		GUILayout.Space(3);
		GUILayout.Label("-------------[打包AssetBundle]-------------");
		GUILayout.Space(3);

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("打包场景",GUILayout.Width(70))){
			CreateAssetsBundleForZone.Execute(_plat);
		}
		if(GUILayout.Button("打包模型",GUILayout.Width(70))){
			CreateAssetsBundleForModel.Execute(_plat);
		}
		if(GUILayout.Button("打包UI",GUILayout.Width(90))){
			//CreateAssetsBundleForUI.Execute(_plat);
			CreateAssetsBundleForUI.ExecuteForAll(_plat);
		}
		if(GUILayout.Button("打包Assembly-CSharp",GUILayout.Width(150))){
			//CreateAssetsBundle.CreateAssetBunldesSelected(_plat,"/dll/Assembly-CSharp");
			CreateAssetsBundleForCode.Execute(_plat);
			Caching.CleanCache();
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(3);
		GUILayout.Label("-------------[发布设置]-------------");
		GUILayout.Space(3);

		GUILayout.BeginHorizontal();
		if(GUILayout.Button("发布IOS",GUILayout.Width(70))){

		}
		if(GUILayout.Button("发布Android",GUILayout.Width(90))){
			string path = Application.dataPath +"/"+"sg.apk";
			BuildPipeline.BuildPlayer(GetBuildScenes(), path, BuildTarget.Android, BuildOptions.None);
		}
		GUILayout.EndHorizontal();

		GUILayout.Space(3);
		GUILayout.Label("-------------[调试用]-------------");
		GUILayout.Space(3);

		GUILayout.BeginHorizontal();

		if(GUILayout.Button("打包选择内容",GUILayout.Width(90))){
			CreateAssetsBundle.CreateAssetBunldesSelected(_plat,"tmp");
		}
		if(GUILayout.Button("清缓存",GUILayout.Width(80))){
			Caching.CleanCache();
		}
		if(GUILayout.Button("查看assetbundle",GUILayout.Width(120))){
			DebugAssetBundle();
		}

		GUILayout.EndHorizontal();
	}

	static string[] GetBuildScenes()
	{
		List<string> names = new List<string>();
		foreach(EditorBuildSettingsScene e in EditorBuildSettings.scenes)
		{
			if(e==null)
				continue;
			if(e.enabled)
				names.Add(e.path);
		}
		return names.ToArray();
	}


	static void DebugAssetBundle(){
		Object[] SelectedAsset = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
		//Debug.Log(SelectedAsset.Length);
		foreach (Object item in SelectedAsset) {
			string path = AssetDatabase.GetAssetPath(item);

			if(path.Contains(".assetbundle")){
				path = PackageUtils.AssetUrlToFileUrl(path);
				byte[] x = File.ReadAllBytes(path);
				AssetBundle ab = AssetBundle.CreateFromMemoryImmediate(x);
				#if UNITY_5_0
				Debug.Log(item + "有{"+ab.AllAssetNames().Length+"}个内容:");
				foreach (string c in ab.AllAssetNames()) {
					Debug.Log(c);
				}
				#else
				Object[] tmp = ab.LoadAll();
				Debug.Log(item + "有{"+tmp.Length+"}个内容:");
				foreach (Object c in tmp) {
				Debug.Log(c);
				}
				#endif
				Debug.Log(item + " end------ ");
				ab.Unload(false);
				ab = null;
			}
		}

	}

}
