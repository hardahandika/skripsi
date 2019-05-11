using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnItem : MonoBehaviour {
	// Use this for initialization
	public GameObject something;
	public GameObject[] spawnSpot;
	void Start () {
		StartCoroutine(Spawn());
	}
	
	// Update is called once per frame
	void Update () {
	}
	IEnumerator Spawn(){
		while(true){
			if(GameObject.FindGameObjectsWithTag("Ammo").Length < 6){
				int randomIndex = Random.Range(0, spawnSpot.Length);
				if(spawnSpot[randomIndex].transform.childCount == 0){
					Instantiate(something, spawnSpot[randomIndex].transform);
					yield return new WaitForSeconds(2f);
				}
				else{
					yield return null;
				}
			}
			else{
				yield return null;
			}
		}
	}
}
