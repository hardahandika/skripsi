using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public GameObject[] panels;
	public int index = 0;

	// Use this for initialization
	void Start () {
		Time.timeScale = 0f;
	}
	
	// Update is called once per frame
	void Update () {

		if (index >= panels.Length){
			Time.timeScale = 1f;
			foreach(GameObject panel in panels){
				panel.SetActive(false);
			}
		}
	}

	public void DisableTutorial(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");

		// membuat index lebih dari array supaya tutorial berhenti
		index = panels.Length;
	}

	public void NextPanel(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");
		index++;
		for (int i = 0; i < panels.Length; i++){
			panels[i].SetActive(false);
		}
		panels[index].SetActive(true);
	}
}
