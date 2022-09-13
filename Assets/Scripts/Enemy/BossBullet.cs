using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{

    public GameObject explosion;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(2);

    }

    // Update is called once per frame
    void Update()
    {
        //Aggiornamento della posizione
        transform.position += new Vector3(-speed * transform.localScale.x * Time.deltaTime, 0f, 0f);
    }

    //Quando tocca il player...
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //...Subisce danno ed esplode
            PlayerHealthController.instance.Dealdamage();
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);
        }

        AudioManager.instance.PlaySFX(1);
       
        //...Altrimenti se colpisce lo scenario esplode comunque
        if (other.tag == "Ground")
        {
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(gameObject);

        }


        

    }


}
