using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM : FSM {

	public enum FSMState
	{
		Patrol,
		KejarTarget,
		KejarPlayer,
		TangkapTarget,
		SerangPlayer,
		Kabur,
		Tertangkap
	}
        //Current state that the Box is in
        public FSMState currentState;
		public Animator animator;
		EnemyStat enemyStat;
		EnemyAttack enemyAttack;
		AstarAI ai;
		[SerializeField] GameObject[] patrolPoints;
		GameObject objPlayer;
		GameObject objTarget;
		int index;
	protected override void Initialize ()
	{            
			
		//Get the list of points
		//GameObject[] patrolPoints = GameObject.FindGameObjectsWithTag ("Patrol Point");
		//Get the target enemy(Player)
		//objTarget = GameObject.FindGameObjectsWithTag ("Target");
		enemyStat = GetComponent<EnemyStat>();
		enemyAttack = GetComponent<EnemyAttack>();
		animator = GetComponent<Animator>();
		objPlayer = GameObject.FindGameObjectWithTag ("Player");
		playerTransform = objPlayer.transform;
		index = UnityEngine.Random.Range(0, patrolPoints.Length-1);
		ai = GetComponent<AstarAI>();
	}

	//Update each frame
	protected override void FSMUpdate ()
	{
		HandleLayers();

		if(enemyStat.health.MyCurrentValue <= 30 && enemyStat.health.MyCurrentValue > 1){
			currentState = FSMState.Kabur;
		}

		switch (currentState) {
		case FSMState.Patrol:
			UpdatePatrolState();
			break;
		case FSMState.KejarTarget:
			UpdateKejarTargetState();
			break;
		case FSMState.KejarPlayer:
			UpdateKejarPlayerState();
			break;
		case FSMState.TangkapTarget:
			UpdateTangkapTargetState();
			break;
		case FSMState.SerangPlayer:
			UpdateSerangPlayerState();
			break;
		case FSMState.Kabur:
			UpdateStateKaburState();
			break;
		case FSMState.Tertangkap:
			UpdateStateTertangkapState();
			break;
		}
	}

    private void UpdateStateTertangkapState()
    {
        gameObject.SetActive(false);
		FindObjectOfType<AudioManager>().PlaySoundOneShot("Enemy Defeated");
    }

    private void UpdateStateKaburState()
    {


		//transisi ke state Tertangkap
		if(enemyStat.health.MyCurrentValue < 1){
			currentState = FSMState.Tertangkap;
		}
		//transisi ke state Kejar Player atau Patrol jika sudah sehat
		else if(enemyStat.health.MyCurrentValue == enemyStat.health.MyMaxValue){
			gameObject.SetActive(true);
			if(Vector3.Distance(transform.position, playerTransform.position) <= 8){
				ai.speed = 2;
				currentState = FSMState.KejarPlayer;
			}
			else{
				currentState = FSMState.Patrol;
			}

			enemyAttack.isShooting = false;
		}

		//output
		enemyAttack.isShooting = false;
        ai.targetPosition = FindClosestTarget("Escape Point").transform;
		ai.speed = 5;
    }

    private void UpdateSerangPlayerState()
    {
		//transisi ke state Patrol
		if(Vector3.Distance(transform.position, playerTransform.position) > 8){
			currentState = FSMState.Patrol;
		}

		//output
		enemyAttack.isShooting = true;
    }

    private void UpdateTangkapTargetState()
    {
		//transisi ke state Kejar Player
        if(Vector3.Distance(transform.position, playerTransform.position) <= 8){
			currentState = FSMState.KejarPlayer;
		}
		//transisi ke state Patrol
		else if(Vector3.Distance(transform.position, FindClosestTarget("Target").transform.position) > 18){
			currentState = FSMState.Patrol;
		}
    }

    private void UpdateKejarPlayerState()
    {
		//transisi ke state Kabur
		Debug.Log("Distance = "+Vector3.Distance(transform.position, playerTransform.position));
		if(enemyStat.health.MyCurrentValue <= 30){
			currentState = FSMState.Kabur;
		}
		//transisi ke state Patrol
		else if(Vector3.Distance(transform.position, playerTransform.position) > 8){
			currentState = FSMState.Patrol;
		}
		//transisi ke state Serang Player
		else if(Vector3.Distance(transform.position, playerTransform.position) <= 5){
			currentState = FSMState.SerangPlayer;
		}

		//output
		ai.speed = 3;
        ai.targetPosition = playerTransform;
    }

    private void UpdateKejarTargetState()
    {
		//transisi ke state Patrol
		if(Vector3.Distance(transform.position, FindClosestTarget("Target").transform.position) > 18){
			currentState = FSMState.Patrol;
		}
		//transisi ke state Kejar Player
		else if(Vector3.Distance(transform.position, playerTransform.position) <= 8){
			currentState = FSMState.KejarPlayer;
		}

		//output
		ai.speed = 3;
        ai.targetPosition = FindClosestTarget("Target").transform;
    }

    private void UpdatePatrolState()
    {
		//transisi ke state Kejar Target
		if(Vector3.Distance(transform.position, FindClosestTarget("Target").transform.position) <= 18){
			currentState = FSMState.KejarTarget;
		}
		//transisi ke state Kejar Player
		else if(Vector3.Distance(transform.position, playerTransform.position) <= 8){
			currentState = FSMState.KejarPlayer;
		}

		//output
		enemyAttack.isShooting = false;
		ai.speed = 2;
		ai.targetPosition = patrolPoints[index].transform;
		if(Vector3.Distance(transform.position, patrolPoints[index].transform.position) <= 1){
			index = UnityEngine.Random.Range(0, patrolPoints.Length-1);
		}
		
    }

	private void OnTriggerEnter2D(Collider2D other) {
		if(other.tag == "Target"){
			currentState = FSMState.TangkapTarget;
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
		if(!enemyAttack.isShooting){
			if(ai.isMoving){
				ActivateLayer("WalkLayer");
				animator.SetFloat("x", ai.horizontal);
				animator.SetFloat("y", ai.vertical);
			}
			else{
				ActivateLayer("IdleLayer");
			}
		}
		else if(enemyAttack.isShooting){
			ActivateLayer("ShootLayer");
			animator.SetFloat("x", enemyAttack.horizontalAttack);
			animator.SetFloat("y", enemyAttack.verticalAttack);
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
