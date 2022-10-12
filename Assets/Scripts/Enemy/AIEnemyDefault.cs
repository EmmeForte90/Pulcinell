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
    [Header("parametri d'attacco")]
    [SerializeField] LayerMask playerlayer;
    [SerializeField] float nextAttackTime;
    [SerializeField] public float attackRange;
    [SerializeField] public float agroRange;
    //Variabile per il tempo d'attacco

    private void Awake()
    {
        //Inizializzazione
        instance = this;
        LP.parent = null;
        RP.parent = null;
        moveCount = moveTime;
    }
    

void Update()
{
//Calcolo distanza tra player e nemico
float disToPlayer = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
Debug.DrawRay(transform.position, new Vector2(agroRange, 0), Color.red);
Debug.DrawRay(transform.position, new Vector2(attackRange, 0), Color.blue);
#region Se il nemico NON sta attaccando...

if(!isAttack && disToPlayer > agroRange){

    StopAttack();

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
    //Se non sta attaccando 
    if(!isAttack)
    {
    //Insegue il player
    ChasePlayer();
    }
    //Se sta attaccando
   //Colpisce il player se si trova nel range d'attacco
   float disToPlayerToAttack = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
   //Debug.Log("disToPlayerToAttack:" +disToPlayerToAttack); 
   if(disToPlayerToAttack < attackRange)
   {
    Attack();
    AudioManager.instance.PlaySFX(3);
    PlayerMovement.instance.Hurt();
    }       
}//Altrimenti smettere di inseguirlo
else if(disToPlayer > agroRange)
{
StopChasingPlayer();
}

#endregion
} 
#region  Insegue il player

private void  ChasePlayer()
{
    anim.SetBool("isRunning", true);
if(transform.position.x < PlayerMovement.instance.transform.position.x)
{
    //Sinistra
    RB.velocity = new Vector2(runSpeed, 0);
    movingRight = true;
    transform.localScale = new Vector2(1, transform.localScale.y);
}
else if(transform.position.x > PlayerMovement.instance.transform.position.x)
{
    //Destra
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

#endregion

#region  Flippa lo sprite
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

#endregion

#region  Attacco
public void Attack()
    {
    isAttack = true;
    RB.velocity = new Vector2(0, 0);
    anim.SetBool("isMoving", false);
    anim.SetBool("isAttack", true);
    if(movingRight)
        {
        PlayerMovement.instance.knockBackLeftDir();
        }
        else if(!movingRight)
        {
        PlayerMovement.instance.knockBackRightDir();
        }
    }

public void StopAttack()
    {
        isAttack = false;
        anim.SetBool("isAttack", false);
        anim.SetBool("isRunning", false);
        moveCount = moveTime;

    }


#endregion

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