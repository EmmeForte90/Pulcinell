using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinPickupSFX;
    //Variabile per il suono
    [SerializeField] public int pointsForCoinPickup;
    //Valore della moneta quando raccolta
    [SerializeField] public float loadDelay = 0.5f;
    //Temo di recupero
    [SerializeField] public Animator myAnimator;
    //Animatore
    bool wasCollected = false;
    //Bool per evitare che la moneta sia raccolta più volte

void Start()
{
    myAnimator = GetComponent<Animator>();
    //Recupera i componenti dell'animator
}


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
            //Avvia l'audio
            myAnimator.SetTrigger("take");
            //Attiva il suono
            Invoke("takeCoin", loadDelay);
            
        }
    }

    void takeCoin()
    {
        gameObject.SetActive(false);
        //Disattiva l'oggetto
        Destroy(gameObject);
        //Lo distrugge
    }
}
