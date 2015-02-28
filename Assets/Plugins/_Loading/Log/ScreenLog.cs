using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//namespace Dylib
//{
	public class ScreenLog : MonoBehaviour {

		static List<string> messages = new List<string>();  
		static List<string> names = new List<string>();  

		public GUIStyle style = null;  
		public Rect rect;  

		public float IntervalSize = 16;  
		//绘制持续时间(秒)  
		public float ClearTime = 10;  
		float nowTime = 0;  

		void Start()  
		{   
		}  

		void Update()  
		{  
			if(nowTime < ClearTime)  
				nowTime+=Time.deltaTime;  
			else  
			{  
				messages.Clear();  
				names.Clear();  
				nowTime = 0;  
			}     
		}  

		void OnGUI()  
		{  
			Display();  
		}  

		void Display()  
		{  
			for(int i=0;i<names.Count;i++)  
			{  
				GUI.Box(new Rect(0,i*IntervalSize,rect.width,rect.height),  
					names[i] +" : "+messages[i],style);  
			}  

		}  

		public static void Log(string name, string message)  
		{  
			if(names.Contains(name) == false)  
			{  
				names.Add(name);  
				messages.Add(message);  
			}  
			else  
			{  
				for(int i=0;i<names.Count;i++)  
				{  
					if(names[i] == name)  
					{  
						messages[i] = message;  
						break;  
					}  
				}  

			}  
		}  

		public static void Log(string name, object mess)  
		{  
			string message = mess.ToString();  
			Log(name,message);  
		}  

		public static void Log(string name, bool mess)  
		{  
			string message;  

			if(mess == true)  
				message = mess.ToString()+"~~~~~~~";  
			else  
				message = mess.ToString()+".....";  

			Log(name,message);  
		}
	}

//}

