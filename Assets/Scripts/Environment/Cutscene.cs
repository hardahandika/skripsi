using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour {

	public GameObject enemy, target, player, overlay;
	float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 2.0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		currentTime = Time.time;
	}

	public void StartCutsceneEnemy(){
		Time.timeScale = 0.5f;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().target = enemy.transform;
	}
	public void StartCutsceneTarget(){
		Time.timeScale = 0.5f;
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().target = target.transform;
	}
	public void StopCutscene(){
		GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollow>().target = player.transform;
		Time.timeScale = 0f;
	}

	public void EnableOverlay(){
		overlay.SetActive(true);
	}
	public void DisableOverlay(){
		overlay.SetActive(false);
	}
	public void ToggleOverlay(){
		if(overlay.activeInHierarchy){
			overlay.SetActive(false);
		}
		else{
			overlay.SetActive(true);
		}
	}
}
