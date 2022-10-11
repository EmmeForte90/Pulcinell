using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelExit : MonoBehaviour
{
    //[SerializeField] float levelLoadDelay = 1f;
    //Variabile del ritardo
    [Header("Fade")]
    [SerializeField] GameObject callFadeIn;
    [SerializeField] GameObject callFadeOut;
    [SerializeField] GameObject centerCanvas;
    [SerializeField] string startScene;
    [SerializeField] float Timelife;


    public void SceneLoad()
    {       
        StartCoroutine(Finishstage());
    }

    IEnumerator Finishstage()
    {
        Instantiate(callFadeIn, centerCanvas.transform.position, centerCanvas.transform.rotation);
        FindObjectOfType<PlayerMovement>().playerStopInput();
        yield return new WaitForSeconds(Timelife);
        FindObjectOfType<ScenePersist>().ResetScenePersist();
        SceneManager.LoadScene(startScene);
        FindObjectOfType<PlayerMovement>().playerActivateInput();

    }


    void OnTriggerEnter2D(Collider2D other) 
    {        
        if (other.tag == "Player")
        //Se l'elemento tocca il player
        {
            StartCoroutine(Finishstage());
            //StartCoroutine(LoadNextLevel());
            //Parte il conteggio
        }
    }
/*
    IEnumerator LoadNextLevel()
    {
        yield return new WaitForSecondsRealtime(levelLoadDelay);
        //Il gioco aspettare il tempo indicato dalla variabile
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        //Assume il livello dal build manager e ne aggiunge 1

        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        //Se la prossima scena ha lo stesso valore del conteggio del build setting
        {
            nextSceneIndex = 0;
            //Il suo valore è zero e riparte il gioco dallo scenario 0
        }

        FindObjectOfType<ScenePersist>().ResetScenePersist();
        //Attiva la funzione presente nello script ScenePersist
        SceneManager.LoadScene(nextSceneIndex);
        //Carica lo scenario in base al valore della variabile
    }
    */
}
