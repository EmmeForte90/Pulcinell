﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public static PauseMenu instance;

    public string levelSelect, mainMenu;

    private void Awake()
    {
        instance = this;
    }

    public void LevelSelect()
    {
        SceneManager.LoadScene(levelSelect);


    }

    public void MainMenu()
    {
        SceneManager.LoadScene(mainMenu);
        Time.timeScale = 1;
    }

}
