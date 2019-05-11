using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimalStat : MonoBehaviour {

	public Stat health;
	public Stat savemeter;
	public Image image;
	float drainRate = 13f;
	float regenRate = 10f;
	float regenSaveRate = 25f;
	public bool isSaved;

	// Use this for initialization
	void Start () {
		health.MyMaxValue = 100;
		health.MyCurrentValue = 0;
		savemeter.MyCurrentValue = 0;
		savemeter.MyMaxValue = 100;

	}
	
	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate(){
		if(savemeter.MyCurrentValue == savemeter.MyMaxValue){
			isSaved = true;
		}
	}

	public void DrainHealth(){
		
		health.MyCurrentValue = Mathf.Clamp(health.MyCurrentValue + (drainRate * Time.deltaTime), 0, health.MyMaxValue);
	}

	public void RegenHealth(){
		
		health.MyCurrentValue = Mathf.Clamp(health.MyCurrentValue - (regenRate * Time.deltaTime), 0, 0);
	}

	public void SavingTheAnimal(){

		savemeter.MyCurrentValue = Mathf.Clamp(savemeter.MyCurrentValue + (regenSaveRate * Time.deltaTime), 0, savemeter.MyMaxValue);
	}

	public void UnsaveTheAnimal(){
		
		savemeter.MyCurrentValue = Mathf.Clamp(savemeter.MyCurrentValue - (regenRate * Time.deltaTime), 0, 0);
	}
}
