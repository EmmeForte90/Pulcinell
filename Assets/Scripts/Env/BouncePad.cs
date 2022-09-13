using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private Animator anim;

    public float bounceForce = 20f;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Se il giocatore tocca il bersaglio
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Una forza che settiamo con una variabile lo spinge sull'asse Y
            //PlayerController.instance.m_Rigidbody2D.velocity = new Vector2(PlayerController.instance.m_Rigidbody2D.velocity.x, bounceForce);
            anim.SetTrigger("Bounce");
        }
    }


}
