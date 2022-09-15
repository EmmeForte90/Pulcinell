﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] float bulletSpeed = 20f;
    //Variabile della velocità del proiettile
    [SerializeField] GameObject Explode;
    [SerializeField] Transform prefabExp;
    Rigidbody2D myRigidbody;
    //Il corpo rigido
    PlayerMovement player;
    //Attribuscie una variabile allo script di movimento del player
    //Per permettere al proiettile di emularne l'andamento
    float xSpeed;
    //L'andatura

    [Header("Che tipo di bullet")]
    [SerializeField] bool isNormal;
    [SerializeField] bool isRapid;
    [SerializeField] bool isBomb;
    [SerializeField] bool target;
    [SerializeField] bool isShotgun;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //Recupera i componenti del rigidbody
        player = FindObjectOfType<PlayerMovement>();
        //Recupera i componenti dello script
        xSpeed = player.transform.localScale.x * bulletSpeed;
        //La variabile è uguale alla scala moltiplicata la velocità del proiettile
        //Se il player si gira  anche lo spawn del proittile farà lo stesso
    }

#region Update
    void Update()
    {
         myRigidbody.velocity = new Vector2 (xSpeed, 0f);
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
        if(other.gameObject.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Explode, transform.position, transform.rotation);
            //CameraShake.Shake(0.10f, 0.50f);
            if(isNormal && !isRapid )
            {
                Destroy(gameObject);
                Destroy(other.gameObject);
            }
            else if(!isNormal && isRapid)
            {
                Destroy(other.gameObject);
            }
            //Destroy(other.gameObject);
            //Viene distrutto
        }

        if(other.gameObject.tag == "ground")
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Explode, transform.position, transform.rotation);
            //CameraShake.Shake(0.10f, 0.50f);
            Destroy(gameObject);
            //Viene distrutto
        }
        
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);   
        //Se il proiettile tocca una superficie viene distrutto 
    }

}