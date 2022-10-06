using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Spine.Unity.AttachmentTools;
using Spine.Unity;
using Spine;

public class GameSession : MonoBehaviour
{
    [SerializeField] int playerLives = 3; 
    // Numero vite del player
    [SerializeField] int money = 0;
    // Valore dello money
    [SerializeField] TextMeshProUGUI livesText;
    //Variabile del testo della vita
    [SerializeField] TextMeshProUGUI scoreText;
    //Variabile del testo dello money
    [SerializeField]  GameObject fade;
    
    
    void Awake()
    {
        livesText.text = playerLives.ToString();
        //Il testo assume il valore delle vite del player
        scoreText.text = money.ToString();    
        //Il testo assume il valore dello money
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        //Preparazione Singleton della game session
        StartCoroutine(StartStage());
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
            GameOver();
        }
    }

    public void AddToScore(int pointsToAdd)
    {
        money += pointsToAdd;
        //Lo money aumenta
        scoreText.text = money.ToString(); 
        //il testo dello money viene aggiornato
    }

    void TakeLife()
    {
        playerLives--;
        //Il player perde 1 vita
        StartCoroutine(Restart());
    }
    IEnumerator StartStage()
    {
    fade.gameObject.SetActive(true);
    PlayerMovement.instance.playerStopInput();
    yield return new WaitForSeconds(3f);
    PlayerMovement.instance.playerActivateInput();
    fade.gameObject.SetActive(false);
    }

    IEnumerator Restart()
    {
        fade.gameObject.SetActive(true);
        PlayerMovement.instance.playerStopInput();
        yield return new WaitForSeconds(3f);
        livesText.text = playerLives.ToString();
        //Le vite del player vengono aggiornate
         int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Lo scenario assume il valore della build
        SceneManager.LoadScene(currentSceneIndex);
        //Lo scenario viene ricaricato
        PlayerMovement.instance.playerActivateInput();
        fade.gameObject.SetActive(false);
    }

    void GameOver()
    {
        PlayerMovement.instance.GameOver();
    }

    public void ResetGameSession()
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
