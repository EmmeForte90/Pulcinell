using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject objectSwitch;
    private SpriteRenderer theSR;
    public Sprite downSprite;

    private bool hasSwitched;

    public bool deactivateOnSwitch;


    // Start is called before the first frame update
    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();


    }

    // Update is called once per frame
    void Update()
    {
        


    }

    //Quando il player tocca l'oggetto
    private void OnTriggerEnter2D(Collider2D other)
    {
        

        //E se l'oggetto è falso
        if(other.tag == "Player"  && !hasSwitched)
        {

            //Disattiva
            if (deactivateOnSwitch)
            {

                objectSwitch.SetActive(false);
            }
            //Altrimenti attiva
            else
            {
                objectSwitch.SetActive(true);
            }


            theSR.sprite = downSprite;
            hasSwitched = true;
        }


    }


}
