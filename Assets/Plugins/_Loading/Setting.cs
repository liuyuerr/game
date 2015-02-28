using UnityEngine;
using System.Collections;

public class Setting {
	public static int FPS = 60;
	public static string[] CONFIGS = {"base.xml"};

	public static bool isDebug
	{
		get{ return Application.isEditor; }
	}

	#if UNITY_EDITOR
	public static string FILEPATH = "file://" + Application.streamingAssetsPath;
	
	#elif UNITY_IPHONE
	public static string FILEPATH = Application.streamingAssetsPath;
	
	#elif UNITY_ANDROID
	public static string FILEPATH = Application.streamingAssetsPath;
 
	#endif

}
