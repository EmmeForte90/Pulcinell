using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIEnemyDefault : DatabaseEnemy, IDamegable

{
    //public int HP { get; set; }
    [Header("Movimenti")]
    [SerializeField]
    public Transform LP;
    public Transform RP;
    private float horizontal;
    //public Transform Detector;
    [Header("parametri d'attacco")]
    [SerializeField] public BoxCollider2D hitBox;
    [SerializeField] LayerMask playerlayer;
    [SerializeField] float nextAttackTime;
    [SerializeField] public float agroRange;
    //Variabile per il tempo d'attacco

    
    

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
        hitBox = GetComponent<BoxCollider2D>();
        //RB.velocity = new Vector2(moveSpeed, RB.velocity.y);

    }

void Update()
{

//Calcolo distanza tra player e nemico
float disToPlayer = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);

#region Se il nemico NON sta attaccando...

if(!isAttack && disToPlayer > agroRange){

    anim.SetBool("isRunning", false);

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
else if(disToPlayer < agroRange)
{           
    //Insegue il player
    ChasePlayer();
   
//Se sta attaccando
    if (isAttack)
        {
            if(PunchNow)
            {
                StartCoroutine(nextAttackTrue());
                hitBox.enabled = true;
            }else if(!PunchNow)
            {
                StartCoroutine(nextAttackFalse());
                hitBox.enabled = false;
            }
            
        }
            
            
}
else if(disToPlayer > agroRange)
{
StopChasingPlayer();
}

#endregion
} 


private void  ChasePlayer()
{
    anim.SetBool("isRunning", true);
if(transform.position.x < PlayerMovement.instance.transform.position.x)
{
    //Left
    RB.velocity = new Vector2(runSpeed, 0);
    movingRight = true;
    transform.localScale = new Vector2(1, transform.localScale.y);
}
else if(transform.position.x > PlayerMovement.instance.transform.position.x)
{
    //Right
    RB.velocity = new Vector2(-runSpeed, 0);
    movingRight = false;
    transform.localScale = new Vector2(-1, transform.localScale.y);

}
}

private void StopChasingPlayer()
{
    StopAttack();
    anim.SetBool("isRunning", false);
    RB.velocity = new Vector2(moveSpeed, 0);
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

#region  Attacco
public void Attack()
    {
    isAttack = true;
    anim.SetBool("isMoving", false);
    anim.SetBool("isAttack", true);
    StartCoroutine(nextAttackTrue());
    }

public IEnumerator nextAttackTrue()
{
    PunchNow = true;
    yield return new WaitForSeconds(nextAttackTime);
    PunchNow = false;
}
public IEnumerator nextAttackFalse()
{
    PunchNow = false;
    anim.SetBool("isAttack", false);
    yield return new WaitForSeconds(nextAttackTime);
    PunchNow = true;

}

public void StopAttack()
    {
        hitBox.enabled = false;
        isAttack = false;
        anim.SetBool("isAttack", false);
        moveCount = moveTime;

    }


#endregion

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



    #endregion

}