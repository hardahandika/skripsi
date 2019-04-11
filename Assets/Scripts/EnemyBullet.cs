using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour {
	public float bulletDamage = 10;
	private void OnTriggerEnter2D(Collider2D hit) {
		if(hit.tag == "Hitbox Player"){
			if(hit.GetComponentInParent<PlayerStat>() != null){
				hit.GetComponentInParent<PlayerStat>().TakeDamage(bulletDamage);
            	Destroy(gameObject);
			}
			
		}


		else if(hit.gameObject.layer == 8){
			Destroy(gameObject);
		}
	}
}
