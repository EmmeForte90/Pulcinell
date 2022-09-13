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
    [SerializeField] float runSpeed = 10f; 
    // Variabile per il valore della corsa
    [SerializeField] float jumpSpeed = 5f;
    // Variabile per il valore della velocità del salto
    [SerializeField] float climbSpeed = 5f;
    //Variabile per la velocità di arrampicata
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] GameObject bullet;
    // Variabile per il gameobject del proiettile
    [SerializeField] Transform gun;
    //Variabile per spostare o scalare l'oggetto
    
    Vector2 moveInput; 
    //Variabile per il vettore che serve al player per muoversi
    Rigidbody2D myRigidbody;
    //Variabile per il rigidbody
    Animator myAnimator;
    //Variabile per l'animator 
    CapsuleCollider2D myBodyCollider;
    //Variabile per il capsule collider 
    BoxCollider2D myFeetCollider;
    //Variabile per il box collider
    float gravityScaleAtStart;
    //Variabile per la gravità
    [SerializeField] public GameObject PauseMenu;
    bool isAlive = true;
    //Variabile per identificare la morte
    bool stopInput = false;
    //Blocca i movimenti
    bool heShoot = false;
    private Vector2 stopMove = new Vector2 (0f, 0f);



    [Header("Attacco")]
    [SerializeField] float attackRate = 2f;
    [SerializeField] float attackrange = 0.5f;
    [SerializeField] float nextAttackTime = 0f;


#region Start
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //Recupera i componenti del rigidbody
        myAnimator = GetComponent<Animator>();
        //Recupera i componenti dell'animator
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        //Recupera i componenti del capsule collider
        myFeetCollider = GetComponent<BoxCollider2D>();
        //Recupera i componenti del box collider
        gravityScaleAtStart = myRigidbody.gravityScale;
        //Le dimensioni della gravità diventano quelle del rigidbody
    }
#endregion

#region Update
    void Update()
    {
        if (!isAlive) { return; }
        //Se il player è morto si disattiva la funzione
        //Altrimenti si attivano le funzioni
        Run();
        FlipSprite();
        ClimbLadder();
        Die();
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
        myRigidbody.velocity = stopMove;
        Time.timeScale = 0f;
    }
    else if(value.isPressed && stopInput)
    {
        stopInput = false;
        Time.timeScale = 1;
        PauseMenu.gameObject.SetActive(false);
    }
}
#endregion

#region Sparo
    void OnFire(InputValue value)
    //Funzione per sparare
    {
        if (!isAlive) { return; }
        if(value.isPressed && !stopInput && !heShoot)
        {
        if (Time.time > nextAttackTime && value.isPressed)
        {
        nextAttackTime = Time.time + 1f / attackRate;
        //Se il player è morto si disattiva la funzione
        myAnimator.SetTrigger("isShoot");
        Instantiate(bullet, gun.position, transform.rotation);
        //Richiama una variabile che appare alla posizione del
        //Gameobject gun e viene influenzato nella rotazione
        myRigidbody.velocity = stopMove;
        }
        }
    }    
    
#endregion

#region Movimento
    void OnMove(InputValue value) 
    //Funzione per il movimento
    {
        if (!isAlive) { return; }
         //Se il player è morto si disattiva la funzione
        moveInput = value.Get<Vector2>(); 
        //Il vettore assume il valore base
    }

#endregion

#region Salto
    void OnJump(InputValue value) //La funzione si attiva se si preme il tasto
    {
        if (!isAlive) { return; }
         //Se il player è morto si disattiva la funzione
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) { return;}
        //Se il player non sta toccando il suolo il salto(metodo) finisce
        if(value.isPressed)
        //Se il player sta premendo il tasto
        {
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
            //Il rigidbody influisce sul vettore sull'asse Y facendo saltare il player
        }
    }

#endregion

#region Corsa
    void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        //Il vettore si muove su input su coordinate X moltiplicata la velocità, mentre l'asse y si basa 
        //sulla velocità del rigidbody
        myRigidbody.velocity = playerVelocity;
        //La velocità del rigidbody diventa quella del player

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //La variabile assume la formula matematica 
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
        //L'animatore può attivare il bool basandosi sulla variabile

    }
#endregion

#region  FlipSprite
    void FlipSprite()
    {
        if(!stopInput && !heShoot)
        {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
        //di un valore inferiore a 0

        if (playerHasHorizontalSpeed) //Se il player si sta muovendo
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
            //La scala assume un nuovo vettore e il rigidbody sull'asse x 
            //viene modificato mentre quello sull'asse y no. 
        }
        }
    }

#endregion

#region Arrampicarsi
    void ClimbLadder()
    {
        if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) 
        //Se il player non si sta arrampicando
        { 
            myRigidbody.gravityScale = gravityScaleAtStart;
            //La gravità del rigidbody viene resettata
            myAnimator.SetBool("isClimbing", false);
            //L'animazione viene disattivata
            return;
            //Il metodo ritorna
        }
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
        myAnimator.SetBool("isClimbing", playerHasVerticalSpeed);
        //Si attiva l'animazione dell'arrampicata
    }
#endregion

#region Morte
    void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        //Se il player tocca il nemico
        {
            isAlive = false;
            //Non è più vivo
            myAnimator.SetTrigger("Dying");
            //Parte l'animazione di morte
            myRigidbody.velocity = deathKick;
            //Il rigidbody assume il valore death kick
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            //Richiama i componenti dello script gamesessione e 
            //ne attiva la funzione di processo di morte 
        }
    }
#endregion
}