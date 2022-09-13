﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTankMine : MonoBehaviour
{
    public GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            {

            AudioManager.instance.PlaySFX(3);

            Destroy(gameObject);

            Instantiate(explosion, transform.position, transform.rotation);

            PlayerHealthController.instance.Dealdamage();

        }


    }

    public void Explode()
    {
        AudioManager.instance.PlaySFX(3);

        Destroy(gameObject);

        Instantiate(explosion, transform.position, transform.rotation);

    }


}
