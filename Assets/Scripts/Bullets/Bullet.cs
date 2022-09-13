using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed = 20f;
    //Variabile della velocità del proiettile
    Rigidbody2D myRigidbody;
    //Il corpo rigido
    PlayerMovement player;
    //Attribuscie una variabile allo script di movimento del player
    //Per permettere al proiettile di emularne l'andamento
    float xSpeed;
    //L'andatura
    
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

    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
        //La velocità e la direzione del proiettile
    }

    void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Enemy")
        //Se il proiettile tocca il nemico
        {
            Destroy(other.gameObject);
            //Viene distrutto
        }
        Destroy(gameObject);
        //Viene distrutto comunque
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
        Destroy(gameObject);   
        //Se il proiettile tocca una superficie viene distrutto 
    }

}
