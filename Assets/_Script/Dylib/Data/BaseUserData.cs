using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Dylib
{
	public interface iUserData
	{
		string uid {get;set;}
		string name {get;set;}
		int lv {get;set;}
		int sex {get;set;}
		int cash {get;set;}
		int gold {get;set;}
		int RegisterTime {get;set;}
		int LastLoginTime {get;set;}
	}
	
	public class BaseUserData : iUserData
	{
		private string _uid = "";
		private string _name = "";
		private int _lv = 0;
		private int _exp = 0;
		private int _sex = 0;
		private int _cash = 0;
		private int _gold = 0;
		
		private int _registerTime = 0;
		private int _lastLoginTime = 0;
		
		public string uid
		{
			get { return _uid; }
			set { _uid = value; }
		}
		
		public string name
		{
			get { return _name; }
			set { _name = value; }
		}
		public int lv
		{
			get { return _lv; }
			set { _lv = value; }
		}
		public int exp
		{
			get { return _exp; }
			set { _exp = value; }
		}
		public int sex
		{
			get { return _sex; }
			set { _sex = value; }
		}
		public int cash
		{
			get { return _cash; }
			set { _cash = value; }
		}
		public int gold
		{
			get { return _gold; }
			set { _gold = value; }
		}

		public int RegisterTime
		{
			get { return _registerTime; }
			set { _registerTime = value; }
		}
		public int LastLoginTime
		{
			get { return _lastLoginTime; }
			set { _lastLoginTime = value; }
		}
	}
}


