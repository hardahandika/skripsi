using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStat : MonoBehaviour {

	public Stat health;
	public Stat stamina;
	public int machinegunAmmo, rifleAmmo, ammoMax;
	PlayerMove playerMove;
	Image image;
	public Text ammoText;
	public StageManager stageManager;
	float delay = 5f;
	float delayDash = 2f;
	float drainRate = 20f;
	float regenRate = 30f;
	public float dashDrain = 10f;
    private float healthRegenRate = 2f;

    // Use this for initialization
    void Start () {
		health.MyMaxValue = 100;
		health.MyCurrentValue = health.MyMaxValue;
		stamina.MyMaxValue = 100;
		stamina.MyCurrentValue = stamina.MyMaxValue;
		ammoMax = 0;
		machinegunAmmo = 30;
		rifleAmmo = 10;
		playerMove = GetComponent<PlayerMove>();
		image = GameObject.FindWithTag("Player Stamina Bar").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {

	}
	void FixedUpdate(){
		//pergerakan berdasarkan stamina
		if(stamina.MyCurrentValue == 0){	
			playerMove.isRunning = false;
			playerMove.isDashing = false;
			playerMove.isExhausted = true;
			image.color = new Color32(255,0,0,255);
			RegenStamina();
		}
		else if(playerMove.isExhausted){
			playerMove.sprintButton.enabled = false;
			playerMove.dashButton.enabled = false;
			RegenStamina();
			if(stamina.MyCurrentValue == stamina.MyMaxValue){
				image.color = new Color32(255,252,0,255);
				playerMove.isExhausted = false;
				playerMove.sprintButton.enabled = true;
				playerMove.dashButton.enabled = true;
			}
		}
		else{
			if(playerMove.isRunning){
				DrainStamina();
			}
			else if(playerMove.isDashing){
				DrainStaminaDash();
			}
			else{
				if(stamina.MyCurrentValue < stamina.MyMaxValue)
				RegenStamina();
				
			}
		}

		if (health.MyCurrentValue < health.MyMaxValue){
			RegenHealth();
		}

		if(health.MyCurrentValue < 1f){
			stageManager.isOutOfHealth = true;
			stageManager.GameOver();
		}
		
	}


	void DrainStamina(){
		stamina.MyCurrentValue = Mathf.Clamp(stamina.MyCurrentValue - (drainRate * Time.deltaTime), 0, stamina.MyMaxValue);

	}
	void DrainStaminaDash(){
		stamina.MyCurrentValue -= dashDrain;
	}
	void RegenStamina(){
		stamina.MyCurrentValue = Mathf.Clamp(stamina.MyCurrentValue + (regenRate * Time.deltaTime), 0, stamina.MyMaxValue);
	}
	void GetInput(){
		if(Input.GetKeyDown(KeyCode.Q)){
			health.MyCurrentValue -= 10;
		}
		else if(Input.GetKeyDown(KeyCode.E)){
			health.MyCurrentValue += 10;
		}
	}

	public void TakeDamage(float damage){
		health.MyCurrentValue -= damage;
	}

	void RegenHealth(){
		health.MyCurrentValue = Mathf.Clamp(health.MyCurrentValue + (healthRegenRate * Time.deltaTime), 0, health.MyMaxValue);
	}
	
}
