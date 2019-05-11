using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour {

	public Text notif;
	int animalsTotal = 4;
	PlayerStat playerStat;
	PlayerAttack playerAttack;
	private bool showText = false, someRandomCondition = true;
    private float currentTime = 0.0f, executedTime = 0.0f, timeToWait = 2.0f;

	void Start()
	{
		playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
		playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
		notif = GetComponent<Text>();
		notif.enabled = false;
	}
	void Update()
	{
		currentTime = Time.time;

         
		if(executedTime != 0.0f)
		{
			if(currentTime - executedTime > timeToWait)
			{
				executedTime = 0.0f;
				notif.enabled = false;
			}
		}
		//CheckAmmo();
		//CheckEnemy();

		if(GameObject.FindGameObjectsWithTag("Target").Length < animalsTotal){
			CountAnimal();
		}
		if(GameObject.FindGameObjectWithTag("Button Switch Weapon").GetComponent<ButtonEvents>().isButtonDown){
			ChangeWeapon();
		}
		
	}

	void CheckAmmo(){
		if(playerStat.machinegunAmmo == 0 && playerStat.rifleAmmo == 0){
			if(playerAttack.shootButton.isButtonDown){
				notif.text = "Amunisi habis, silakan isi kembali";
			}
			else{
				notif.text = "";
			}
		}
		else{
			notif.text = "";
		}
	}

	void CheckEnemy(){
		if(Vector2.Distance(playerAttack.transform.position, playerAttack.enemy.transform.position) <10){
			notif.text = "Ada musuh di dekat sini!";
		}
		else{
			notif.text = "";
		}
	}

	void CountAnimal(){
		notif.enabled = true;
		executedTime = Time.time;
		notif.text = "Sisa satwa di hutan = " + GameObject.FindGameObjectsWithTag("Target").Length.ToString();
		animalsTotal --;
		
		
		
	}
	public void ChangeWeapon(){
		notif.enabled = true;
		executedTime = Time.time;
		notif.text = "Senjata berganti ke " + playerAttack.activeWeapon;
	}
}
