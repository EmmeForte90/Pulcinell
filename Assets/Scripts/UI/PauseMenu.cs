using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;


    public string levelSelct, mainMenu;

    public GameObject pauseScreen;

    public bool isPaused;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            PauseUnpause();
        }


    }

    public void PauseUnpause()
    {
        if (isPaused)
        {
            isPaused = false;
            //PlayerController.instance.stopInput = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;

        }
        else
        {
            isPaused = true;
           // PlayerController.instance.stopInput = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0f;
        }

    }

    public void PauseUnscene()
    {
        if (isPaused)
        {
            isPaused = false;

        }
        else if(!isPaused)
        {
            isPaused = true;
            //PlayerController.instance.stopInput = true;

        }

    }



    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelct);


    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1;
    }

}
