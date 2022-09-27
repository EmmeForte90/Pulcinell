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

        }
}
