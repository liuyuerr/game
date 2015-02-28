using UnityEngine;
using System.Collections;

public class TestXml : MonoBehaviour {


	private string xmlStr = "<os>" + 
								"<obj id='x'>hahaha</object>" +
								"<obj id='a'/>" +
								"<obj id='b'/>" +
								"<obj id='c'/>" +
								"<obj id='d'/>" +
							"</os>";

	// Use this for initialization
	void Start () {
		XMLParser parse = new XMLParser();
		XMLNode root = parse.Parse(xmlStr);
		root = root.GetNode("os");
		Debug.Log(root.GetNodeList("obj").Count);
		Debug.Log(root.GetNode("obj").GetValue("@id"));
		Debug.Log(root.GetNode("obj").text());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
