using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usa le API di unity di base
using UnityEngine.InputSystem;
//Usa l'input system di Unity scaricato dal packet manager
using Spine.Unity;
//Collega Spine a Unity
using Spine;
//Usa i Json di Spine

public class PlayerMovement : MonoBehaviour
{

    [Header("Fisica")]
    [SerializeField] float runSpeed = 10f; 
    // Variabile per il valore della corsa
    [SerializeField] float jumpSpeed = 5f;
    // Variabile per il valore della velocità del salto
    [SerializeField] float climbSpeed = 5f;
    //Variabile per la velocità di arrampicata
    float gravityScaleAtStart;
    //Variabile per la gravità
    Vector2 knockBackRight = new Vector2 (-5f, 5f);
    Vector2 knockBackLeft = new Vector2 (5f, 5f);
    
    Vector2 knockBackRightBig = new Vector2 (-10f, 10f);
    Vector2 knockBackLeftBig = new Vector2 (10f, 10f);
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    //[SerializeField] float knockBack;
    //Saltello di morte
    public Rigidbody2D myRigidbody;
    //Variabile per il rigidbody
    protected bool m_FacingRight = true;

    private SkeletonMecanim skelGraph;

    [Header("Animazione e Collisioni")]
    Animator myAnimator;
    //Variabile per l'animator 
    CapsuleCollider2D myBodyCollider;
    //Variabile per il capsule collider 
    BoxCollider2D myFeetCollider;
    //Variabile per il box collider
    
    [Header("Abilitazioni")]
    [SerializeField] public GameObject PauseMenu;
    //Variabile per identificare il menu
    [SerializeField] GameObject Player;
    //Variabile per identificare il player
    Vector2 moveInput; 
    //Variabile per il vettore che serve al player per muoversi
    bool isAlive = true;
    //Variabile per identificare la morte
    bool stopInput = false;
    //Blocca i movimenti
    bool heShoot = false;
    //Sta sparando
    bool lookUp = false;
    //Sta guardando su
    bool lookDown = false;
    //Sta guardando giù
    bool isInvincible = false;
    //è invincibile
    private Vector2 stopMove = new Vector2 (0f, 0f);
    //Blocca il vettore del player
    //[HideInInspector]
    bool isGround = false;
    //Variabile per identificare il terreno
    bool isMoving = false;
    //Si sta muovendo
    bool canDoubleJump = false;
    //Può fare il doppio salto
    private bool platform = false;
    //Variabile per identificare la piattaforma
    bool isFall = false;
    bool isClimbing = false;
    bool moveClimb = false;
    [SerializeField] bool wallJumpSkill = false;
    //Variabile per verificare se è sbloccato il walljump
    [SerializeField] bool doubleJumpSkill = false;
    //Variabile per verificare se è sbloccato il doppio salto
    [SerializeField] bool isKnockBack = false;
    //[SerializeField] float timeKnockBack;

    [SerializeField] public LayerMask layerMask;
    //Variabile per identificare i vari layer
    float timeInvincible = 1f;
    //Tempo di invincibilità
    Vector2 wallJumpDir;
    //Vettore del walljump
    bool canWallJumpLeft = false, canWallJumpRight = false, isWallJumping = false;
    //Variabili per il processo di walljump
    
    [Header("Gizmo per l'engine")]
    [SerializeField] float wallJumpDistance = 1f;
    [SerializeField] float knockDirectionDistance = 1f;
    
    [Header("Attacco")]
    [SerializeField] float attackRate = 2f;
    //Variabile per quantificare le volte di attacco
    [SerializeField] float attackrange = 0.5f;
    //Variabile per il raggio d'attacco
    [SerializeField] float nextAttackTime = 0f;
    //Variabile per il tempo d'attacco
    float bounceForce = 7f;
    //La forza per il knockback
    
    [Header("VFX")]
    [SerializeField] private GameObject bullet;
    // Variabile per il gameobject del proiettile
    [SerializeField] GameObject blam;
    //Variabile per identificare il vfx dell'esplosione
    [SerializeField] GameObject BAM;
    [SerializeField] public Transform gun;
    //Variabile per spostare o scalare l'oggetto
    [SerializeField] public Transform stompBox;
    [SerializeField] public Transform backDirection;
    [SerializeField] public GameObject dust;
    [SerializeField] GameObject Change;
    //VFX per il cambio arma
    [SerializeField] public ParticleSystem footsteps;
    [SerializeField] public ParticleSystem impactEffect;
    private ParticleSystem.EmissionModule footEmission;
    //Particle system interact  
     [Header ("Morte")]
    public GameObject callDie;

private void Awake()
    {
        instance = this;
        myRigidbody = GetComponent<Rigidbody2D>();
        //Recupera i componenti del rigidbody
        myAnimator = GetComponent<Animator>();
        //Recupera i componenti dell'animator
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        //Recupera i componenti del capsule collider
        myFeetCollider = GetComponent<BoxCollider2D>();
        //Recupera i componenti del box collider
        //Player = GetComponent<GameObject>();
        gravityScaleAtStart = myRigidbody.gravityScale;
        //Le dimensioni della gravità diventano quelle del rigidbody
        skelGraph = this.GetComponent<SkeletonMecanim>();
        footEmission = footsteps.emission;
        //Emissione Particle
    }

#region Singleton
        public static PlayerMovement instance;
        public static PlayerMovement Instance
        {
            //Se non trova il componente lo trova in automatico
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<PlayerMovement>();
                return instance;
            }
        }
        #endregion

#region FixedUpdate

    void FixedUpdate()
    {
        if (!isAlive) { return; }
        //Se il player è morto si disattiva la funzione
        //Altrimenti si attivano le funzioni

        //Calcolo distanza tra player e nemico
        float disToPlayer = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        //hit = Physics2D.Raycast(new Vector2(transform.position.x + moveSpeed * frontcheck, transform.position.y));
        Debug.DrawRay(backDirection.transform.position, new Vector2(knockDirectionDistance, 0), Color.blue);
        Debug.DrawRay(transform.position, new Vector2(wallJumpDistance, 0), Color.red);

        Run();
        FlipSprite();
        CheckGround();
        if(wallJumpSkill)
        //Se l'abilità walljump non è sbloccato non puoi usare questa abilità
        {
        checkWallJump();
        }
        ClimbLadder();
        Die();
       {
       if (isMoving && isGround)
        {
            dust.gameObject.SetActive(true);
            footEmission.rateOverTime = 35f;
            
        } else if (!isMoving && isGround)
        {
            footEmission.rateOverTime = 0f;
            dust.gameObject.SetActive(false);
        }

        if(myRigidbody.velocity.y > 0)
        {
            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            myAnimator.SetTrigger("Jump");

        } 
        else if(myRigidbody.velocity.y < 0)
        {
            myRigidbody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            isFall = true;
            UpdateAnimation();
        }


    }
       

    }
#endregion

void UpdateAnimation()
    {
            myAnimator.SetBool("isGround", isGround);
            myAnimator.SetBool("wallJump", isWallJumping);
            myAnimator.SetBool("lookDown", lookDown);
            myAnimator.SetBool("lookUp", lookUp);
            myAnimator.SetBool("isFall", isFall);

    }


#region CheckGround
void CheckGround()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, 1f, layerMask))
        {
            isGround = true;
            isFall = false;
            UpdateAnimation();
            if(doubleJumpSkill)
            //Se l'abilità doppio salto non è sbloccato non puoi usare questa abilità
            {
            canDoubleJump = true;
            }
        }
        else
        {
            isGround = false;
            UpdateAnimation();

        }
    }
#endregion

#region WallJump
void checkWallJump()
{

     if(Physics2D.Raycast(transform.position, Vector2.left, 1f, layerMask))
        {
            canWallJumpLeft = true;
            canWallJumpRight = false;
            isWallJumping = false;
            UpdateAnimation();

        }
        else
        {
            canWallJumpLeft = false;
            UpdateAnimation();

        }
        
        if(Physics2D.Raycast(transform.position, Vector2.right, 1f, layerMask))
        {
            canWallJumpLeft = false;
            canWallJumpRight = true;
            isWallJumping = false;
            UpdateAnimation();
        }
        else
        {
            canWallJumpRight = false;
            UpdateAnimation();
        }
}
#endregion

#region Risalto sul nemico
public void BumpEnemy()
{
    myAnimator.SetTrigger("Jump");
    isGround = false;
    Instantiate(BAM, stompBox.position, transform.rotation);
    AudioManager.instance.PlaySFX(3);
    myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, bounceForce);
}
#endregion

#region Guarda in basso/alto

public void OnLookUp(InputValue value)
//Funzione guarda su
{
    if(!isClimbing)
    {
    if (value.isPressed)
    {
        stopInput = true;
        lookUp = true;
        lookDown = false;
        myAnimator.SetBool("lookUp", lookUp);
        myRigidbody.velocity = stopMove;
    }
    else
    {
        lookUp = false;
    }
    }
}
public void OnLookDown(InputValue value)
//Funzione guarda su
{
    if(!isClimbing)
    {
    if (value.isPressed)
    {
        stopInput = true;
        lookDown = true;
        lookUp = false;
        UpdateAnimation();
        //myAnimator.SetBool("lookDown", lookDown);
        myRigidbody.velocity = stopMove;
    }
    else
    {
        lookDown = false;
    }
    }
}

public void LookNormal()
{
    stopInput = false;
    lookDown = false;
    //myAnimator.SetBool("lookDown", lookDown);
    lookUp = false;
    UpdateAnimation();
    //myAnimator.SetBool("lookUp", lookUp);

} 
#endregion

#region Cambio skin per arma indossata

public void ChangeWeaponSkin(int id)
    {

        if(id == 0)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Mano");
            Instantiate(Change, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(4);   
        }else if(id == 1)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Normal"); 
            Instantiate(Change, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(4);   
  
        }
        else if(id == 2)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Rapid");
            Instantiate(Change, transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(4);   
        }else if(id == 3)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Shotgun");
            Instantiate(Change,  transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(4);   

        }else if(id == 4)
        {
            var skeleton = skelGraph.Skeleton;
            skeleton.SetSkin("Bomb");
            Instantiate(Change,  transform.position, transform.rotation);
            AudioManager.instance.PlaySFX(4);   

        }
       
    }

#endregion

#region Pausa
public void OnPause(InputValue value)
//Funzione pausa
{
    if (value.isPressed && !stopInput)
    {
        stopInput = true;
        PauseMenu.gameObject.SetActive(true);
        myAnimator.SetTrigger("idle");
        //SFX.Play(0);
        myRigidbody.velocity = stopMove;
        Time.timeScale = 0f;
    }
    else if(value.isPressed && stopInput)
    {
        stopInput = false;
        Time.timeScale = 1;
        //SFX.Play(0);
        PauseMenu.gameObject.SetActive(false);
    }
}
#endregion

#region Disabilità comandi

public void playerStopInput()
{
    myRigidbody.velocity = stopMove;
    stopInput = true;
}
public void playerActivateInput()
{
    stopInput = false;
}

#endregion

#region Sparo
    void OnFire(InputValue value)
    //Funzione per sparare
    {
        if (!isAlive) { return; }
        if(value.isPressed && !stopInput && !heShoot)
        {
        LookNormal();
        if (Time.time > nextAttackTime && value.isPressed)
        {
        nextAttackTime = Time.time + 1f / attackRate;
        //Se il player è morto si disattiva la funzione
        myAnimator.SetTrigger("isShoot");
        AudioManager.instance.PlaySFX(1);
        Instantiate(blam, gun.position, transform.rotation);
        Instantiate(bullet, gun.position, transform.rotation);
        PlayerBulletCount.instance.removeOneBullet();
        //Richiama una variabile che appare alla posizione del
        //Gameobject gun e viene influenzato nella rotazione
        myRigidbody.velocity = stopMove;
        }
        }
    }    
    
#endregion

#region CambioArma
    public void SetBulletPrefab(GameObject newBullet)
    //Funzione per cambiare arma
    {
       bullet = newBullet;
       LookNormal();
    }    
    
#endregion

#region Movimento
    void OnMove(InputValue value) 
    //Funzione per il movimento
    {
        if (!isAlive) { return; }
         //Se il player è morto si disattiva la funzione
        isMoving = true;
        if(!isKnockBack)
        {    
        moveInput = value.Get<Vector2>();
        }      
        //Dust();
        //Il vettore assume il valore base
    }

#endregion

#region Salto
    void OnJump(InputValue value) 
    {
        
    
        //Se il player non sta toccando il suolo il salto(metodo) finisce
        if(value.isPressed && !platform && isGround)
        //Se il player sta premendo il tasto
        {
            LookNormal();
            myAnimator.SetTrigger("Jump");
            AudioManager.instance.PlaySFX(0);
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
            //Il rigidbody influisce sul vettore sull'asse Y facendo saltare il player
        } 
        else if (value.isPressed && platform)
        {
            myAnimator.SetTrigger("Jump");
            platform = false;
            AudioManager.instance.PlaySFX(0);
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);

        }
        else if(value.isPressed && canDoubleJump)
        {
            myAnimator.SetTrigger("Jump");
            AudioManager.instance.PlaySFX(0);
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
            canDoubleJump = false;
        }
        else if(canWallJumpLeft)
        {
            isWallJumping = true;
            wallJumpDir = new Vector2(1, 1);
        
        }
        else if(canWallJumpRight)
        {
            isWallJumping = true;
            wallJumpDir = new Vector2(-1, 1);
        }
    }

    public void OnAir()
    {
        isGround = false;
        myAnimator.SetTrigger("Jump");
    }

#endregion

#region Corsa
    void Run()
    {
        LookNormal();
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        //Il vettore si muove su input su coordinate X moltiplicata la velocità, mentre l'asse y si basa 
        //sulla velocità del rigidbody
        myRigidbody.velocity = playerVelocity;
        //La velocità del rigidbody diventa quella del player

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //La variabile assume la formula matematica 
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        if(playerHasHorizontalSpeed)
        {        
            AudioManager.instance.PlaySFX(2);
        }
        else if(!playerHasHorizontalSpeed)
        {
            AudioManager.instance.StopSFX(2);
        }
        //L'animatore può attivare il bool basandosi sulla variabile
        if(isWallJumping)
        {
            myRigidbody.velocity = wallJumpDir * jumpSpeed; 
        }

    }
#endregion

#region FlipSprite
    void FlipSprite()
    {
        if(!stopInput && !heShoot)
        {
            //Lo sprite e tutti i parenti si girano dall'altro lato.
            if (myRigidbody.velocity.x < 0)  
            {
                m_FacingRight = true;
                transform.localScale = new Vector2(-1, transform.localScale.y);
                Vector3 theScale = transform.localScale;
                theScale.z = 1;
                transform.localScale = theScale;


            }
            else if (myRigidbody.velocity.x > 0 && m_FacingRight)
            {
               m_FacingRight = false;
               transform.localScale = new Vector2(1, transform.localScale.y);
                Vector3 theScale = transform.localScale;
                theScale.z = 1;
                transform.localScale = theScale;
            }

        /*bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
        //di un valore inferiore a 0
        
        if (playerHasHorizontalSpeed) //Se il player si sta muovendo
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
            //La scala assume un nuovo vettore e il rigidbody sull'asse x 
            //viene modificato mentre quello sull'asse y no. 
           
        }
        }*/
        
    }
    }

#endregion

#region Arrampicarsi
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        //Se il player non si sta arrampicando
        { 
            LookNormal();
            myRigidbody.gravityScale = gravityScaleAtStart;
            //La gravità del rigidbody viene resettata
            myAnimator.SetBool("isClimbing", false);
            myAnimator.SetBool("moveClimb", false);
            isClimbing = false;
            //L'animazione viene disattivata
            return;
            //Il metodo ritorna
        }
        myAnimator.SetBool("isClimbing", true);
        //Se il player si sta arrampicando
        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        //La variabilità dell'arrampicata assume un nuovo vettore
        //l'asse X non subisce alterazione, mentre l'asse y moltiplica il
        //movimento dell'input con la variabile della velocità dell'arrampicata
        myRigidbody.velocity = climbVelocity;
        //La velocità del rigidbody assime quella dell'arrampicata
        myRigidbody.gravityScale = 0f;
        //La gravità del rigidbody diventa 0
        bool playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        //se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
        //di un valore inferiore a 0
        myAnimator.SetBool("moveClimb", playerHasVerticalSpeed);
        isClimbing = true;
        //Si attiva l'animazione dell'arrampicata
    }
#endregion

#region Morte
    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Fall")))
        //Se il player tocca il nemico
        {
            LookNormal();
            isAlive = false;
            stopInput = true;
            //Non è più vivo
            AudioManager.instance.StopSFX(2);
            //GameSession.instance.ProcessPlayerDeath();
            FindObjectOfType<GameSession>().StartDie();
            Player.gameObject.SetActive(false);
            Instantiate(callDie, Player.transform.position, Player.transform.rotation);
            //Parte l'animazione di morte
            //myRigidbody.velocity = knockBack;
            //Il rigidbody assume il valore knockBack 
           
        }
    }

    public void ReactivatePlayer()
    {
        Player.gameObject.SetActive(true);
    }

#endregion

#region Collisioni

void OnCollisionEnter2D(Collision2D other)
    {
        //Se il player tocca il terreno
    if (other.gameObject.tag == "ground" && !isGround)
        {
            isGround = true;
            //Debug.Log("Hai tocca il terreno" + isGround);
            myAnimator.SetBool("isGround", isGround = true);
		}
        //Se il player tocca una piattaforma
        if (other.gameObject.tag == "Platforms")
        {
            isGround = true;
            platform = true;
            //Debug.Log("Hai tocca la paittaforma" + isGround);
            myAnimator.SetBool("isGround", isGround = true);
            if (platform){Player.transform.parent = other.transform;}
		}

        if (other.gameObject.tag == "Enemy")
        {
            //Debug.Log("Hai ricevuto il danno");
            Hurt();
		}
    //Testando il knock back    
        /*if (other.gameObject.tag == "Test")
        {

            //Debug.Log("colpito");
            isKnockBack = true;
            if(m_FacingRight)
            {
            moveInput = knockBackLeft;
            }
            else if(!m_FacingRight)
            {
                moveInput = knockBackRight;
            }
            StartCoroutine(stopKnockback());*/



            //Da usare per teletrasportare il player
            /*
            Debug.Log("colpito");
            isKnockBack = true;
            if(m_FacingRight)
            {
            moveInput = -knockBack;
            }
            else if(!m_FacingRight)
            {
                moveInput = knockBack;
            }
            //myRigidbody.velocity = new Vector2(-10, 10);
            StartCoroutine(stopKnockback());
            }*/
		
	}

    
private void OnCollisionExit2D(Collision2D other){
	if (other.gameObject.tag == "Platforms")
        {
            //Debug.Log("Sei uscito dalla piattaforma");
            platform = false;
			Player.transform.parent = null;
		}
	}
#endregion

#region  KnockBack
public void knockBackLeftDir()
{
            isKnockBack = true;
            moveInput = knockBackLeft;
            StartCoroutine(stopKnockback());
}
public void knockBackRightDir()
{
            isKnockBack = true;
            moveInput = knockBackRight;
            StartCoroutine(stopKnockback());
}

public void knockBackBigLeft()
{
            isKnockBack = true;
            moveInput = knockBackLeftBig;
            StartCoroutine(stopKnockback());
}

public void knockBackBigRight()
{
            isKnockBack = true;
            moveInput = knockBackRightBig;
            StartCoroutine(stopKnockback());
}
private IEnumerator stopKnockback()
{
        yield return new WaitForSeconds(0.2f);
        isKnockBack = false;
        moveInput = stopMove;

}

#endregion

#region Danno
public void Hurt()
{
    LookNormal();
    if(!isInvincible)
    {
    PlayerHealth.instance.removeOneHeart();
    AudioManager.instance.PlaySFX(5);
    StartCoroutine(Invincible());
    myAnimator.SetTrigger("Hurt");
    }
    else if(isInvincible)
    {

    }
}

IEnumerator Invincible()
{
    isInvincible = true;
    yield return new WaitForSeconds(timeInvincible);
    isInvincible = false;
}
#endregion



}

