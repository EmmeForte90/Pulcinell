using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    //Velocità del nemico
    Rigidbody2D myRigidbody;
    //Il suo rigidbody
    
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        //Recupero dei componenti del rigidbody
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (moveSpeed, 0f);
        //Velocità di andatura sull'asse X
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        moveSpeed = -moveSpeed;
        //Quando il nemico tocca una superficie il suo movimento diventa negativo
        FlipEnemyFacing();
        //Richiama la funzione per flippare il nemico
    }

    void FlipEnemyFacing()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
        //Flippa il nemico e ne corregge l'andatura
    
    }
}
