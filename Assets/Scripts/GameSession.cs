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

    
    [Header("Money")]
    [SerializeField] int playerLives = 3; 
    // Numero vite del player
    [SerializeField] int money = 0;
    // Valore dello money
    [SerializeField] bool isTImeline;

    [SerializeField] TextMeshProUGUI livesText;
    //Variabile del testo della vita
    [SerializeField] TextMeshProUGUI moneyText;
    //Variabile del testo dello money
    
    [Header("Fade")]
    [SerializeField] GameObject callFadeIn;
    [SerializeField] GameObject callFadeOut;
    [SerializeField] GameObject centerCanvas;

    [Header("Music")]
    [SerializeField] public AudioSource bgm, dieMusic;
    [SerializeField] bool isStartGame;

    
    [Header("GameOver")]
    [SerializeField] public GameObject gameOver;

#region Singleton
        public static GameSession instance;
        public static GameSession Instance
        {
            //Se non trova il componente lo trova in automatico
            get
            {
                if (instance == null)
                    instance = GameObject.FindObjectOfType<GameSession>();
                return instance;
            }
        }
        #endregion
    
    void Awake()
    {
        if(!isStartGame)
        {
            playMusic();
        }
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
            StartPlay();

        }
    }
    

    void Start() 
    {
        //StartCoroutine(StartStage());
        livesText.text = playerLives.ToString();
        //Il testo assume il valore delle vite del player
        moneyText.text = money.ToString();    
        //Il testo assume il valore dello money
    }

    public void StartPlay()
    {
        StartCoroutine(StartStage());
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
            StartCoroutine(AfterDie());
            //ResetGameSession();
            //Richiama la funzione di reset
        }
    }

#region Ricalcolo Morte

    public void StartDie()
    {
        StartCoroutine(CallGameSession());
        DieMusic();
    }

    IEnumerator CallGameSession()
    {
        yield return new WaitForSeconds(2f);
        ProcessPlayerDeath();
        //Richiama i componenti dello script gamesessione e 
        //ne attiva la funzione di processo di morte 

    }

#endregion


    public void AddTomoney(int pointsToAdd)
    {
        money += pointsToAdd;
        //Lo money aumenta
        moneyText.text = money.ToString(); 
        //il testo dello money viene aggiornato
    }

    void TakeLife()
    {
        playerLives--;
        livesText.text = playerLives.ToString();
        //Il player perde 1 vita
        StartCoroutine(Restart());
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

    public void DeactiveGameOver()
    {
        gameOver.gameObject.SetActive(false);
        playerLives = 3;
        Time.timeScale = 1;
        //playMusic();
        StartCoroutine(Restart());
    }


    IEnumerator AfterDie()
    {
        //FadeAnimation.instance.OnFadeIn();
        yield return new WaitForSeconds(2f);
        gameOver.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    IEnumerator Restart()
    {
        //FadeAnimation.instance.OnFadeIn();
        Instantiate(callFadeIn, centerCanvas.transform.position, centerCanvas.transform.rotation);
        yield return new WaitForSeconds(5f);
        //playMusic();
        livesText.text = playerLives.ToString();
        //Le vite del player vengono aggiornate
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //Lo scenario assume il valore della build
        SceneManager.LoadScene(currentSceneIndex);
        //fade.gameObject.SetActive(false);
        //Lo scenario viene ricaricato
        playMusic();

    }


    IEnumerator StartStage()
    {
        //fade.gameObject.SetActive(true);
        playMusic();
        //FadeAnimation.instance.OnFadeOut();
        if(!isStartGame)
        {
        Instantiate(callFadeOut, centerCanvas.transform.position, centerCanvas.transform.rotation);
        }
        FindObjectOfType<PlayerMovement>().ReactivatePlayer();
        yield return new WaitForSeconds(5f);
        //FindObjectOfType<PlayerMovement>().playerStopInput();
        //fade.gameObject.SetActive(false);
    }


#region Music

    public void stopMusic()
    {
        bgm.Stop();
    }
     public void playMusic()
    {
        bgm.Play();
    }
    
    public void DieMusic()
    {
        bgm.Stop();
        dieMusic.Play();
    }

    #endregion

}

