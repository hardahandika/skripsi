using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyMove : MonoBehaviour {
	public Transform targetPosition;
	public Transform[] targets;
	int index;

    private Seeker seeker;
    private CharacterController controller;

    public Path path;

    public float speed = 2;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;
	public bool canSearch = true;
	public float repathRate = 0.5F;

	float switchTime = float.PositiveInfinity;

	public float delay = 0;

    private Rigidbody2D body;
    private Animator animator;
	public float horizontal, previousHorizontal;
	public float vertical, previousVertical;

	public float walkSpeed = 0.5f;
	public bool isMoving{
		get{
			return horizontal != 0 || vertical != 0;
		}
	}

    // Use this for initialization
    void Start () {
		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();



		seeker = GetComponent<Seeker>();
        // If you are writing a 2D game you can remove this line
        // and use the alternative way to move sugggested further below.
        controller = GetComponent<CharacterController>();

        // Start a new path to the targetPosition, call the the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
	}

	public void OnPathComplete (Path p) {
        Debug.Log("Yay, we got a path back. Did it have an error? " + p.error);

		if (!p.error) {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {

		SetFaceDirectionHorizontal();
		SetFaceDirectionVertical();
		HandleLayers();

	}

	public void ActivateLayer(string layerName){
		//mengaktifkan satu layer animasi dan mematikan yang lain
		for (int i = 0; i < animator.layerCount; i++)
		{
			animator.SetLayerWeight(i,0);
		}
		animator.SetLayerWeight(animator.GetLayerIndex(layerName),1);
	}

	public void HandleLayers()
	{
		//manajemen layer animasi
		if(isMoving){
			ActivateLayer("WalkLayer");
			animator.SetFloat("x", horizontal);
			animator.SetFloat("y", vertical);
		}
		// else if(enemyAttack.isAttacking){
		// 	ActivateLayer("AttackLayer");
		// }
		
		else{
			
			ActivateLayer("IdleLayer");
		}
	}

	void SetFaceDirectionHorizontal(){
		
		//cek kalau objek hampir sejajar pada sumbu x
		if (Mathf.Round(transform.position.x - path.vectorPath[currentWaypoint].x) == 0)
		{horizontal = 0;}
		else
		{
			if(transform.position.x > path.vectorPath[currentWaypoint].x)
				{horizontal = -1;}
			else
				{horizontal = 1;}
		}
	}
	void SetFaceDirectionVertical(){
		//cek kalau objek hampir sejajar pada sumbu x
		if (Mathf.Round(transform.position.y - path.vectorPath[currentWaypoint].y) == 0)
		{vertical = 0;}
		else{
			if(transform.position.y > path.vectorPath[currentWaypoint].y)
				{vertical = -1;}
			else
				{vertical = 1;}
		}
	}

	void Chase(){
		body.velocity = new Vector2(horizontal * walkSpeed, vertical * walkSpeed);
	}
}
