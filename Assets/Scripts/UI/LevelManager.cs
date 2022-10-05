using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public float waitToRespawn;

    public int pizzaCollected;

   

    public string levelToLoad;

    public float timeInLevel;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        timeInLevel = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        timeInLevel += Time.deltaTime;
    }

    public void RespawnPlayer()
    {
        StartCoroutine(RespawnCo());
    }

    private IEnumerator RespawnCo()
    {
        //CameraController.instance.stopFollow = true;
        //PlayerController.instance.stopInput = true;
        yield return new WaitForSeconds(1f);
        AudioManager.instance.PlaySFX(8);
        UIController.instance.FadeToBlack();
        yield return new WaitForSeconds(waitToRespawn - (1f / UIController.instance.fadeSpeed));

        
       // PlayerController.instance.gameObject.SetActive(false);


        //Platformdatabase.instance.Spawn();

        //PlayerController.instance.transform.position = CheckpointController.instance.spawnPoint;
        //PlayerController.instance.gameObject.SetActive(true);

        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + .1f);

        UIController.instance.FadeFromBlack();
       // CameraController.instance.stopFollow = false;
       // PlayerController.instance.stopInput = false;

        PlayerHealthController.instance.currentHealth = PlayerHealthController.instance.maxHealth;
        UIController.instance.UpdateHealthDisplay();
    }

    public void EndLevel()
    {
        StartCoroutine(EndLevelCo());
    }

    public IEnumerator EndLevelCo()
    {
        //AudioManager.instance.PlayLevelVictory();

       // PlayerController.instance.stopInput = true;

        //CameraController.instance.stopFollow = true;

        UIController.instance.levelCompleteText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        UIController.instance.FadeToBlack();

        yield return new WaitForSeconds((1f / UIController.instance.fadeSpeed) + 1f);

        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_unlocked", 1);
        PlayerPrefs.SetString("CurrentLevel", SceneManager.GetActiveScene().name);

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_gems"))
        {
            if(pizzaCollected > PlayerPrefs.GetInt(SceneManager.GetActiveScene().name + "_gems"))
            {
                PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", pizzaCollected);
            }
        }
        else
        {
            PlayerPrefs.SetInt(SceneManager.GetActiveScene().name + "_gems", pizzaCollected);
        }

        if (PlayerPrefs.HasKey(SceneManager.GetActiveScene().name + "_time"))
        {
            if(timeInLevel < PlayerPrefs.GetFloat(SceneManager.GetActiveScene().name + "_time"))
            {
                PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
            }
        }
        else
        {
            PlayerPrefs.SetFloat(SceneManager.GetActiveScene().name + "_time", timeInLevel);
        }

        SceneManager.LoadScene(levelToLoad);
    }
}
