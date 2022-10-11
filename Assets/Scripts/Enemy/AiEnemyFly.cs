using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class AiEnemyFly : DatabaseEnemy, IDamegable
{
    	
	[Header("Movimenti")]
	public Transform[] points;
    public int currentPoint;
    public Transform platform;
	private float horizontal;

    [Header("Shooting")]
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public Transform firePoint;
    [SerializeField]
    public float timeBetweenShots;
    [SerializeField]
    public float shotCounter;
    [SerializeField]
    public float FUOCO;
    [SerializeField]
    public GameObject blam;

	[Header("Calcoli distanza")]
	[SerializeField] public float targetRange;
  

    private void Awake()
    {
        //Inizializzazione
        instance = this;
        moveCount = moveTime;
    }	


	private void FixedUpdate()
	{
		
		#region  Se il nemico NON sta attaccando
		if(!isAttack)
		{
		//La piattaforma si muove nei punti e nella velocità stabiliti
        platform.position = Vector3.MoveTowards(platform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);
		anim.SetBool("isMoving", true);
        //Conteggio dei puntidi locazione
        if(Vector3.Distance(platform.position, points[currentPoint].position) <.05f)
        {
            currentPoint++;

            if(currentPoint >= points.Length)
            {
                currentPoint = 0;

            }
        }
		}
		#endregion

		#region Se il nemico STA ATTACCANDO
		
		float disToPlayerToAttack = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
		Debug.DrawRay(transform.position, new Vector2(targetRange, 0), Color.red);

		if(disToPlayerToAttack < targetRange)
   			{
    			Shoot();
    			if(isAttack)
        		{
		   			
        		if (transform.position.x < PlayerMovement.instance.transform.position.x)
        		{
					horizontal = 1;
				}
				else 
				{
					horizontal = -1;
				}

        		Flip();

        		}
   			}
			else if(disToPlayerToAttack > targetRange)
   			{
    				StopAttack();
    		}
	}
		#endregion
	


	public void Shoot()
	{
		shotCounter -= Time.deltaTime;
		isAttack = true;
				if (shotCounter <= 0)
				{
					shotCounter = timeBetweenShots;
					//Repeting shooter
					var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
					Instantiate(blam, firePoint.position, firePoint.rotation);
					AudioManager.instance.PlaySFX(4);
					anim.SetBool("isAttack", isAttack);
					anim.SetTrigger("isShoot");
					newBullet.transform.localScale = Enemy.localScale;
				}
	}
	public void StopAttack()
    {
        isAttack = false;
        anim.SetBool("isAttack", isAttack = false);
        moveCount = moveTime;

    }

	private void Flip()
    {
        if (movingRight && horizontal < 0f || !movingRight && horizontal > 0f)
        {
            movingRight = !movingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

	#region Danno
 
private void OnTriggerEnter2D(Collider2D other)
    {
    
        if(other.tag == "Bullet")
        {
            StartCoroutine(HitEnemy());
            //Quando il nemico collide con il bullet parte la corutine
        }

    }

    #endregion
}
