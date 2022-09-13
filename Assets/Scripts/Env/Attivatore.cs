using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Attivatore : MonoBehaviour
{
    public TilemapRenderer render;
    public GameObject attivatore;
    public GameObject Light1;
    public GameObject Light2; 
    public GameObject Light3;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {




    }

    
    //Se il giocatore tocca l'area...
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Color faded = GetComponent<SpriteRenderer>().color;
            //faded.a = 0.1f;
            //GetComponent<SpriteRenderer>().color = faded;

            render.enabled = false;
            Light1.gameObject.SetActive(true);
            Light2.gameObject.SetActive(true);
            Light3.gameObject.SetActive(true);

        }
    }

    //Se il giocatore esce dall'area...
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
           

            render.enabled = true;
            Light1.gameObject.SetActive(false);
            Light2.gameObject.SetActive(false);
            Light3.gameObject.SetActive(false);


        }
    }

}


