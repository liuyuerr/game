using UnityEngine;
using System.Collections;
public class LogMgr : MonoBehaviour {
	
	private static LogMgr s_Instance = null;
	
	public static LogMgr instance
    {
        get{  return s_Instance;}
    }

    public static bool IsNull()
    {
        return (s_Instance == null);
    }

    void Awake()
    {
        s_Instance = this;
    }
	
	public void DebugLog(object message)
	{
        if (Setting.isDebug)
        {
            Debug.Log(message);
        }
	}
	
	// raily add
	public void DebugLog(Vector2 vec)
	{
		if (Setting.isDebug)
		{
			Debug.Log("Vector2: (" + vec.x + ", " + vec.y + ")");
		}
	}
	// public void DebugLog(Point pnt)
	// {
		// if (Build._DEBUG)
		// {
			// Debug.Log("Point: (" + pnt.X + ", " + pnt.Y + ")");
		// }
	// }
	public void DebugLog(Rect rec)
	{
		if (Setting.isDebug)
		{
			Debug.Log("Point: (" + rec.x + ", " + rec.y + ", " + rec.width + ", " + rec.height + ")");
		}
	}
}
