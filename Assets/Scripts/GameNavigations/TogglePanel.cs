using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TogglePanel : MonoBehaviour {

	public GameObject[] something;
	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Time.timeScale = 0f;
	}

	public void EnableIt(){
		foreach(GameObject thing in something){
			thing.SetActive(true);
		}
	}
	public void DisableIt(){
		Time.timeScale = 1f;
		foreach(GameObject thing in something){
			thing.SetActive(false);
		}
	}

	public void ToggleTimeOn(){
		Time.timeScale = 1f;
		Debug.Log("waktu jalan");
	}

	public void ToggleTimeOff(){
		Time.timeScale = 0f;
	}
}
