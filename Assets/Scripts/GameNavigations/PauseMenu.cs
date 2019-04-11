using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject PauseMenuUI, Notif;
	public void Pause(){
		Notif.SetActive(false);
		PauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}
	public void Resume(){
		Notif.SetActive(true);
		PauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	public void BackToMainMenu(){
		Resume();
		SceneManager.LoadScene("MainMenu");
	}

}
