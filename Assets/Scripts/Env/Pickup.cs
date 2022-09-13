using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{

    public bool isPizza, isHeal, isBullet;

    private bool isCollected;

    public GameObject pickupEffect;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //Quando il player tocca l'oggetto
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isCollected)
        {

            //Se è una pizza
            if (isPizza)
            {
                //Colleziona l'oggetto
                LevelManager.instance.pizzaCollected++;

                isCollected = true;
                Destroy(gameObject);


                Instantiate(pickupEffect, transform.position, transform.rotation);

                UIController.instance.UpdatePizzaCount();

                AudioManager.instance.PlaySFX(6);

            }

            //Se è una cura
            if (isHeal)
            {
                if (PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
                {
                    //Ripristina gli hp
                    PlayerHealthController.instance.HealPlayer();

                    isCollected = true;
                    Destroy(gameObject);

                    Instantiate(pickupEffect, transform.position, transform.rotation);

                    AudioManager.instance.PlaySFX(7);
                }
            }
            //Se è una munizione
            if (isBullet)
            {
                if (PlayerHealthController.instance.currentBullet != PlayerHealthController.instance.maxBullet)
                {
                    //Ripristana le munizioni
                    PlayerHealthController.instance.RechargeBull();

                    isCollected = true;
                    Destroy(gameObject);

                    Instantiate(pickupEffect, transform.position, transform.rotation);

                    AudioManager.instance.PlaySFX(7);
                }


            }

        }
    }
}
