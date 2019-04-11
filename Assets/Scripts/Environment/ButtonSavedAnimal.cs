using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSavedAnimal : MonoBehaviour {

	public GameObject PanelSavedAnimal;
	
	public void ActivateSavedAnimalPanel(){
		PanelSavedAnimal.SetActive(true);
	}
	public void DeactivateSavedAnimalPanel(){
		PanelSavedAnimal.SetActive(false);
	}
	public void FillAnimalID(){
		GameObject.FindGameObjectWithTag("Animal Image").GetComponent<Image>().sprite = gameObject.GetComponent<AnimalID>().animalImage;
		GameObject.FindGameObjectWithTag("Animal Name").GetComponent<Text>().text = gameObject.GetComponent<AnimalID>().animalName;
		GameObject.FindGameObjectWithTag("Animal Description").GetComponent<Text>().text = gameObject.GetComponent<AnimalID>().animalDesc;
	}
}
