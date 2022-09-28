using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
     void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        //Se il proiettile tocca il nemico
        {            
            PlayerMovement.instance.Hurt();
        }
        if(other.gameObject.tag == "Test")
        //Se il proiettile tocca il nemico
        {            
            Debug.Log("Ha colpito");
        }

    }

   /* void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player")
        //Se il proiettile tocca il nemico
        {            
            PlayerMovement.instance.Hurt();
        }
    }*/
}
