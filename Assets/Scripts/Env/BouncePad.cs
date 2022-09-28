using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] public float bounceForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }


    //Se il giocatore tocca il bersaglio
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Una forza che settiamo con una variabile lo spinge sull'asse Y
            //PlayerMovement.instance.OnAir();
            PlayerMovement.instance.myRigidbody.velocity = new Vector2(PlayerMovement.instance.myRigidbody.velocity.x, bounceForce);
            anim.SetTrigger("bounce");
        }
    }
    
    private void OnColliderEnter2D(Collider2D other)
    {
       if(other.tag == "Player")
        {
            //Una forza che settiamo con una variabile lo spinge sull'asse Y
            //PlayerMovement.instance.OnAir();
            PlayerMovement.instance.myRigidbody.velocity = new Vector2(PlayerMovement.instance.myRigidbody.velocity.x, bounceForce);
            anim.SetTrigger("bounce");
        }
    }

}
