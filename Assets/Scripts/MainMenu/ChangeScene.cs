using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public string startScene;
    public float Timelife;


public void SceneLoad()
{        
        StartCoroutine(FinishVideo());
}

    IEnumerator FinishVideo()
    {
        yield return new WaitForSeconds(Timelife);
        SceneManager.LoadScene(startScene);
    }
}
