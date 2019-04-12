using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour {
    public float bulletDamage = 10;
	private void OnTriggerEnter2D(Collider2D hit) {
		if(hit.tag == "Hitbox Enemy"){
			if(hit.GetComponentInParent<EnemyStat>() != null){
				hit.GetComponentInParent<EnemyStat>().TakeDamage(bulletDamage);
            	Destroy(gameObject);
			}
			
		}


		else if(hit.gameObject.layer == 8){
			Destroy(gameObject);
		}
	}
}
