using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] float bulletSpeed = 20f;
    [SerializeField] float shotgunBullet = 3f;
    [SerializeField] float bombBullet = 10f;
    //Variabile della velocità del proiettile
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    private float lifeTime = 0.5f;
    //Riservato allo shotgun

    Rigidbody2D myRigidbody;
    //Il corpo rigido
    AIEnemyDefault Enemies;
    //Attribuscie una variabile allo script di movimento del player
    //Per permettere al proiettile di emularne l'andamento
    float xSpeed;
    float shotgunSpeed;
    //L'andatura
    float bombSpeed = 5f;
    public Vector3 LauchOffset;
    //Riservato alla bomb

    [Header("Che tipo di bullet")]
    [SerializeField] bool isNormal;
    [SerializeField] bool isRapid;
    [SerializeField] bool isBomb;
    [SerializeField] bool target;
    [SerializeField] bool isShotgun;
    [SerializeField] bool rightFace;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //Recupera i componenti del rigidbody
        Enemies = FindObjectOfType<AIEnemyDefault>();
        //Recupera i componenti dello script
        xSpeed = Enemies.transform.localScale.x * bulletSpeed;
        shotgunSpeed = Enemies.transform.localScale.x * shotgunBullet;
        //La variabile è uguale alla scala moltiplicata la velocità del proiettile
        //Se il player si gira  anche lo spawn del proittile farà lo stesso
        
        if(isBomb)
        {        
            if(AIEnemyDefault.instance.transform.localScale.x > 0)
            {
            var direction = transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * bombSpeed, ForceMode2D.Impulse);
            }
            else if(AIEnemyDefault.instance.transform.localScale.x < 0)
            {
            var direction = -transform.right + Vector3.up;
            GetComponent<Rigidbody2D>().AddForce(direction * bombSpeed, ForceMode2D.Impulse);
            }
        }
        transform.Translate(LauchOffset);
        
    }




#region Update
    void Update()
    {
        if(isNormal || isRapid && !isShotgun && !isBomb)
        {
         myRigidbody.velocity = new Vector2 (xSpeed, 0f);
        }
        else if(!isNormal && !isRapid && isShotgun && !isBomb)
        {
        myRigidbody.velocity = new Vector2 (shotgunSpeed, 0f);
        }
        else if(!isNormal && !isRapid && !isShotgun && isBomb)
        {

        }
        //La velocità e la direzione del proiettile
        FlipSprite();
        
    }
#endregion
 

#region  FlipSprite
    void FlipSprite()
    {
        bool bulletHorSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        //se il player si sta muovendo le sue coordinate x sono maggiori di quelle e
        //di un valore inferiore a 0

        if (bulletHorSpeed) //Se il player si sta muovendo
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);
            //La scala assume un nuovo vettore e il rigidbody sull'asse x 
            //viene modificato mentre quello sull'asse y no. 
        }
        
        
    }

#endregion


    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Explode, transform.position, transform.rotation);
            //CameraShake.Shake(0.10f, 0.50f);
            if(isNormal && !isRapid )
            //Se è un proiettile normale e non rapido
            {
            Destroy(gameObject);
            PlayerMovement.instance.Hurt();

            }
            else if(!isNormal && isRapid)
            //Quando è un proiettile rapido e non normale
            {
            PlayerMovement.instance.Hurt();

            }
            else if(isBomb)
            //Quando è un proiettile rapido e non normale
            {
            //Debug.Log("Hit enemy");
            Destroy(gameObject);
            PlayerMovement.instance.Hurt();

            }
            else if(isShotgun)
            //Quando è un proiettile rapido e non normale
            {
            PlayerMovement.instance.Hurt();

            }
        }

        if(other.gameObject.tag == "ground" && !isShotgun)
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Explode, transform.position, transform.rotation);
            //CameraShake.Shake(0.10f, 0.50f);
            Destroy(gameObject);
            //Viene distrutto
        }
        else if(isShotgun)
        {
            Invoke("Destroy", lifeTime);
        }
        
    }

    void Destroy()
    {
        Destroy(gameObject);   
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);   
        //Se il proiettile tocca una superficie viene distrutto 
    }

}