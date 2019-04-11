using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notifications : MonoBehaviour {

	public Text notif;
	int animalsTotal = 4;
	PlayerStat playerStat;
	PlayerAttack playerAttack;

	void Start()
	{
		playerStat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStat>();
		playerAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>();
		notif = GetComponent<Text>();
	}
	void Update()
	{
		//CheckAmmo();
		//CheckEnemy();

		if(GameObject.FindGameObjectsWithTag("Target").Length < animalsTotal){
			StartCoroutine("CountAnimal");
		}
	}

	void CheckAmmo(){
		if(playerStat.ammo == 0 && playerStat.ammoMax == 0){
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

	IEnumerator CountAnimal(){
		if(GameObject.FindGameObjectsWithTag("Target").Length < animalsTotal)
		notif.text = "Sisa satwa di hutan = " + GameObject.FindGameObjectsWithTag("Target").Length.ToString();
		animalsTotal --;
		yield return new WaitForSeconds(2);
		notif.text = "";
		yield return null;
	}
}
