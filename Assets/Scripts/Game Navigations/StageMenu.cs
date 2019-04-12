using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMenu : MonoBehaviour {

	public GameObject panelStage;
	
	public void EnablePanel(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");
		panelStage.SetActive(true);
	}
	public void DisablePanel(){
		FindObjectOfType<AudioManager>().PlaySound("Button Click");
		panelStage.SetActive(false);
	}
}
