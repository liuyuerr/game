using UnityEngine;
using System.Collections;
using System.Diagnostics;

using Debug = UnityEngine.Debug;

//namespace Dylib
//{
	public class Logger {

		static protected Stopwatch watch = new Stopwatch();

		static public void Log(string s, params object[] p){
			Debug.Log(string.Format(s,p));
		}

		static public void Watch(){
			watch.Reset();
			watch.Start();
		}

		static public long useTime{
			get{
				return watch.ElapsedMilliseconds;
			}
		}

		static public string useMemory{
			get{
				return (Profiler.usedHeapSize / 1024 / 1024).ToString() + " mb";
			}
		}

	}

//}
