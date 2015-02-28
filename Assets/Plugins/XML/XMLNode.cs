using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class XMLNode: Hashtable
{
	public string text(){
		return (string)this["_text"];
	}

	/* new GetNode() and GetNodeList() method change to get object directly.
	 * get object by name param.
	 * ex: 
	 * <a>
	 * 	<b/>
	 *  <b/>
	 *  <c/>
	 * </a>
	 * GetNodeList("b")  GetNode("c");
	 * 
	 * old method read by path modify using GetObject instead.
	 * add GetNodes() and GetNodeLists() 
	 * modify by dylan
	 */

//	public XMLNodeList GetNodeList(string path)
//	{
//		return GetObject(path) as XMLNodeList;
//	}
//	
//	public XMLNode GetNode(string path)
//	{
//		return GetObject(path) as XMLNode;
//	}

	public XMLNodeList GetNodeList(string name){
		if(this[name] != null) return (XMLNodeList)this[name];
		return new XMLNodeList();
	}
	
	public XMLNode GetNode(string name, int index = 0){
		object o = this[name];
		if(o is ArrayList){
			return (XMLNode)(o as ArrayList)[0]; 
		} 
		return o as XMLNode;
	}

	//list all the children in node...
	public XMLNode[] ListNodes(){
		List<XMLNode> tmp = new List<XMLNode>();
		foreach (DictionaryEntry entry in this) {
			if(entry.Value is XMLNodeList){
				foreach (XMLNode item in (XMLNodeList)entry.Value) {
					tmp.Add(item);
				}
			}
		}
		//return tmp.Count > 0 ? tmp.ToArray() : null;
		return tmp.ToArray();
	}

	public string name{
		get{return (string)this["_name"];}
	}

	public string GetValue(string param){
		string ret = GetObject(param) as string;
		return string.IsNullOrEmpty(ret) ? string.Empty : ret;
	}

	protected object GetObject(string path)
	{
		string[] bits = path.Split('>');
		XMLNode currentNode = this;
		XMLNodeList currentNodeList = null;
		bool listMode = false;
		object ob;
		
		for (int i = 0; i < bits.Length; i++)
		{
			 if (listMode)
             {
                currentNode = (XMLNode)currentNodeList[int.Parse(bits[i])];
                ob = currentNode;
				listMode = false;
			 }
			 else
			 {
				ob = currentNode[bits[i]];

				if (ob is ArrayList)
				{
					currentNodeList = (XMLNodeList)(ob as ArrayList);
					listMode = true;
				}
				else
				{
					// reached a leaf node/attribute
					if (i != (bits.Length - 1))
					{
						// unexpected leaf node
						string actualPath = "";
						for (int j = 0; j <= i; j++)
						{
							actualPath = actualPath + ">" + bits[j];
						}
						
						//Debug.Log("xml path search truncated. Wanted: " + path + " got: " + actualPath);
					}
					
					return ob;
				}
			 }
		}
		
		if (listMode) 
			return currentNodeList;
		else 
			return currentNode;
	}
}