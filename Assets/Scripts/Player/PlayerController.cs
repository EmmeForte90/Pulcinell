using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usa le API di unity di base
using UnityEngine.InputSystem;
//Usa l'input system di Unity scaricato dal packet manager
using Spine.Unity;
using Spine;

public class PlayerController : MonoBehaviour
{
[Header("Movimento")]


    [SerializeField] public static PlayerController instance;
    [SerializeField] public float moveSpeed;
    float horizontalMove;
     // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] protected float m_CrouchSpeed = 2f;          
    // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] protected float m_MovementSmoothing = .05f;  
    // How much to smooth out the movement

[Header("Salto")]
    [SerializeField] public float wallJumpTime;
    [SerializeField] public float wallSlideSpeed;
    [SerializeField] public float wallDistance;
    [SerializeField] protected float m_JumpForce = 400f;                         
    bool isWallSliding = false;
    bool isWallJump = false;
    float jumpTime;
   

[Header("Attacco")]
    [SerializeField] public float attackRate = 2f;
    [SerializeField] public float attackrange = 0.5f;
    float nextAttackTime = 0f;
    private readonly int AttackDamage = 1;
    [Range(0, 100)] [SerializeField] public float chanceToDrop;
    [SerializeField] public GameObject collectible;

[Header("Abilitazioni")]
    private bool doubleJump = false;
    private bool canDoubleJump = true;
    private bool caanstomp;
    private bool crouch = false;
    private bool jump = false;
    [SerializeField] public float jumpForce;
    [SerializeField] public float knockBackLegth, knockBackForce;
    private float knockBackCounter;
    [SerializeField] protected bool m_AirControl = false;                        
     // Se un giocatore può o meno controllare mentre salta;
    [SerializeField] public float bouceForce;
    [SerializeField] public bool stopInput;
    private bool wasonground;

    
[Header("Particles")]  
    [SerializeField] public ParticleSystem footsteps;
    private ParticleSystem.EmissionModule footEmission;
    [SerializeField] public ParticleSystem impactEffect;

[Header("Detector")]

    [SerializeField] public Transform firePoint;
    [SerializeField] public Transform attackPoint;
    [SerializeField] public LayerMask enemyLayers;
    [SerializeField] public GameObject bullet;
    [SerializeField] protected LayerMask m_WhatIsGround;                         
    // A mask determining what is ground to the character
	[SerializeField] protected Transform m_GroundCheck;                          
     // A position marking where to check if the player is grounded.
	[SerializeField] protected Transform m_CeilingCheck;                          
    // A position marking where to check for ceilings
	[SerializeField] protected Collider2D m_CrouchDisableCollider;               
     // A collider that will be disabled when crouching
    RaycastHit2D wallCheckHit;



[Header("Animazioni")]

    [SerializeField] public SpriteRenderer theSR;
    [SerializeField] public Transform player;
    [SerializeField] protected private Animator anim;
	const float k_GroundedRadius = .2f; 
    // Radius of the overlap circle to determine if grounded
	[SerializeField]public bool m_Grounded;           
    // Whether or not the player is grounded.
    protected bool m_wasCrouching = false;
	[SerializeField] public bool ceiling;
	const float k_CeilingRadius = .2f; 
    // Radius of the overlap circle to determine if the player can stand up
	[SerializeField] public float Soffitto;
	[SerializeField] public Rigidbody2D m_Rigidbody2D;
	[SerializeField] public bool m_FacingRight = false;  
    // For determining which way the player is currently facing.
	protected Vector3 m_Velocity = Vector3.zero;


/*--------------------------------------------------------------------*/

private void Awake()
    {
        instance = this;
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        theSR = GetComponent<SpriteRenderer>();
        footEmission = footsteps.emission;
    }


/*--------------------------------------------------------------------*/    

    // Update is called once per frame
void Update()
{
//Se il gioco è in pausa e l'input dei comandi è bloccato
if (!PauseMenu.instance.isPaused && !stopInput)
    { //Se non è in pausa e l'input è abilitato

#region Input

            if (knockBackCounter <= 0)
            {
                m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, .2f, m_WhatIsGround);

                //Tasto abbassato

                if (m_Grounded)
                {
                    if (Input.GetButtonDown("Crouch"))
                    {
                        crouch = true;
                        canDoubleJump = false;
                    }

                    if (Input.GetButtonUp("Crouch"))
                    {
                        crouch = false;
                    }



                } else if (Input.GetButtonDown("Crouch") && !m_Grounded)
                {}


                //Tasto attacco
                if (Time.time > nextAttackTime && Input.GetButtonDown("Fire1"))
                {
                    nextAttackTime = Time.time + 1f / attackRate;

                    Punchhitbox();
                }


                //Tasto sparo
                if (Time.time > nextAttackTime && Input.GetButtonDown("Blast"))
                {
                    nextAttackTime = Time.time + 1f / attackRate;

                    Fire();
                }


                //Se è a terra...
                if (m_Grounded)
                {
                    canDoubleJump = true;
                }

                //Può saltare o effettuare un walljump
                if (Input.GetButtonDown("Jump") || isWallSliding && Input.GetButtonDown("Jump"))
                {
                    if (m_Grounded || isWallSliding)
                    {
                        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
                        AudioManager.instance.PlaySFX(10);
                        jump = true;
                    }
                    else
                    {
                        if (canDoubleJump)
                        {
                            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, jumpForce);
                            canDoubleJump = false;
                            AudioManager.instance.PlaySFX(10);
                        }
                    }
                }
            }
            else
            {
                //Se subisce danno indietreggia 
                knockBackCounter -= Time.deltaTime;
                if (m_FacingRight)
                {
                    m_Rigidbody2D.velocity = new Vector2(-knockBackForce, m_Rigidbody2D.velocity.y);
                }
                else
                {
                    m_Rigidbody2D.velocity = new Vector2(knockBackForce, m_Rigidbody2D.velocity.y);
                }
            }
        }
        #endregion

/*--------------------------------------------------------------------*/

#region Movimento Asse X
        if (stopInput)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
        }
        else if (!stopInput)
        {
            m_Rigidbody2D.velocity = new Vector2(moveSpeed * Input.GetAxisRaw("Horizontal"), m_Rigidbody2D.velocity.y);
            
            if (m_Rigidbody2D.velocity.x < 0) 
            //Lo sprite e tutti i parenti si girano dall'altro lato. Molto utile per gli hitbox
            {
                m_FacingRight = true;
                transform.localScale = new Vector2(-2, transform.localScale.y);
                Vector3 theScale = transform.localScale;
                theScale.z = 1;
                transform.localScale = theScale;
            }
            else if (m_Rigidbody2D.velocity.x > 0 && m_FacingRight)
            {
               m_FacingRight = false;
               transform.localScale = new Vector2(2, transform.localScale.y);
                Vector3 theScale = transform.localScale;
                theScale.z = 1;
                transform.localScale = theScale;
            }
        }
        
        UpdateAnimator();
    }
    #endregion

/*--------------------------------------------------------------------*/

public void FixedUpdate()
{
#region Polvere

        if (Input.GetAxisRaw("Horizontal") != 0 && m_Grounded)
        {
            footEmission.rateOverTime = 35f;
        } else
        {
            footEmission.rateOverTime = 0f;
        }

        if (!wasonground && m_Grounded)
        {
            impactEffect.gameObject.SetActive(true);
            impactEffect.Stop();
            impactEffect.transform.position = footsteps.transform.position;
            impactEffect.Play();
        }

        wasonground = m_Grounded;
        #endregion

#region Abbassarsi

    bool wasGrounded = m_Grounded;
    m_Grounded = false;

/* Il giocatore viene messo a terra se un circlecast nella posizione di groundcheck colpisce qualcosa designato 
come ground, questo può essere fatto utilizzando invece i livelli ma le risorse campione non sovrascriveranno 
le impostazioni del progetto.*/
    Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					{//OnLandEvent.Invoke();
                    }
			}
		}
        if (crouch)
        {
            if (!m_wasCrouching)
            {
                m_wasCrouching = true;
                {//OnCrouchEvent.Invoke(true);
                }
            }
            //Riduce la velocità quando è abbassato
            m_Rigidbody2D.velocity *= m_CrouchSpeed;
            //Disabilità il collider totale
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = false;
        } else
        {
            if (m_CrouchDisableCollider != null)
                m_CrouchDisableCollider.enabled = true;
            if (m_wasCrouching)
            {
                m_wasCrouching = false;
                //OnCrouchEvent.Invoke(false);
            }
        }
        jump = false;
        #endregion

/*--------------------------------------------------------------------*/        

//Area di calcoli da rivedere(Il player non resta abbassato quando tocca il soffitto)
#region Flip Sprite
        if (!m_FacingRight)// il lato in cul il personaggio vede il muro e mostra lo sprite
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(wallDistance, 0), wallDistance, m_WhatIsGround);
            Debug.DrawRay(transform.position, new Vector2(wallDistance, 0), Color.blue);
            //Raycast soffitto
            Physics2D.Raycast(transform.position, new Vector2(0, Soffitto), Soffitto, m_WhatIsGround);
            Debug.DrawRay(transform.position, new Vector2(0, Soffitto), Color.blue);
        }
        else
        {
            wallCheckHit = Physics2D.Raycast(transform.position, new Vector2(-wallDistance, 0), wallDistance, m_WhatIsGround);
            Debug.DrawRay(transform.position, new Vector2(-wallDistance, 0), Color.blue);
            //Raycast soffitto

            Physics2D.Raycast(transform.position, new Vector2(0, Soffitto), Soffitto, m_WhatIsGround);
            Debug.DrawRay(transform.position, new Vector2(0, Soffitto), Color.blue);

        }
        //Quando si verificano queste condizioni avviene il walljump (Rilevato muro, il pg non è a terra, il movimento orizontale è a zero)
        if (wallCheckHit && !m_Grounded && m_Rigidbody2D.velocity.x != 0) 
        {
            isWallJump = false;
            isWallSliding = true;
            jumpTime = Time.time + wallJumpTime;
        }
        else if (jumpTime < Time.time)
        {
            isWallSliding = false;
            isWallJump = true;
        }
        //Scivola sul muro
        if (isWallSliding) 
        {
            m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, Mathf.Clamp(m_Rigidbody2D.velocity.y, wallSlideSpeed, float.MaxValue));
        }
        UpdateAnimator();
#endregion

#if UNITY_EDITOR


        if (Input.GetKeyDown(KeyCode.M))
        {
            knockBack();


        }
}
#endif
//Database per le animazioni
#region Animatore
private void UpdateAnimator()
    {
        anim.SetFloat("moveSpeed", Mathf.Abs(m_Rigidbody2D.velocity.x));
        anim.SetBool("isGrounded", m_Grounded);
        anim.SetBool("doubleJump", canDoubleJump);
        anim.SetBool("isCrouch", crouch);
        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isWallJump", isWallJump);

    }
#endregion
//Quando subisce danno il player indietreggia

#region Salto per danno
public void knockBack()
    {
        knockBackCounter = knockBackLegth;
        if(m_FacingRight)
        {
            m_Rigidbody2D.AddForce(new Vector2(knockBackForce, 1));
           // m_Rigidbody2D.GetComponent<Rigidbody2D>().velocity = new Vector2 (knockBackForce, 2);
        }
        else if(!m_FacingRight)
        {
            m_Rigidbody2D.AddForce(new Vector2(-knockBackForce, 1));
        }
        anim.SetTrigger("isHurt");
    }
#endregion

//Quando è un nemico particolare il player viene sbalzato parecchio indietro (DA SISTEMARE)
#region Salto per danno da nemico grosso
public void knockBackgigafat()
    {
        knockBackCounter = knockBackLegth;
        if (m_FacingRight)
        {
            m_Rigidbody2D.AddForce(new Vector2(5000f, 1));
        }

        else if (!m_FacingRight)
        {
            m_Rigidbody2D.AddForce(new Vector2(-5000f, 1));
        }
        anim.SetTrigger("isHurt");
    }
#endregion
/*--------------------------------------------------------------------*/

//Quando il player spara
#region Attacco
private void Fire()

    {
        if (PlayerHealthController.instance.currentBullet > 0)
        {
            //viene tolta una munizione
            PlayerHealthController.instance.currentBullet--;
            UIController.instance.UpdateBulletDisplay();
            var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
            newBullet.transform.localScale = player.localScale;
            if (m_Grounded)
            {
                anim.SetTrigger("isFire");
            }
            else
            {
                anim.SetTrigger("isFireinAir");
            }
            AudioManager.instance.PlaySFX(3);
        } else if (PlayerHealthController.instance.currentBullet <= 0)
        {
            if (m_Grounded)
            {
                anim.SetTrigger("isFire");
            }
        }
    }

    //Quando il player attacca
    private void Punchhitbox()
    {
        anim.SetTrigger("isAttack");
        Collider2D[] HIT = Physics2D.OverlapCircleAll(attackPoint.position, attackrange, enemyLayers);
        foreach (Collider2D Enemy in HIT)
        {
            Debug.Log("We hit");
            #region Calcolo del danno
            IDamegable hit = Enemy.GetComponent<IDamegable>();
            hit.Damage();
            #endregion

            CameraShake.Shake(0.10f, 1f);
            //Probabilità che il nemico droppi un oggetto
            float dropSelect = Random.Range(0, 100f);
            if (dropSelect <= chanceToDrop)
            {
                Instantiate(collectible, Enemy.transform.position, Enemy.transform.rotation);
            }
            AudioManager.instance.PlaySFX(3);
        }

        //Quando il player colpisce un oggetto
        foreach (Collider2D Box in HIT)
        {
            Debug.Log("We hit");
            BreakEvairoment.instance.Dealdamage();
            float dropSelect = Random.Range(0, 100f);
            if (dropSelect <= chanceToDrop)
            {
                Instantiate(collectible, Box.transform.position, Box.transform.rotation);
            }

            AudioManager.instance.PlaySFX(3);
        }
    }
#endregion

//Quando il player rimbalza
#region Rimbalzo

public void Bounce()
    {
        m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, bouceForce);
        AudioManager.instance.PlaySFX(10);
    }
    #endregion

//Quando il player sale su delle piattaforme    
#region Piattaforme
private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform") 
        {
            m_Grounded = true;
            transform.parent = other.transform;
        }
        if (other.gameObject.tag == "Enemy")
        {
            knockBack();
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform")
        {
            transform.parent = null;
            m_Grounded = false;
            
        }
    }
    #endregion
}
