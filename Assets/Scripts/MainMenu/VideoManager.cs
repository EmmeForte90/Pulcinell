using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VideoManager : MonoBehaviour
{
    public string startScene;
    public float Timelife;
    public Image fadeScreen;
    public float fadeSpeed;
    private bool shouldFadeToBlack, shouldFadeFromeBack;

    void Start()
    {
        StartCoroutine(FinishVideo());
    }
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(startScene);
        }*/

        if (shouldFadeToBlack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 1f)
            {
                shouldFadeToBlack = false;
            }
        }

        if (shouldFadeFromeBack)
        {
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));

            if (fadeScreen.color.a == 0f)
            {
                shouldFadeFromeBack = false;
            }
        }
    }

    public void FadeToBlack()
    {
        shouldFadeToBlack = true;
        shouldFadeFromeBack = false;
    }

    public void FadeFromBlack()
    {
        shouldFadeFromeBack = true;
        shouldFadeToBlack = false;
    }

    IEnumerator FinishVideo()
    {

        yield return new WaitForSeconds(Timelife);
        SceneManager.LoadScene(startScene);
    }
}