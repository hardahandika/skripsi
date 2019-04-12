using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {
	AudioManager audioManager;
	Animator animator;
	PlayerMove playerMove;
	PlayerStat playerStat;
	Bullet bullet;
	public GameObject BulletPrefab;
	Vector2 direction;
	[SerializeField] Transform[] ExitPoints;
	int exitIndex;
	public ButtonEvents shootButton;
	public GameObject enemy;

	public float fireRate = 0.1f;
	public float bulletSpeed = 10;

	public float nextFire;

	int bulletCount;
    private int index;

	private bool inLineOfSight(){
		Vector3 targetDirection = (enemy.transform.position - transform.position);
		Debug.DrawRay(transform.position, targetDirection, Color.red);
		return false;
	}

    // Use this for initialization
    void Start () {
		audioManager = FindObjectOfType<AudioManager>();
		animator = GetComponent<Animator>();
		playerMove = GetComponent<PlayerMove>();
		playerStat = GetComponent<PlayerStat>();
		bullet = GetComponent<Bullet>();
		enemy = FindClosestEnemy();
	}
	
	void Update()
	{
		AmmoHandler();
		GetInput();
		enemy = FindClosestEnemy();
		SetExitPoints();
		//inLineOfSight();

	}

	void FixedUpdate () {
		
		if (playerMove.isShooting && nextFire < Time.time){
			if(playerStat.ammo > 0 || playerStat.ammoMax > 0){
				if(GameObject.FindGameObjectsWithTag("Enemy").Length > 0){
					bulletShoot();
				}
				
			}
			
			else{ToggleShoot();}
		}
		playerStat.ammoText.text = playerStat.ammo + " / " + playerStat.ammoMax;
	}
	

	//toggle bool is attacking
	public void ToggleAttack(){
		if(!playerMove.isAttacking){playerMove.isAttacking = true;}
		else{playerMove.isAttacking = false;}
		animator.SetBool("attack", playerMove.isAttacking);
	}

	//jika tombol shoot ditekan
	void GetInput(){
		if(shootButton.isButtonDown){
			//if(Vector2.Distance(transform.position, enemy.transform.position) < 5){
			if(playerStat.ammo > 0 || playerStat.ammoMax > 0){
				playerMove.isShooting = true;
			}
			else{
				playerMove.isShooting = false;
			}
		}
		else
		{
			playerMove.isShooting = false;
		}
		//}
		
		//else{playerMove.isShooting = false;}
		animator.SetBool("shoot", playerMove.isShooting);
	}

	//toggle bool is shooting
	public void ToggleShoot(){


		//cek jarak jangkau tembak
		if(Vector2.Distance(transform.position, enemy.transform.position) < 5){
			if(playerStat.ammo > 0 || playerStat.ammoMax > 0){
				if(!playerMove.isShooting){playerMove.isShooting = true;}
				else{playerMove.isShooting = false;}
			}
			else{playerMove.isShooting = false;}
		}
		
		else{playerMove.isShooting = false;}
		animator.SetBool("shoot", playerMove.isShooting);
	}


	//bulletshoot
	public void bulletShoot()
	{
		nextFire = Time.time + fireRate;
		Rigidbody2D bullet = Instantiate(BulletPrefab.GetComponent<Rigidbody2D>(), ExitPoints[index].position, Quaternion.identity);

		audioManager.PlaySoundOneShot("Shoot");

		playerStat.ammo --;
		

		//arah tembakan
		direction = ((Vector2)enemy.transform.position - bullet.position).normalized * bulletSpeed;


		//peluru bergerak
		bullet.velocity = direction;
		//agar peluru menghadap ke musuh
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		AmmoHandler();
		if(Vector3.Distance(bullet.transform.position, transform.position) > 1){
			Destroy(bullet);
		}
		//Debug.Log("Sisa Peluru = "+playerStat.ammo.MyCurrentValue);
	}

	void AmmoHandler(){
		if(playerStat.ammo == 0){
			
			if(playerStat.ammoMax >= 30){
				playerStat.ammo = 30;
				playerStat.ammoMax -= 30;
			}
			else if(playerStat.ammoMax < 30){
				playerStat.ammo = playerStat.ammoMax;
				playerStat.ammoMax = 0;
			}
		}
	}

	public GameObject FindClosestEnemy()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Enemy");
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
	
	void SetExitPoints(){
		if(playerMove.animator.GetFloat("x") == -1){
			index = 1; //array ke 1
		}
		else if(playerMove.animator.GetFloat("x") == 1){
			index = 2; //array ke 2
		}
		else if(playerMove.animator.GetFloat("y") == -1){
			index = 0; //array ke 0
		}
		else if(playerMove.animator.GetFloat("y") == 1){
			index = 3;
		}
	}
}
