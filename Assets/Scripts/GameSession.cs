using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3; 
    // Numero vite del player
    [SerializeField] int score = 0;
    // Valore dello score

    [SerializeField] TextMeshProUGUI livesText;
    //Variabile del testo della vita
    [SerializeField] TextMeshProUGUI scoreText;
    //Variabile del testo dello score
    
    void Awake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        //Preparazione Singleton della game session
        if (numGameSessions > 1)
        //Se la  game session è maggiore di 1
        {
            Destroy(gameObject);
            //Distrugge quest'oggetto
        }
        else
        //Altrimenti
        {
            DontDestroyOnLoad(gameObject);
            //Preserva quest'oggetto
        }
    }

    void Start() 
    {
        livesText.text = playerLives.ToString();
        //Il testo assume il valore delle vite del player
        scoreText.text = score.ToString();    
        //Il testo assume il valore dello score
    }

    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        //Se le vita del player sono maggiori di 1
        {
            TakeLife();
            //Richiama la funzione
        }
        else
        //Altrimenti
        {
            ResetGameSession();
            //Richiama la funzione di reset
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        score += pointsToAdd;
        //Lo score aumenta
        scoreText.text = score.ToString(); 
        //il testo dello score viene aggiornato
    }

    void TakeLife()
    {
        playerLives--;
        //Il player perde 1 vita
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Lo scenario assume il valore della build
        SceneManager.LoadScene(currentSceneIndex);
        //Lo scenario viene ricaricato
        livesText.text = playerLives.ToString();
        //Le vite del player vengono aggiornate
    }

    void ResetGameSession()
    {
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        //Recupera i componenti dallo script della scena persistente
        //E ne attiva la funzione
        SceneManager.LoadScene(0);
        //Il gioco riparte dallo scenario 0 indicato nella build manager
        Destroy(gameObject);
        //L'oggetto viene distrutto

    }
}
