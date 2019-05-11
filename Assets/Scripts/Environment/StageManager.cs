using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {
	public GameObject panelGameOver;
	public Text gameOverText, gameOverElaborationText;
	GameObject[] enemy;
	public GameObject buttonAnimalPrefab, panelSavedAnimal, buttonPilihStage;
	public GameObject[] animals;

	int savedAnimalCount;
	public bool isOutOfTime, isOutOfHealth, enemyWiped, allAnimalSaved, allAnimalCaptured, someAnimalSaved;

    // Use this for initialization
    void Start () {
		animals = GameObject.FindGameObjectsWithTag("Target");
	}
	
	// Update is called once per frame
	void Update () {
		
		
		if(GameObject.FindGameObjectsWithTag("Enemy").Length == 0){
			enemyWiped = true;
		}
		
		if (GameObject.FindGameObjectsWithTag("Target").Length == 0 || isOutOfTime){
			if(savedAnimalCount == animals.Length){
				allAnimalSaved = true;
			}
			else if(savedAnimalCount == 0){
				allAnimalCaptured = true;
			}
			else{
				someAnimalSaved = true;
			}
			GameOver();
		}

	}

	public void GameOver(){
		FindObjectOfType<AudioManager>().StopPlaying("BGM In Game");
		FindObjectOfType<AudioManager>().PlaySound("BGM Result");
		Time.timeScale = 0f;
		gameOverText.text = SetGameOverMessage();

		foreach(GameObject animal in animals){
			if (animal.GetComponent<AnimalStat>().isSaved == true){
				savedAnimalCount++;
			}
		}

		gameOverElaborationText.text = "Satwa yang berhasil diselamatkan : " + (savedAnimalCount);
		panelGameOver.SetActive(true);
		if(isOutOfTime || isOutOfHealth){
			buttonPilihStage.SetActive(false);
		}

		if(enemyWiped || allAnimalSaved){buttonPilihStage.SetActive(true);}

		foreach (GameObject animal in animals){
			if (animal.GetComponent<AnimalStat>().isSaved == true){
				GameObject button = Instantiate(buttonAnimalPrefab);
				button.GetComponent<Image>().sprite = animal.GetComponent<AnimalID>().animalImage;
				button.GetComponent<AnimalID>().animalImage = animal.GetComponent<AnimalID>().animalImage;
				button.GetComponent<AnimalID>().animalName = animal.GetComponent<AnimalID>().animalName;
				button.GetComponent<AnimalID>().animalDesc = animal.GetComponent<AnimalID>().animalDesc;
				button.GetComponent<ButtonSavedAnimal>().PanelSavedAnimal = panelSavedAnimal;
				button.transform.SetParent(GameObject.FindGameObjectWithTag("Scroll View").transform);
				animal.GetComponent<AnimalStat>().isSaved = false;
			}
		}

	}

	string SetGameOverMessage()
	{
		if(isOutOfHealth){
			return "Anda dikalahkan oleh pemburu.";
		}
		else if(enemyWiped && allAnimalSaved){
			return "Anda berhasil mengalahkan semua pemburu dan menyelamatkan semua hewan.";
		}
		else if(allAnimalSaved){
			return "Anda berhasil menyelamatkan semua hewan.";
		}
		else if(allAnimalCaptured){
			return "Anda gagal menyelamatkan satu pun satwa.";
		}
		else if(someAnimalSaved){
			return "Anda berhasil menyelamatkan sebagian satwa di hutan.";
		}
		return null;
	}
}
