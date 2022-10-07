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
    [SerializeField] int score = 0;
    // Valore dello score

    [SerializeField] TextMeshProUGUI livesText;
    //Variabile del testo della vita
    [SerializeField] TextMeshProUGUI scoreText;
    //Variabile del testo dello score
    [SerializeField] GameObject callFadeIn;
    [SerializeField] GameObject callFadeOut;
    [SerializeField] GameObject centerCanvas;

    
    
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
            StartCoroutine(StartStage());

        }
    }

    void Start() 
    {
        //StartCoroutine(StartStage());
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

#region Ricalcolo Morte

    public void StartDie()
    {
        StartCoroutine(CallGameSession());
        AudioManager.instance.DieMusic();
    }

    IEnumerator CallGameSession()
    {
        yield return new WaitForSeconds(2f);
        ProcessPlayerDeath();
        //Richiama i componenti dello script gamesessione e 
        //ne attiva la funzione di processo di morte 

    }

#endregion

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
        StartCoroutine(Restart());
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

    IEnumerator Restart()
    {
        //FadeAnimation.instance.OnFadeIn();
        Instantiate(callFadeIn, centerCanvas.transform.position, centerCanvas.transform.rotation);
        yield return new WaitForSeconds(5f);
        livesText.text = playerLives.ToString();
        //Le vite del player vengono aggiornate
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Lo scenario assume il valore della build
        SceneManager.LoadScene(currentSceneIndex);
        //fade.gameObject.SetActive(false);
        //Lo scenario viene ricaricato
    }

    IEnumerator StartStage()
    {
        //fade.gameObject.SetActive(true);
        AudioManager.instance.playMusic();
        //FadeAnimation.instance.OnFadeOut();
        Instantiate(callFadeOut, centerCanvas.transform.position, centerCanvas.transform.rotation);
        PlayerMovement.instance.ReactivatePlayer();
        PlayerMovement.instance.playerStopInput();
        yield return new WaitForSeconds(5f);
        PlayerMovement.instance.playerActivateInput();
        //fade.gameObject.SetActive(false);
    }

}

