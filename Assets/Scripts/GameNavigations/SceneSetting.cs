using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSetting : MonoBehaviour {

	public void ChangeTheScene(string sceneName){
		Time.timeScale = 1f;
		SceneManager.LoadScene(sceneName);	
	}
	
	public void ExitGame(){
		Application.Quit();
	}
}