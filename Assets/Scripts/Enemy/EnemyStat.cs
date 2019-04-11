using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStat : MonoBehaviour {

	public Stat health;
	Image image;
	float delay = 5f;
	float regenRate = 10f;

	// Use this for initialization
	void Start () {
		health.MyMaxValue = 100;
		health.MyCurrentValue = health.MyMaxValue;

		image = GameObject.FindWithTag("Enemy Health Bar").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate(){
		//pergerakan berdasarkan stamina
		RegenHealth();
	
	}

	public void TakeDamage(float damage){
		health.MyCurrentValue -= damage;
	}

	void RegenHealth(){
		health.MyCurrentValue = Mathf.Clamp(health.MyCurrentValue + (regenRate * Time.deltaTime), 0, health.MyMaxValue);
	}

}
