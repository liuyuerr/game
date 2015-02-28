using UnityEngine;
using System.Collections;

public class TestingAudio : MonoBehaviour {

	void Awake(){

	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(isMove){
			Camera.main.transform.position = new Vector3( Mathf.Sin(Time.time * 2) * 10,0,0);
		}
	}

	bool isPlay = false;
	bool isPause = false;
	bool isMove = false;

	void OnGUI(){
		if(GUI.Button(new Rect(0,0,80,40), "return")){
			Application.LoadLevel("Launch");
		}

		string btnLabel = isPlay ? isPause ? "resume" : "pause"  : "play";
		if(GUI.Button(new Rect(0,50,80,40), btnLabel)){
			if(!isPlay){
				AudioMgr.instance.PlayBgMusic("Halloween Audio Kit/Music/Halloween Rocks");
				isPlay = true;
			}else{
				if(!isPause){
					AudioMgr.instance.PauseBgMusic();
				}else{
					AudioMgr.instance.ResuseBgMusic();
				}
				isPause = !isPause; 
			}
		}

		if(GUI.Button(new Rect(0,100,80,40), "sound")){
			AudioMgr.instance.PlaySound("Halloween Audio Kit/FX/Evil Laughter 1");
		}

		if(GUI.Button(new Rect(100,100,80,40),"effect")){
			isMove = !isMove;
			if(isMove){
				AudioMgr.instance.PlaySoundAt(new Vector3(0,0,5),"Halloween Audio Kit/FX/Worms");
			}
			//GameObject.Find("aaa").GetComponent<AudioSource>().enabled = isMove;
		}

	}
}
