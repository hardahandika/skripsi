using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

	public static bool GameIsPaused = false;
	public GameObject PauseMenuUI, Notif;
	public void Pause(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");
		Notif.SetActive(false);
		PauseMenuUI.SetActive(true);
		Time.timeScale = 0f;
		GameIsPaused = true;
	}
	public void Resume(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");
		Notif.SetActive(true);
		PauseMenuUI.SetActive(false);
		Time.timeScale = 1f;
		GameIsPaused = false;
	}

	public void BackToMainMenu(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");
		Resume();
		SceneManager.LoadScene("MainMenu");
	}

}
