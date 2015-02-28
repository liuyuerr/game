using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Dylib.File;

public class ResConfig{

	public static Dictionary<string, XMLNode> _config = new Dictionary<string, XMLNode>();

	public static void Init(string url){
		IFile file = new TextFile(url);
		file.GetContentWhenReady(OnComplete);
	}

	public static void OnComplete(IFile f){
		_config.Clear();
		string c = (string)f.GetContent();
		XMLParser p = new XMLParser();
		XMLNode root = p.Parse(c).GetNode("configs");
		XMLNodeList a = root.GetNodeList("object");
		for(int i=0; i<a.Count; i++){
			XMLNode node = (XMLNode)a[i];
			string id = node.GetValue("@id");
			_config.Add(id, node);
		}
	}

	public static XMLNode GetObject(string id){
		return _config[id] ?? null ;
	}

	public static string GetObjectClass(string id){
		XMLNode node = GetObject(id);
		return node.GetValue("@class") as string;
	}

	public static string GetObjectLocate(string id){
		return GetObject(id).GetValue("@locate") as string;
	}

	public static string GetObjectType(string id){
		return GetObject(id).GetValue("@type") as string;
	}


}
