using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyGun : DatabaseEnemy, IDamegable

{
    //public int HP { get; set; }
        
    [Header("Movimenti")]
    [SerializeField]
    public Transform LP;
    public Transform RP;
    private float horizontal;
    //public Transform Detector;
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
    
    

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        //Inizializzazione
        //RB = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        LP.parent = null;
        RP.parent = null;
        //Detector.parent = null;
        moveCount = moveTime;
        //RB.velocity = new Vector2(moveSpeed, RB.velocity.y);

    }

void Update()
{

//Calcolo distanza tra player e nemico

#region Se il nemico NON sta attaccando...

if(!isAttack){

            if (moveCount > 0)
            //Tempo di pausa per far fermare il nemico
            {
                moveCount -= Time.deltaTime;
                //MovimentoPer deltatime

                if (movingRight)
                //Se si muove a destra
                {
                    RB.velocity = new Vector2(moveSpeed, RB.velocity.y);
                    //Un vettore lo fa muovere a destra
                    if (transform.position.x > RP.transform.position.x)
                    //Se la posizione è maggiore del RightPoint
                    {
                        movingRight = false;
                        //Non si muove più a destra
                        transform.localScale = new Vector2(-1, transform.localScale.y);
                        //Si volta dall'altra parte e inizia a muoversi
                    }
                }
                else
                {
                    RB.velocity = new Vector2(-moveSpeed, RB.velocity.y);
                    //Il vettore lo fa muovere a sinistra
                    if (transform.position.x < LP.transform.position.x)
                    {
                        movingRight = true;
                        //Si muove a destra
                        transform.localScale = new Vector2(1, transform.localScale.y);
                        //Si volta dall'altra parte e inizia a muoversi
                    }
                }

                if (moveCount <= 0)
                //Se il conteggio del movimento è uguale o inferiore a zero
                {
                    waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
                    //Il tempo di attesa diventa randomico
                }

                anim.SetBool("isMoving", true);
            }
            else if (waitCount > 0)
            //Se il conteggio del movimento è maggiore di zero
            {
                waitCount -= Time.deltaTime;
                RB.velocity = new Vector2(0f, RB.velocity.y);
                //Il personaggio si muove
                if (waitCount <= 0)
                //Se il conteggio del movimento è uguale o inferiore a zero
                {
                    moveCount = Random.Range(moveTime * .75f, waitTime * .75f);
                    //Il tempo di movimento diventa randomico
                }
                anim.SetBool("isMoving", false);
            }
        } 
#endregion

#region Se il nemico STA ATTACCANDO
    //Altrimenti se è un nemico con un fucile
    else if (isAttack)
        {
        if (transform.position.x < PlayerMovement.instance.transform.position.x)
        {horizontal = 1;}
		else {horizontal = -1;}
        Flip();
        }
       
}


#endregion


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
#region Danno
 
private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.tag == "Player")
        {

            //anim.SetTrigger("isAttack");
            //attack = true;
            //AudioManager.instance.PlaySFX(10);
            //HitEnemy();
        }*/
        if(other.tag == "Bullet")
        {
            StartCoroutine(HitEnemy());
            //Quando il nemico collide con il bullet parte la corutine
        }

    }

// Cooldown dell'attacco
IEnumerator HitEnemy()
    {
        //Attacco.gameObject.SetActive(false);
        moveCount = 0;
        //Il nemico si ferma 
        anim.SetTrigger("isHurt");
        RB.AddForce(transform.up * bounceForce);
        yield return new WaitForSeconds(0.5f);
        //Si ferma per mezzo secondo
        moveCount = 1;
        //Poi riparte
        isAttack = false;
        //Attacco.gameObject.SetActive(true);

    }

    #endregion


}


