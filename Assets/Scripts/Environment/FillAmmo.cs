using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAmmo : MonoBehaviour {


	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Hitbox Player"){
			FindObjectOfType<AudioManager>().PlaySoundOneShot("Ammo Up");
			other.GetComponentInParent<PlayerStat>().ammoMax = 90;
			other.GetComponentInParent<PlayerStat>().ammo = 30;
		}
	}
}
