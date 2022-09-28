using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    //Rilevatore nemico, quando il player entra nel raggio d'azione di questo rilevatore il nemico inizia a sparare
    [Header("Rilevatore")]
    [SerializeField]
    public Transform Detector;
    DatabaseEnemy Enemies;
    [SerializeField]
    public bool shooter;

    void Start()
    {
        Enemies = FindObjectOfType<AIEnemyDefault>();
    }


//Il player entra nel raggio d'azione del nemico
    private void OnTriggerStay2D(Collider2D other)
    {
        //waitaftershot();
        if (other.tag == "Player")
        {
            //Se è un nemico che spara attiva le funzioni di sparo come l'animazione
            if(shooter)
            {
                FindObjectOfType<DatabaseEnemy>().Shoot();
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
            
            FindObjectOfType<DatabaseEnemy>().Attack();
        
        }
}

    //Quando il player esce dal raggio d'azione del nemico questo ferma l'attacco e si rimettere a fare quello che stava facendo prima
    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.tag == "Player")
        {
            FindObjectOfType<DatabaseEnemy>().StopAttack();
        }
    }
}
