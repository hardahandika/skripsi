
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalFSM : FSM {

	public enum FSMState
	{
		Patrol,
		SedangDitangkap,
		Tertangkap,
		Diselamatkan,
		Terselamatkan
	}
        //Current state that the Box is in
        public FSMState currentState;
		public Animator animator;
		public AnimalMove ai;
		AnimalStat animalStat;
		[SerializeField] GameObject[] patrolPoints;
		GameObject enemy;
		public GameObject savemeter;
		int index = 0;
	protected override void Initialize ()
	{            
			
		//Get the list of points
		//GameObject[] patrolPoints = GameObject.FindGameObjectsWithTag ("Patrol Point");
		//Get the target enemy(Player)
		//objTarget = GameObject.FindGameObjectsWithTag ("Target");
		animator = GetComponent<Animator>();
		animalStat = GetComponent<AnimalStat>();
		ai = GetComponent<AnimalMove>();
	}

	//Update each frame
	protected override void FSMUpdate ()
	{
		HandleLayers();
		enemy = FindClosestTarget("Enemy");
		switch (currentState) {
		case FSMState.Patrol:
			UpdatePatrolState();
			break;
		case FSMState.SedangDitangkap:
			UpdateSedangDitangkapState();
			break;
		case FSMState.Tertangkap:
			UpdateStateTertangkapState();
			break;
		case FSMState.Diselamatkan:
			UpdateDiselamatkanState();
			break;
		case FSMState.Terselamatkan:
			UpdateTerselamatkanState();
			break;
		}

		if(animalStat.savemeter.MyCurrentValue > 0){
			savemeter.SetActive(true);
		}
		else{
			savemeter.SetActive(false);
		}
	}

    private void UpdateTerselamatkanState()
    {
		gameObject.SetActive(false);
		FindObjectOfType<AudioManager>().PlaySoundOneShot("Animal Saved");
    }

    private void UpdateDiselamatkanState()
    {
		if(animalStat.savemeter.MyCurrentValue == animalStat.savemeter.MyMaxValue){
			currentState = FSMState.Terselamatkan;
		}

		
        ai.enabled = false;
		animalStat.SavingTheAnimal();
    }

    private void UpdateSedangDitangkapState()
    {
		if(animalStat.health.MyCurrentValue == animalStat.health.MyMaxValue){
			currentState = FSMState.Tertangkap;
		}

		
        ai.enabled = false;
		
		animalStat.DrainHealth();
    }

    private void UpdateStateTertangkapState()
    {
        gameObject.SetActive(false);
		FindObjectOfType<AudioManager>().PlaySoundOneShot("Animal Captured");
    }

    

    private void UpdatePatrolState()
    {
		ai.enabled = true;


		ai.speed = 1.6f;
		ai.targetPosition = patrolPoints[index].transform;
		if(Vector3.Distance(transform.position, patrolPoints[index].transform.position) <= 1){
			if(index == patrolPoints.Length-1){
				index = 0;
			}
			else{
				index++;
			}
		}
		
    }

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Enemy"){
			currentState = FSMState.SedangDitangkap;
		}
		else if(other.tag == "Player"){
			currentState = FSMState.Diselamatkan;
		}
	}
	private void OnTriggerExit2D(Collider2D other) {
		if(other.tag == "Player" && animalStat.savemeter.MyCurrentValue < animalStat.savemeter.MyMaxValue){
			animalStat.UnsaveTheAnimal();
			currentState = FSMState.Patrol;
		}
		else if(other.tag == "Enemy" && animalStat.health.MyCurrentValue < animalStat.health.MyMaxValue){
			animalStat.RegenHealth();
			currentState = FSMState.Patrol;
		}
	}

	public GameObject FindClosestTarget(string des)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(des);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

	public void HandleLayers()
	{
		//manajemen layer animasi

		// else if(enemyAttack.isAttacking){
		// 	ActivateLayer("AttackLayer");
		// }
		if(ai.isMoving){
			if(ai.enabled == true){
				ActivateLayer("WalkLayer");
				animator.SetFloat("x", ai.horizontal);
				animator.SetFloat("y", ai.vertical);
			}
			else{
				ActivateLayer("IdleLayer");
			}
		}
		else{
			ActivateLayer("IdleLayer");
		}
	}

	public void ActivateLayer(string layerName){
		//mengaktifkan satu layer animasi dan mematikan yang lain
		for (int i = 0; i < animator.layerCount; i++)
		{
			animator.SetLayerWeight(i,0);
		}
		animator.SetLayerWeight(animator.GetLayerIndex(layerName),1);
	}

}
