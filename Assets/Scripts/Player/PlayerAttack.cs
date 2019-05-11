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
	public GameObject RifleBulletPrefab;
	Vector2 direction;
	[SerializeField] Transform[] ExitPoints;
	int exitIndex;
	public ButtonEvents shootButton;
	public GameObject enemy;

	public float machinegunFireRate = 0.1f;
	public float rifleFireRate = 1f;
	public float bulletSpeed = 10;

	public float nextFire;

	int bulletCount;
    private int index;

	public enum weapon{
		Machinegun,
		Rifle
	}
	public weapon activeWeapon;

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
		activeWeapon = weapon.Machinegun;
	}
	
	void Update()
	{
		//AmmoHandler();
		GetInput();
		enemy = FindClosestEnemy();
		SetExitPoints();
		switch(activeWeapon){
			case weapon.Rifle:
			animator.SetFloat("speed", 0.16f);
			break;
			
			case weapon.Machinegun:
			animator.SetFloat("speed", 1f);
			break;
			default:
			break;
		}
		//inLineOfSight();

	}

	void FixedUpdate () {
		
		if (playerMove.isShooting && nextFire < Time.time){
			if(GameObject.FindGameObjectsWithTag("Enemy").Length > 0){
				switch(activeWeapon){
					case weapon.Machinegun:
					if(playerStat.machinegunAmmo > 0)
					MachinegunShoot();
					break;
					case weapon.Rifle:
					if(playerStat.rifleAmmo > 0)
					RifleShoot();
					break;
					default:
					break;
				}
				
			}
				
			
			else{ToggleShoot();}
		}
		switch(activeWeapon){
					case weapon.Machinegun:
					playerStat.ammoText.text = "Peluru Machine Gun = " + playerStat.machinegunAmmo.ToString();
					break;
					case weapon.Rifle:
					playerStat.ammoText.text = "Peluru Rifle = " + playerStat.rifleAmmo.ToString();
					break;
					default:
					break;
				}
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
			if(enemy == null){
				playerMove.isShooting = false;
			}
			else{playerMove.isShooting = true;}
		}
		else
		{
			playerMove.isShooting = false;
		}
		animator.SetBool("shoot", playerMove.isShooting);
	}

	//toggle bool is shooting
	public void ToggleShoot(){
		

		//cek jarak jangkau tembak
		if(Vector2.Distance(transform.position, enemy.transform.position) < 5){
			if(!playerMove.isShooting){playerMove.isShooting = true;}
			else{playerMove.isShooting = false;}
		}
		else{playerMove.isShooting = false;}
	}


	//MachinegunShoot
	public void MachinegunShoot()
	{
		nextFire = Time.time + machinegunFireRate;
		Rigidbody2D bullet = Instantiate(BulletPrefab.GetComponent<Rigidbody2D>(), ExitPoints[index].position, Quaternion.identity);

		audioManager.PlaySoundOneShot("Shoot");

		playerStat.machinegunAmmo --;
		

		//arah tembakan
		direction = ((Vector2)enemy.transform.position - bullet.position).normalized * bulletSpeed;


		//peluru bergerak
		bullet.velocity = direction;
		//agar peluru menghadap ke musuh
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		//AmmoHandler();
		if(Vector3.Distance(bullet.transform.position, transform.position) > 1){
			Destroy(bullet);
		}
		//Debug.Log("Sisa Peluru = "+playerStat.ammo.MyCurrentValue);
	}

	public void RifleShoot()
	{
		nextFire = Time.time + rifleFireRate;
		Rigidbody2D riflebullet = Instantiate(RifleBulletPrefab.GetComponent<Rigidbody2D>(), ExitPoints[index].position, Quaternion.identity);

		audioManager.PlaySoundOneShot("Shoot");

		playerStat.rifleAmmo --;
		

		//arah tembakan
		direction = ((Vector2)enemy.transform.position - riflebullet.position).normalized * (bulletSpeed*1.2f);


		//peluru bergerak
		riflebullet.velocity = direction;
		//agar peluru menghadap ke musuh
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		riflebullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		//AmmoHandler();
		if(Vector3.Distance(riflebullet.transform.position, transform.position) > 1){
			Destroy(riflebullet);
		}
		//Debug.Log("Sisa Peluru = "+playerStat.ammo.MyCurrentValue);
	}

	public void SwitchWeapon(){
		switch(activeWeapon){
			case weapon.Machinegun:
			activeWeapon = weapon.Rifle;
			break;
			case weapon.Rifle:
			activeWeapon = weapon.Machinegun;
			break;
			default:
			break;
		}
	}

	// void AmmoHandler(){
	// 	if(playerStat.ammo == 0){
			
	// 		if(playerStat.ammoMax >= 30){
	// 			playerStat.ammo = 30;
	// 			playerStat.ammoMax -= 30;
	// 		}
	// 		else if(playerStat.ammoMax < 30){
	// 			playerStat.ammo = playerStat.ammoMax;
	// 			playerStat.ammoMax = 0;
	// 		}
	// 	}
	// }

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
