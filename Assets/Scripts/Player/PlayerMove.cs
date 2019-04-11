using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour {

	public Vector2 direction;
	public VJHandler jsMovement;
	public Button sprintButton;
	public Button dashButton;
	public float horizontal, previousHorizontal, horizontalAttack;
	public float vertical, previousVertical, verticalAttack;
	public bool isMoving{
		get{
			return horizontal != 0 || vertical != 0;
		}
	}
    public bool isRunning;
    public bool isDashing;
	public bool isAttacking;
	public bool isShooting;
	public bool isExhausted = false;
    
    public float runningSpeed = 10f;
    public float moveLimiter = 0.7f;
    public float walkSpeed = 5f;
	public float walkExhaustedSpeed = 2f;
    public float dashTime = 0.05f;
    public float dashSpeed = 17f;

	private Rigidbody2D body;
	[HideInInspector] public Animator animator;
	PlayerStat playerStat;
    private PlayerAttack playerAttack;
    private Transform target;

    //PlayerAttack playerAttack;

    // Use this for initialization
    void Start () {
		body = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		playerStat = GetComponent<PlayerStat>();
		playerAttack = GetComponent<PlayerAttack>();
	}
	
	// Update is called once per frame
	void Update () {
		if(playerAttack.enemy != null){
			target = playerAttack.enemy.transform;
		}
		
		GetInput();
		SetDirectionForAttackHorizontal();
		SetDirectionForAttackVertical();
		HandleLayers();

		
	}

	void FixedUpdate ()
	{
		//pergerakan berdasarkan state
		if(!isAttacking && !isShooting){
			if(isRunning){
				if(isDashing)			{StartCoroutine(Dashing());}
				else if(isExhausted)	{WalkExhausted();}
				else					{Run();}
			}
			else if(isDashing)		{StartCoroutine(Dashing());}
			else if(isExhausted)	{WalkExhausted();}
			else					{Walk();}
		}
		else{
			Idle();
		}
		

		//pergerakan berdasarkan stamina
		

		
/* 		//jika berlari maka kecepatan bertambah
		if(!isExhausted && isRunning){
			Run();
		}
		else if(!isExhausted && isDashing){
			StartCoroutine(Dashing());
		}
			
		//jika berjalan saja maka kecepatan sedang
		else if(!isExhausted)
		{
			Walk();
		}
		else {
			WalkExhausted();
		} */
		
	}

	public void GetInput(){
		//deteksi gerakan analog
		direction = jsMovement.InputDirection;
		if(direction.magnitude != 0){
			horizontal = direction.normalized.x;
			vertical = direction.normalized.y;
		}
		else{
			horizontal = 0;
			vertical = 0;
		}

		//cek last movement dan exit targets
		if(horizontal != 0 || vertical != 0){
			previousHorizontal = horizontal;
			previousVertical = vertical;
		}
	}

	//Pergerakan
	void Idle(){
		{body.velocity = new Vector2(0,0); }
	}
	void Run(){
		
		if(horizontal != 0 && vertical != 0) 
		{body.velocity = new Vector2((horizontal * runningSpeed) * moveLimiter , (vertical * runningSpeed) * moveLimiter); }
		else 
		{body.velocity = new Vector2(horizontal * runningSpeed, vertical * runningSpeed); }
	}
	
	void Walk(){
		
		if(horizontal != 0 && vertical != 0) 
		{body.velocity = new Vector2((horizontal * walkSpeed) * moveLimiter , (vertical * walkSpeed) * moveLimiter); }
		else 
		{body.velocity = new Vector2(horizontal * walkSpeed, vertical * walkSpeed); }
	}

	void WalkExhausted(){
		
		if(horizontal != 0 && vertical != 0) 
		{body.velocity = new Vector2((horizontal * walkExhaustedSpeed) * moveLimiter , (vertical * walkExhaustedSpeed) * moveLimiter); }
		else 
		{body.velocity = new Vector2(horizontal * walkExhaustedSpeed, vertical * walkExhaustedSpeed); }
	}
	void Dash(){
		
		if(horizontal != 0 && vertical != 0) 
		{body.velocity = new Vector2((previousHorizontal * dashSpeed) * moveLimiter , (previousVertical * dashSpeed) * moveLimiter); }
		else 
		{body.velocity = new Vector2(previousHorizontal * dashSpeed, previousVertical * dashSpeed); }

	}
	public void ToggleDash(){
		if(!isAttacking && !isDashing){isDashing = true; isRunning = false;}
		else{isDashing = false;}
	}
	public void ToggleSprint(){
		if(isRunning){isRunning = false;}
		else{isRunning = true;}
	}

	IEnumerator Dashing()
	{
		Dash();
		// If you want the character to stop after dashing ends.
		//body.velocity = Vector2.zero;
		yield return new WaitForSeconds(dashTime);
		isDashing = false;
	}

	
	
	//Animasi
	public void HandleLayers()
	//manajemen layer animasi
	{


		if(!isAttacking && !isShooting){
			if(isMoving){
				ActivateLayer("WalkLayer");
				animator.SetFloat("x", horizontal);
				animator.SetFloat("y", vertical);
			}
			else{
				ActivateLayer("IdleLayer");
			}
		}
		else if(isDashing){
			ActivateLayer("WalkLayer");
			animator.SetFloat("x", horizontal);
			animator.SetFloat("y", vertical);
		}
		else if(isShooting){
			ActivateLayer("ShootLayer");
			animator.SetFloat("x", horizontalAttack);
			animator.SetFloat("y", verticalAttack);
		}
		else if(isAttacking){
			ActivateLayer("AttackLayer");
		}
	}

	//menentukan arah animasi karakter saat menembak
	void SetDirectionForAttackHorizontal(){
		//cek kalau objek hampir sejajar pada sumbu x
		if (Mathf.Round(transform.position.x - target.transform.position.x) == 0)
		{
            horizontalAttack = 0;
        }
		else
		{
			if(transform.position.x > target.transform.position.x)
			{
                horizontalAttack = -1;
            }
			else
				{horizontalAttack = 1;}
		}
	}
	void SetDirectionForAttackVertical(){
		//cek kalau objek hampir sejajar pada sumbu x
		if (Mathf.Round(transform.position.y - target.transform.position.y) == 0)
		{verticalAttack = 0;}
		else{
			if(transform.position.y > target.transform.position.y)
				{verticalAttack = -1;}
			else
				{verticalAttack = 1;}
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
