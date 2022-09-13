using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.collider.tag == "Player")
        {
            PlayerHealthController.instance.Dealdamage();

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if(other.tag == "Player")
        {

            PlayerHealthController.instance.Dealdamage();


        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            PlayerHealthController.instance.Dealdamage();
            



        }


    }

    public IEnumerator Counterdamage()
    {
        PlayerHealthController.instance.Dealdamage();


        yield return new WaitForSeconds(1.5f);

    }

}
