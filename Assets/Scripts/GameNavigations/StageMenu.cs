using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageMenu : MonoBehaviour {

	public GameObject panelStage;
	
	public void EnablePanel(){
		panelStage.SetActive(true);
	}
	public void DisablePanel(){
		
		panelStage.SetActive(false);
	}
}
