using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour {

	public Sound[] sounds;
	public static AudioManager instance;
	
	void Awake () {
		if(instance == null){
			instance = this;
		}
		else{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);

		foreach (Sound s in sounds){
			s.source = gameObject.AddComponent<AudioSource>();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
		}
	}

	void Update(){
		Scene currentScene = SceneManager.GetActiveScene();
		string sceneName = currentScene.name;

		switch(sceneName){
			case "MainMenu":
			StopPlaying("BGM In Game");
			StopPlaying("BGM Result");
			PlaySound("BGM Menu");
			break;

			case "PilihStage":
			StopPlaying("BGM In Game");
			StopPlaying("BGM Result");
			PlaySound("BGM Menu");
			break;

			case "StageAsiatis":
			StopPlaying("BGM Menu");
			PlaySound("BGM In Game");
			break;

			case "StagePeralihan":
			StopPlaying("BGM Menu");
			PlaySound("BGM In Game");
			break;

			case "StageAustralis":
			StopPlaying("BGM Menu");
			PlaySound("BGM In Game");
			break;
		}
	}
	
	public void PlaySound(string name)
	{
		Sound s = Array.Find(sounds, sound => sound.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		if(!s.source.isPlaying){
			s.source.Play();
		}
		
	}

	public void PlaySoundOneShot(string name)
	{
		Sound s = Array.Find(sounds, item => item.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.PlayOneShot(s.clip, 1f);
	}

	
	public void StopPlaying(string name)
	{
		Sound s = Array.Find(sounds, item => item.name == name);
		if (s == null)
		{
			Debug.LogWarning("Sound: " + name + " not found!");
			return;
		}
		s.source.Stop();
	}
}
