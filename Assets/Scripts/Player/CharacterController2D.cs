using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[SerializeField] protected float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] protected float m_CrouchSpeed = 2f;          // Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, .3f)] [SerializeField] protected float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] protected bool m_AirControl = false;                         // Se un giocatore può o meno controllare mentre salta;
	[SerializeField] protected LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] protected Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] protected Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] protected Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching
	[SerializeField] protected private Animator anim;

	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;            // Whether or not the player is grounded.
	public bool ceiling;
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	public float Soffitto;

	public Rigidbody2D m_Rigidbody2D;
	public bool m_FacingRight = false;  // For determining which way the player is currently facing.
	protected Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	public BoolEvent OnCrouchEvent;
	protected bool m_wasCrouching = false;

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();

		if (OnCrouchEvent == null)
			OnCrouchEvent = new BoolEvent();
	}

	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// Il giocatore viene messo a terra se un circlecast nella posizione di groundcheck colpisce qualcosa designato come ground
		// Questo può essere fatto utilizzando invece i livelli ma le risorse campione non sovrascriveranno le impostazioni del progetto.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
	}


	public void Move(float move, bool crouch, bool jump)
	{


		// Se accovacciato, controlla se il personaggio può alzarsi

		if (!crouch)
		{


			// Se il personaggio ha un soffitto che gli impedisce di alzarsi, tienili accovacciati
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround))
			{
				
				Debug.Log("touch soffitto!");
				anim.SetBool("isCrouch", crouch);
				anim.SetBool("isCeiling", ceiling);
				crouch = true;
			}
		}

		//Controllo del player se è in aria
		if (m_Grounded || m_AirControl)
		{

			// Se è abbassato
			if (crouch)
			{
				if (!m_wasCrouching)
				{
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}

				// Riduce la velocità di quando è abbassato
				move *= m_CrouchSpeed;

				// Disabilità collisione quando è abbassato
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = false;
			}
			else
			{
				// Abilità collisione quando non è abbassato
				if (m_CrouchDisableCollider != null)
					m_CrouchDisableCollider.enabled = true;

				if (m_wasCrouching)
				{
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
			}

			// Muove il pg in base alla velocità
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// Poi l'applica al personaggio
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// se il player non guarda a destra
			if (move > 0 && !m_FacingRight)
			{
				// gira il player
				Flip();
			}
               //altrimenti gira a sinistra
			else if (move < 0 && m_FacingRight)
			{
				// gira il player
				Flip();
			}
		}
		// Se il player salta
		if (m_Grounded && jump)
		{
			// Aggiungi una forza verticale al pg
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}
	}


	public void Flip()
	{
		// Cambia la direzione
		m_FacingRight = !m_FacingRight;

		// Rotazione secondo la scala X
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		theScale.z = 1;
		transform.localScale = theScale;
	}


}