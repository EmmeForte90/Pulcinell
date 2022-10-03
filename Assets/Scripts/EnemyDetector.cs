using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    //Rilevatore nemico, quando il player entra nel raggio d'azione di questo rilevatore il nemico inizia a sparare
    [Header("Rilevatore")]
    [SerializeField]
    public Transform Detector;
    //AIEnemyGun enGun;
    //AIEnemyDefault enDefault;
    [SerializeField]
    public bool shooter;

//Il player entra nel raggio d'azione del nemico
    private void OnTriggerStay2D(Collider2D other)
    {
        //waitaftershot();
        if (other.tag == "Player")
        {
            //Se è un nemico che spara attiva le funzioni di sparo come l'animazione
            if(shooter)
            {
            FindObjectOfType<AIEnemyGun>().Shoot();

            }
            
            if (other.tag == "Test")
        {
            //Se è un nemico che non spara attiva le funzioni di attacco come l'animazione
            FindObjectOfType<AIEnemyDefault>().Attack();
        }
            /*//Se è un nemico che non spara attiva le funzioni di attacco come l'animazione
            else if(!shooter)
            {
                FindObjectOfType<AIEnemyDefault>().Attack();
            }*/
        }
    }

private void OnTriggerEnter2D(Collider2D other)
{
    if (other.tag == "Player")
        {
            //Se è un nemico che non spara attiva le funzioni di attacco come l'animazione
            if(shooter)
            {
            FindObjectOfType<AIEnemyGun>().Shoot();
            } 
            else
            {
            FindObjectOfType<AIEnemyDefault>().Attack();
            }
        
        }

        if (other.tag == "Test")
        {
            //Se è un nemico che non spara attiva le funzioni di attacco come l'animazione
            
            FindObjectOfType<AIEnemyDefault>().Attack();
        
        }
}

    //Quando il player esce dal raggio d'azione del nemico questo ferma l'attacco e si rimettere a fare quello che stava facendo prima
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            if(shooter)
            {
            FindObjectOfType<AIEnemyGun>().StopAttack();
            } 
            else 
            {
            FindObjectOfType<AIEnemyDefault>().StopAttack();
            }

        }
    }
}
