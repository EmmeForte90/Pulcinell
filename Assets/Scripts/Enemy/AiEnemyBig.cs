using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiEnemyBig : DatabaseEnemy, IDamegable
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
    [SerializeField] public float crashRange;
    [SerializeField] public float agroRange;
    [SerializeField] public float timeBeforeAttack = 1f;
    //[SerializeField] public float frontcheck = 0.51f;
    //Variabile per il tempo d'attacco
    private bool isCrash = false;
    [Header("Calcoli distanza")]
    public Transform agroGizmo;
	public Transform rangeGizmo;
    
    [SerializeField] bool fatEnemy;


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
//hit = Physics2D.Raycast(new Vector2(transform.position.x + moveSpeed * frontcheck, transform.position.y));
Debug.DrawRay(agroGizmo.transform.position, new Vector2(agroRange, 0), Color.red);
Debug.DrawRay(rangeGizmo.transform.position, new Vector2(attackRange, 0), Color.blue);

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
                    //Se la posizione ?? maggiore del RightPoint
                    {
                        movingRight = false;
                        //Non si muove pi?? a destra
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
                //Se il conteggio del movimento ?? uguale o inferiore a zero
                {
                    waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
                    //Il tempo di attesa diventa randomico
                }

                anim.SetBool("isMoving", true);
                anim.SetBool("isCrash", false);
            }
            else if (waitCount > 0)
            //Se il conteggio del movimento ?? maggiore di zero
            {
                waitCount -= Time.deltaTime;
                RB.velocity = new Vector2(0f, RB.velocity.y);
                //Il personaggio si muove
                if (waitCount <= 0)
                //Se il conteggio del movimento ?? uguale o inferiore a zero
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
    
    StartCoroutine(PrepareAttack());
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
    anim.SetBool("isCrash", false);
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

private IEnumerator PrepareAttack()
{
    isAttack = true;
    RB.velocity = new Vector2(0, 0);
    anim.SetBool("isMoving", false);
    anim.SetBool("isAttack", true);
    anim.SetBool("isCrash", false);
    yield return new WaitForSeconds(timeBeforeAttack);
    StartCoroutine(crashAnimation());
    //CinemachineShake.instance.ShakeCamera(7f, .5f);
    Crash();
    
}

private IEnumerator crashAnimation()
{
    anim.SetBool("isCrash", true);
    yield return new WaitForSeconds(1f);
    anim.SetBool("isCrash", false);
    
}

private void Crash()
{
    float crashAttack = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
   //Debug.Log("crashAttack:" +crashAttack); 
   if(crashAttack < crashRange)
   {
    AudioManager.instance.PlaySFX(3);
    PlayerMovement.instance.Hurt();

    if(fatEnemy)
    {
        if(movingRight)
        {
        PlayerMovement.instance.knockBackBigLeft();
        }
        else if(!movingRight)
        {
        PlayerMovement.instance.knockBackBigRight();
        }
    }
    else if(!fatEnemy)
    {
        if(movingRight)
        {
        PlayerMovement.instance.knockBackLeftDir();
        }
        else if(!movingRight)
        {
        PlayerMovement.instance.knockBackRightDir();
        }
    }
}  
    
}

public void StopAttack()
    {
        isAttack = false;
        isCrash = false;
        anim.SetBool("isAttack", false);
        anim.SetBool("isCrash", false);
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
