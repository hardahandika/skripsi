using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour {
	public Animator animator;
	Bullet bullet;
	public GameObject BulletPrefab;
	Vector2 direction;
	[SerializeField] Transform[] ExitPoints;
	public GameObject player;

	public float fireRate = 0.2f;
	public float bulletSpeed = 10;

	public float nextFire;

    private int index;
	public bool isShooting;
    public int horizontalAttack;
    public int verticalAttack;

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
		bullet = GetComponent<Bullet>();
		player = FindPlayer();
	}
	
	// Update is called once per frame
	void Update () {
		SetExitPoints();
		SetDirectionForAttackHorizontal();
		SetDirectionForAttackVertical();
		if (isShooting && nextFire < Time.time){
			bulletShoot();
		}
		animator.SetBool("shoot", isShooting);
	}

	public void bulletShoot()
	{
		
		nextFire = Time.time + fireRate;
		Rigidbody2D bullet = Instantiate(BulletPrefab.GetComponent<Rigidbody2D>(), ExitPoints[index].position, Quaternion.identity);

		

		//arah tembakan
		direction = ((Vector2)player.transform.position - bullet.position).normalized * bulletSpeed;


		//peluru bergerak
		bullet.velocity = direction;
		//agar peluru menghadap ke musuh
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

	}

	public GameObject FindPlayer()
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag("Player");
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
		if(animator.GetFloat("x") == -1){
			index = 1; //array ke 1
		}
		else if(animator.GetFloat("x") == 1){
			index = 2; //array ke 2
		}
		else if(animator.GetFloat("y") == -1){
			index = 0; //array ke 0
		}
		else if(animator.GetFloat("y") == 1){
			index = 3;
		}
	}

	void SetDirectionForAttackHorizontal(){
		//cek kalau objek hampir sejajar pada sumbu x
		if (Mathf.Round(transform.position.x - player.transform.position.x) == 0)
		{
            horizontalAttack = 0;
        }
		else
		{
			if(transform.position.x > player.transform.position.x)
			{
                horizontalAttack = -1;
            }
			else
				{horizontalAttack = 1;}
		}
	}
	void SetDirectionForAttackVertical(){
		//cek kalau objek hampir sejajar pada sumbu x
		if (Mathf.Round(transform.position.y - player.transform.position.y) == 0)
		{verticalAttack = 0;}
		else{
			if(transform.position.y > player.transform.position.y)
				{verticalAttack = -1;}
			else
				{verticalAttack = 1;}
		}
	}
}
