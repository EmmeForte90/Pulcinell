using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    [SerializeField] float timeHurt;

     void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        //Se il proiettile tocca il nemico
        {            
            StartCoroutine(Counterdamage());
        }
        if(other.gameObject.tag == "Test")
        //Se il proiettile tocca il nemico
        {            
            //Debug.Log("Ha colpito");
        }

    }

    IEnumerator Counterdamage()
    {
        yield return new WaitForSeconds(timeHurt);
        PlayerMovement.instance.Hurt();
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
