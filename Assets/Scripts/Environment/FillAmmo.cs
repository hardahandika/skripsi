using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillAmmo : MonoBehaviour {
	SpawnItem item;

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Hitbox Player"){
			FindObjectOfType<AudioManager>().PlaySoundOneShot("Ammo Up");
			other.GetComponentInParent<PlayerStat>().machinegunAmmo = 60;
			other.GetComponentInParent<PlayerStat>().rifleAmmo = 30;
			
			Destroy(gameObject);
		}
	}
}
