using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    //Variabile per il suono
    [SerializeField] int pointsForCoinPickup = 100;
    //Valore della moneta quando raccolta
    
    bool wasCollected = false;
    //Bool per evitare che la moneta sia raccolta più volte

    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.tag == "Player" && !wasCollected)
        //Se il player tocca la moneta e non è stato collezionata
        {
            wasCollected = true;
            //La moneta è collezionata
            FindObjectOfType<GameSession>().AddToScore(pointsForCoinPickup);
            //Richiama la funzione dello script GameSessione e aumenta lo score
            AudioSource.PlayClipAtPoint(coinPickupSFX, Camera.main.transform.position);
            //Attiva il suono
            gameObject.SetActive(false);
            //Disattiva l'oggetto
            Destroy(gameObject);
            //Lo distrugge
        }
    }
}
