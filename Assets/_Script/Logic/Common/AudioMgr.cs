using UnityEngine;
using System.Collections;

public class AudioMgr : MonoBehaviour {

	private static AudioMgr _intance = null;
	public static AudioMgr instance{
		get{return _intance;}
	}

	AudioSource bgMusic = null;

	// Use this for initialization
	void Awake () {
		_intance = this;

	}
	
	// Update is called once per frame
	void Update () {

	}

	AudioClip GetSourceByPath(string path){
		//todo: getClipFrom ResourceMgr.
		return Resources.Load(path) as AudioClip;
	}

	void AddBgSourceIfMiss(){
		if(bgMusic==null){
			//AudioSource source = Camera.main.gameObject.GetComponent<AudioSource>();
			//return source ?? Camera.main.gameObject.AddComponent<AudioSource>();
			bgMusic = Camera.main.gameObject.AddComponent<AudioSource>();
			bgMusic.loop = true;

		}
	}

	//播放背景音乐
	public void PlayBgMusic(string path, float volume = 0.5f){
		//if(bgMusic == null){
		AddBgSourceIfMiss();
		//}
		if(bgMusic.isPlaying){
			bgMusic.Stop();
		}
		bgMusic.clip = GetSourceByPath(path);
		bgMusic.volume = volume;
		bgMusic.Play();
	}

	public void PauseBgMusic(){
		if(bgMusic != null && bgMusic.isPlaying){
			bgMusic.Pause();
		}else{
			Debug.LogWarning("BgMusic is not playing..");
		}
	}

	public void ResuseBgMusic(){
		if(bgMusic!=null && !bgMusic.isPlaying){
			bgMusic.Play();
		}else{
			Debug.LogWarning("BgMusic is null or isPlaying");
		}

	}

	//播放一段循环的音乐 暂不考虑该需求
//	public void PlayMusic(){
//
//	}

	//播放一段声音 ex:dialog hit
	public void PlaySound(string path, float volume = 0.5f){
		AudioClip clip = GetSourceByPath(path);
		if(clip != null){
			//use this method can play one shot in bgMusic but can't set param for the Audio clip.
//			if(bgMusic != null){
//				bgMusic.PlayOneShot(clip);
//			}
			GameObject go = new GameObject("One shot audio");
			go.transform.parent = Camera.main.transform;
			go.transform.localPosition = Vector3.zero;
			AudioSource source = go.AddComponent<AudioSource>();
			source.clip = clip;
			source.volume = volume;
			source.Play();
			Destroy(go, clip.length);
		}else{
			Debug.LogError("can't find sound clip with path: " + path);
		}
	}

	//在全局某个位置 播放一段声音
	public void PlaySoundAt(Vector3 pos, string path, float volume = 0.5f){
		AudioClip clip = GetSourceByPath(path);
		if(clip != null){
			AudioSource.PlayClipAtPoint(clip, pos);
		}else{
			Debug.LogError("can't find sound clip with path: " + path);
		}
	}
}
